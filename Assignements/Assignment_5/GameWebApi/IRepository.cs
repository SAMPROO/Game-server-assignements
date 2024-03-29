using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace dotnetKole 
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(NewPlayer player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);

        Task<NewItem> CreateItem(Guid playerId, NewItem item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Guid item, ModifiedItem modifiedItem);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);

        void SetEnvironment(IHostingEnvironment env);
        
}
}
