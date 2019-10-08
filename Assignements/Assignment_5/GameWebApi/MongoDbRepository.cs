

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
        public async Task<Player> CreatePlayer(Player player)
        {
            await _collection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player[]> GetAllPlayers()
        {
            var players = await _collection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public Task<Player> GetPlayer(Guid id)
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


        public Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score)
        {
            var pull = Builders<Player>.Update.PullFilter(p => p.Items, i => i.Id == itemId);
            var inc = Builders<Player>.Update.Inc(p => p.Score, score);
            var update = Builders<Player>.Update.Combine(pull, inc);
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);

            return _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            await _collection.ReplaceOneAsync(filter, player);
            return player;
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

        public async Task<Player> DeletePlayer(Guid playerId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            return await _collection.FindOneAndDeleteAsync(filter);
        }

        public Task<NewItem> CreateItem(Guid playerId, NewItem item)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach (var player in playerList.players)
                {
                    if (player.Id == playerId)
                    {
                        Item createdItem = new Item();
                        createdItem.ItemType = item.ItemType;
                        createdItem.Price = item.Price;
                        createdItem.Id = Guid.NewGuid();
                        player.Items.Add(createdItem);
                        var json = JsonConvert.SerializeObject(playerList,Formatting.Indented);
                        System.IO.File.WriteAllText(dataFilePath,json);
                        
                        return item;
                    }
                }
                return null;
            });
        }

        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach (var player in playerList.players)
                {
                    if (player.Id == playerId)
                    {
                        for (int index = 0; index < player.Items.Count; index++)
                        {
                            if (player.Items[index].Id == itemId)
                            {   
                                player.Items.RemoveAt(index);
                                return player.Items[index];
                            }
                        }
                    }
                }
                return null;
            });
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach (var player in playerList.players)
                {
                    if (player.Id == playerId)
                    {
                        return player.Items.ToArray();
                    }
                }
                return null;
            });
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach (var player in playerList.players)
                {
                    if (player.Id == playerId)
                    {
                        foreach (var item in player.Items)
                        {
                            if (item.Id == itemId)
                            {
                                return item;
                            }
                        }
                    }
                }
                return null;
            });
        }

        public Task<Item> UpdateItem(Guid playerId, Item item, ModifiedItem modifiedItem)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach (var player in playerList.players)
                {
                    if (player.Id == playerId)
                    {
                        for (int index = 0; index < player.Items.Count; index++)
                        {
                            if (player.Items[index].Id == item.Id)
                            {   
                                player.Items[index].Level = modifiedItem.Price;
                                player.Items[index].ItemType = modifiedItem.ItemType;
                                return player.Items[index];
                            }
                        }
                    }
                }
                return null;
            });
        }

        public class PlayersList
        {
            public PlayersList()
            {

            }

            public PlayersList(int i)
            {
                players = new Player[i];
            }

            [JsonProperty("players")]
            public Player[] players;
        }
    }
}
