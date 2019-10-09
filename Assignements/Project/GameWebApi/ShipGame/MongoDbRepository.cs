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

            Match[] matchesArr = matches.ToArray();

            foreach(var mat in matchesArr)
            {
                Console.WriteLine(mat.InProgress);
            }
            return matchesArr; 
        }
        public async Task<Match> CreateMatch(Player player1, Player player2)
        {
            /*
            Match match = new Match();
            match.Player1 = player1;
            match.Player2 = player2;
            */
            Match match = new Match(player1,player2);
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
            var kyrpa = await _collection.DeleteManyAsync("{}");

            return true;
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