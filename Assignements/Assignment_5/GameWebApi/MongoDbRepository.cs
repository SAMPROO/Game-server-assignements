using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using MongoDB.Bson;

namespace dotnetKole
{
    class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> _collection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Game");
            _collection = database.GetCollection<Player>("players");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }
        public async Task<Player> Create(NewPlayer player)
        {
            Player newPlayer = new Player();
            Guid playerId = Guid.NewGuid();
            newPlayer.Id = playerId;
            newPlayer.Name = player.Name;
            newPlayer.CreationTime = DateTime.Now;
            newPlayer.Level = 0;
            newPlayer.Score = 0;
            newPlayer.IsBanned = false;
        
            await _collection.InsertOneAsync(newPlayer);
            return newPlayer;
        }

        public async Task<Player[]> GetAll()
        {
            var players = await _collection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();    
    
        }
        public async Task<Player[]> GetAllOver(int minLevel)
        {
            var players = await _collection.Find(new BsonDocument()).ToListAsync();
            var playersOverLevel = new List<Player>();
            foreach(var player in players)
            {
                if(player.Level>minLevel)
                {
                    playersOverLevel.Add(player);
                }
            }
            return playersOverLevel.ToArray();
        }

        public Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return _collection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetBetweenLevelsAsync(int minLevel, int maxLevel)
        {
            var filter = Builders<Player>.Filter.Gte(p => p.Level, 18) & Builders<Player>.Filter.Lte(p => p.Level, 30);
            var players = await _collection.Find(filter).ToListAsync();
            return players.ToArray();
        }
        public void SetEnvironment(IHostingEnvironment env)
        {
            throw new NotImplementedException();
        }

        public Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score)
        {
            var pull = Builders<Player>.Update.PullFilter(p => p.Items, i => i.Id == itemId);
            var inc = Builders<Player>.Update.Inc(p => p.Score, score);
            var update = Builders<Player>.Update.Combine(pull, inc);
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);

            return _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<Player> Modify(Guid id,ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player modifiedPlayer = Get(id).Result;
            modifiedPlayer.Score = player.Score;
            await _collection.ReplaceOneAsync(filter, modifiedPlayer);
            return modifiedPlayer;
        }

        public async Task<Player[]> GetAllSortedByScoreDescending()
        {
            var sortDef = Builders<Player>.Sort.Descending(p => p.Score);
            var players = await _collection.Find(new BsonDocument()).Sort(sortDef).ToListAsync();

            return players.ToArray();
        }

        public async Task<Player> IncrementPlayerScore(string id, int increment)
        {
            var filter = Builders<Player>.Filter.Eq("_id", id);
            var incrementScoreUpdate = Builders<Player>.Update.Inc(p => p.Score, increment);
            var options = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };
            Player player = await _collection.FindOneAndUpdateAsync(filter, incrementScoreUpdate, options);
            return player;
        }

        public async Task<Player> Delete(Guid playerId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            return await _collection.FindOneAndDeleteAsync(filter);
        }
        public async Task<NewItem> CreateItem(Guid playerId, NewItem item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Item createdItem = new Item();
            createdItem.ItemType = item.ItemType;
            createdItem.Price = item.Price;
            createdItem.Id = Guid.NewGuid();
            Player player = Get(playerId).Result;
            player.Items.Add(createdItem);

            await _collection.ReplaceOneAsync(filter, player);
            return item;
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = Get(playerId).Result;
            foreach (var item in player.Items)
            {
                if (item.Id == itemId)
                {
                    return item;
                }
            }
            
            return null;
        }

        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = Get(playerId).Result;
            if (player != null)
                return player.Items.ToArray();

            return null;
        }

        public async Task<Item> UpdateItem(Guid playerId, Guid item, ModifiedItem modifiedItem)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player modifiedPlayer = Get(playerId).Result;

            foreach (var i in modifiedPlayer.Items)
            {
                if (item == i.Id)
                {
                    i.Price = modifiedItem.Price;
                    i.ItemType = modifiedItem.ItemType;
                    await _collection.ReplaceOneAsync(filter, modifiedPlayer);
                    return i;
                }
            }

            await _collection.ReplaceOneAsync(filter, modifiedPlayer);
            return null;
        }

        public async Task<Item> DeleteItem(Guid playerId, Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player modifiedPlayer = Get(playerId).Result;
            Item item = null;

            for (int i = 0; i < modifiedPlayer.Items.Count; i++)
            {
                if (modifiedPlayer.Items[i].Id == id)
                {

                    item = modifiedPlayer.Items[i];
                    modifiedPlayer.Items.RemoveAt(i);
                    await _collection.ReplaceOneAsync(filter, modifiedPlayer);
                    return item;
                }
            }

            await _collection.ReplaceOneAsync(filter, modifiedPlayer);
            return item;
        }
        
    }
}
