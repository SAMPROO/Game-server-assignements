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
        public Task<Match> CreateMatch(Player player1, Player player2)
        {
            throw new NotImplementedException();
        }
        public Task<Match[]> GetAll()
        {
            throw new NotImplementedException();
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