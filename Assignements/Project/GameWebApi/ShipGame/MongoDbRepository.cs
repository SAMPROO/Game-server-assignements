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
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
             var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("ShipGame");
            _collection = database.GetCollection<Match>("matches");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("matches");
        }
        public async Task<Match[]> GetAll()
        {
            var matches =  _collection.Find(new BsonDocument()).ToList();

            return matches.ToArray(); 
        }
        public async Task<Match> CreateMatch(NewPlayer player1, NewPlayer player2)
        {
            Player p1 = new Player();
            Player p2 = new Player();
            p1.Name = player1.Name;
            p1.Id = Guid.NewGuid();
            p1.Ships = new List<Ship>();
            p2.Name = player2.Name;
            p2.Id = Guid.NewGuid();
            p2.Ships = new List<Ship>();
            Match match = new Match(p1,p2);
            match.Id = Guid.NewGuid();
            await _collection.InsertOneAsync(match);
            return match;         
        }
        public async Task<Match> Get(Guid id)
        {
            var filter = Builders<Match>.Filter.Eq(p => p.Id, id);
            return await _collection.Find(filter).FirstAsync();
        }
        public async Task<bool> DeleteAll()
        {
            var delete = await _collection.DeleteManyAsync("{}");

            return true;
        }

        public async Task<Ship[]> AddShip(Guid matchId,Guid playerId, Coordinate start, Coordinate end)
        {
            var filter = Builders<Match>.Filter.Eq(p => p.Id, matchId);
            var newShip = new Ship(start, end);
            var match = Get(matchId).Result;
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
                throw new Exception("Player does not exist");
            await _collection.ReplaceOneAsync(filter, match);
            return player.Ships.ToArray();

            
        }
        public async Task<Ship[]> DestroyPart(Guid matchId, Guid playerId,Coordinate pos)
        {
            var filter = Builders<Match>.Filter.Eq(p => p.Id, matchId);
            var match = Get(matchId).Result;
            if(match == null)
                throw new Exception("match does not exist");
            Player enemy = null;
            if(match.Player1.Id == playerId)
                enemy = match.Player2;

            if(match.Player2.Id == playerId)
                enemy = match.Player1;
            if(enemy != null)
            {
                if(enemy.Ships == null || enemy.Ships.Count == 0)
                    {
                        enemy = null;
                        if(match.Player1.Id == playerId)
                            match.Player2 = enemy;
                        if(match.Player2.Id == playerId)
                            match.Player1 = enemy;
            
                        await _collection.ReplaceOneAsync(filter, match);
                    }
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
            else
            {
                if(enemy == null)
                    throw new Exception("Player does not exist");
                return null;
            }
            
            
        }


        public Task<Match> GetMatchStatus(Guid matchId)
        {
            throw new NotImplementedException();
        }
        public Task<Match> DeleteMatch(Guid matchId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> GetPosition(Guid matchId, Guid playerId, Coordinate pos)
        {
            throw new NotImplementedException();
        }
        public Task<ActionReport> DestroyPiece(Guid matchId,Coordinate pos)
        {
            throw new NotImplementedException();
        }
        public Task<Ship[]> GetPlayerShips(Guid matchId, Guid playerId)
        {
            throw new NotImplementedException();
        }
    }
}