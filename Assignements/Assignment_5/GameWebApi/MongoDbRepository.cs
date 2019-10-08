using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace dotnetKole
{
    public class MongoDbRepository : IRepository
    {
        public Task<Player> Create(NewPlayer player)
        {
            throw new NotImplementedException();
        }

        public Task<NewItem> CreateItem(Guid playerId, NewItem item)
        {
            throw new NotImplementedException();
        }

        public Task<Player> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<Player> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            throw new NotImplementedException();
        }

        public void SetEnvironment(IHostingEnvironment env)
        {
            throw new NotImplementedException();
        }

        public Task<Item> UpdateItem(Guid playerId, Item item, ModifiedItem modifiedItem)
        {
            throw new NotImplementedException();
        }
    }
}