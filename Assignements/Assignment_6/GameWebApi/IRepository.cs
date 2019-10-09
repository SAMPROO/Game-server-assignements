using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace dotnetKole 
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player> Get(string name);
        Task<List<Player>> GetByTag(int tag);
        Task<List<Player>> GetWithItemType(int type);
        Task<List<Player>> GetPlayersByItemAmount(int amount);
        Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score);
        Task<Player[]> GetAll();
        Task<List<Player>> GetAll(int minScore = 0);
        Task<Player[]> GetAllOver(int minLevel);
        Task<Player> Create(NewPlayer player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);
        Task<Player[]> GetAllSortedByScoreDescending();
        Task<Player> UpdateNameDirect(Guid id, string name);
        Task<NewItem> CreateItem(Guid playerId, NewItem item);
        //Task<Player> AddItemToPlayer(Guid playerId, NewItem item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Guid item, ModifiedItem modifiedItem);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);
        Task<Player> IncrementPlayerScore(Guid id, int increment);

        void SetEnvironment(IHostingEnvironment env);
        
}
}
