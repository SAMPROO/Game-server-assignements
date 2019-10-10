using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ShipGame
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Match> _collection;
        private readonly IMongoCollection<Player> _collectionPlayers;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("ShipGame");
            _collection = database.GetCollection<Match>("matches");
            _collectionPlayers = database.GetCollection<Player>("players");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("matches");
        }
        public async Task<Match[]> GetAll()
        {
            var matches = await _collection.Find(new BsonDocument()).ToListAsync();
            return matches.ToArray(); 
        }
        public async Task<Player[]> GetAllPlayers()
        {
            var players = await _collectionPlayers.Find(new BsonDocument()).ToListAsync();
            return players.ToArray(); 
        }
        public async Task<Match> CreateMatch(string player1, string player2)
        {
            Match match = new Match();

            Player p1 = new Player(player1, match.Id);
            Player p2 = new Player(player2, match.Id);

            match.Player1 = p1;
            match.Player2 = p2;

            await _collectionPlayers.InsertOneAsync(p1);
            await _collectionPlayers.InsertOneAsync(p2);
            await _collection.InsertOneAsync(match);
            return match;         
        }

        public async Task<Match> CreateMatch(Guid player1, Guid player2)
        {

            FilterDefinition<Player> playerOneFilter = Builders<Player>.Filter.Eq(p => p.Id, player1);
            FilterDefinition<Player> playerTwoFilter = Builders<Player>.Filter.Eq(p => p.Id, player2);

            var p1Result = await _collectionPlayers.Find(playerOneFilter).FirstOrDefaultAsync();
            var p2Result = await _collectionPlayers.Find(playerTwoFilter).FirstOrDefaultAsync();

            if (p1Result == null)
                throw new NotFoundException(NotFoundException.ErrorType.GUID, player1);
            else if (p2Result == null)
                throw new NotFoundException(NotFoundException.ErrorType.GUID, player2);

            if (await CheckIfInMatch(player1))
                throw new NotFoundException(player1 + " Already in game.");
            else if (await CheckIfInMatch(player2))
                throw new NotFoundException(player2 + " Already in game.");

            Match match = new Match(p1Result, p2Result);
            p1Result.InMatchBool = true;
            p1Result.InMatchBool = true;
            p1Result.InMatchGuid = match.Id;
            p2Result.InMatchGuid = match.Id;

            await _collection.InsertOneAsync(match);
            await _collectionPlayers.ReplaceOneAsync(playerOneFilter, p1Result);
            await _collectionPlayers.ReplaceOneAsync(playerTwoFilter, p2Result);

            return match;
        }
        public async Task<Match> Get(Guid id)
        {
            FilterDefinition<Match> filter = Builders<Match>.Filter.Eq(p => p.Id, id);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            if (result == null)
                throw new NotFoundException(NotFoundException.ErrorType.GUID, id);

            return result;
        }
        public async Task<Player> GetPlayer(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var result = await _collectionPlayers.Find(filter).FirstOrDefaultAsync();
            if (result == null)
                throw new NotFoundException(NotFoundException.ErrorType.GUID, id);

            return result;
        }

        public async Task<Player[]> GetPlayer(string name)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            var result = await _collectionPlayers.Find(filter).ToListAsync();
            if (result == null)
                throw new NotFoundException(NotFoundException.ErrorType.STRING, name);

            return result.ToArray();
        }

        public async Task<Player[]> GetTopPlayer(int top)
        {
            var sortDef = Builders<Player>.Sort.Descending(p => p.Wins);
            var players = await _collectionPlayers.Find(new BsonDocument()).Limit(top).Sort(sortDef).ToListAsync();

            return players.ToArray();
        }
        public async Task<bool> DeleteAll()
        {
            var delete = await _collection.DeleteManyAsync("{}");
            return true;
        }

        public async Task<bool> DeleteAllPlayers()
        {
            var delete = await _collectionPlayers.DeleteManyAsync("{}");
            return true;
        }

        public async Task<Ship[]> AddShip(Guid matchId,Guid playerId, Coordinate start, Coordinate end)
        {
            var match = Get(matchId).Result;
            var filter = Builders<Match>.Filter.Eq(p => p.Id, matchId);
            var newShip = new Ship(start, end);
            Player player = null;
            if (match.Player1.Id == playerId)
            {
                if(match.Player1.Ships == null)
                    match.Player1.Ships = new List<Ship>();
                match.Player1.Ships.Add(newShip);             
                player = match.Player1;
            }
            else if(match.Player2.Id == playerId)
            {   
                if(match.Player2.Ships == null)
                    match.Player2.Ships = new List<Ship>();
                match.Player2.Ships.Add(newShip);
                player = match.Player2;
            }
            else
                throw new NotFoundException(NotFoundException.ErrorType.GUID, playerId);
            await _collection.ReplaceOneAsync(filter, match);
            return player.Ships.ToArray();
        }

        public async Task<Ship[]> DestroyPart(Guid matchId, Guid playerId, Coordinate pos)
        {
            var match = Get(matchId).Result;
            var filter = Builders<Match>.Filter.Eq(p => p.Id, matchId);
            Player enemy = null;
            Player player = null;

            if(match.Player1.Id == playerId)
            {
                enemy = match.Player2;
                player = match.Player1;
            }
            else if(match.Player2.Id == playerId)
            {
                enemy = match.Player1;
                player = match.Player2;
            }
            else
                throw new NotFoundException(NotFoundException.ErrorType.GUID, playerId);

            if(enemy.Ships == null || enemy.Ships.Count == 0)
                await GameOver(player, enemy, match);
            else
            {
                bool partWasDestroyed = false;
                foreach(Ship ship in enemy.Ships)
                {
                    foreach(Coordinate part in ship.ShipParts)
                    {   
                        if(part.X == pos.X && part.Y == pos.Y)
                        {
                            ship.ShipParts.Remove(part);
                            partWasDestroyed = true;
                            break;
                        }
                    }
                    if(ship.ShipParts.Count==0)
                    {
                        partWasDestroyed = true;
                        enemy.Ships.Remove(ship);
                    }
                    if(partWasDestroyed)
                        break;
                }
                if(enemy.Ships.Count == 0)
                {
                    enemy = null;
                    if(match.Player1.Id == playerId)
                        match.Player2 = enemy;
                    else
                        match.Player1 = enemy;
                    match.InProgress = false;
                    await _collection.ReplaceOneAsync(filter,match);
                    return null;
                }
            }
            
            if(match.Player1.Id == playerId)
                match.Player2 = enemy;
            if(match.Player2.Id == playerId)
                match.Player1 = enemy;
        
            await _collection.ReplaceOneAsync(filter, match);
            return enemy.Ships.ToArray();
        }

        public async Task<bool> CheckIfInProgress(Guid matchId)
        {
            var status = await Get(matchId);
            return status.InProgress;
        }

        public async Task<bool> CheckIfInMatch(Guid playerId)
        {
            var player = await GetPlayer(playerId);
            return player.InMatchBool ? true : false;
        }

        public async Task<Match[]> GetLiveMatches()
        {
            FilterDefinition<Match> filter = Builders<Match>.Filter.Eq(p => p.InProgress, true);
            var result = await _collection.Find(filter).ToListAsync();
            if (result == null)
                throw new NotFoundException(NotFoundException.ErrorType.OTHER, "No live games found");

            return result.ToArray();
        }
        
        public async Task<Match> DeleteMatch(Guid matchId)
        {   
            Match match = Get(matchId).Result;
            await _collection.DeleteOneAsync(Builders<Match>.Filter.Eq(p => p.Id, matchId));

            return match;
        }
        public async Task<Player> DeletePlayer(Guid playerId)
        {   
            Player match = GetPlayer(playerId).Result;
            await _collectionPlayers.DeleteOneAsync(Builders<Player>.Filter.Eq(p => p.Id, playerId));

            return match;
        }
        public Task<bool> GetPosition(Guid matchId, Guid playerId, Coordinate pos)
        {
            throw new NotImplementedException();
        }
        public Task<ActionReport> DestroyPiece(Guid matchId,Coordinate pos)
        {
            throw new NotImplementedException();
        }
        public async Task<Ship[]> GetPlayerShips(Guid matchId, Guid playerId)
        {
            var match = await Get(matchId);

            if (match.Player1.Id == playerId)
                return match.Player1.Ships.ToArray();
            else if(match.Player2.Id == playerId)
                return match.Player2.Ships.ToArray();
            else
                throw new NotFoundException(NotFoundException.ErrorType.GUID, playerId);
        }

        public async Task<Player> CreatePlayer(string name)
        {
            Player newPlayer = new Player(name);
            await _collectionPlayers.InsertOneAsync(newPlayer);
            return newPlayer;
        }

        public async Task<Player> GameOver(Player winner, Player looser, Match match)
        {
            FilterDefinition<Player> winnerFilter = Builders<Player>.Filter.Eq(p => p.Id, winner.Id);
            FilterDefinition<Player> looserFilter = Builders<Player>.Filter.Eq(p => p.Id, looser.Id);


            winner.Wins = winner.Wins + 1;
            winner.InMatchBool = false;
            winner.InMatchGuid = Guid.Empty;
            winner.Ships.Clear();

            looser.Losses = looser.Losses + 1;
            looser.InMatchBool = false;
            looser.InMatchGuid = Guid.Empty;
            looser.Ships.Clear();

            await DeleteMatch(match.Id);
            await _collectionPlayers.ReplaceOneAsync(winnerFilter, winner);
            await _collectionPlayers.ReplaceOneAsync(looserFilter, looser);

            return winner;
        }
    }
}