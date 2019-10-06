using System.Threading.Tasks;
using System;

namespace dotnetKole
{
    public class ItemsController
    {
        private IRepository _repository;

        public ItemsController(IRepository i)
        {
            _repository = i;
        }

        Task<Item> CreateItem(Guid playerId, Item item)
        {
            return _repository.CreateItem(playerId, item);
        }
        Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return _repository.GetItem(playerId, itemId);
        }
        Task<Item[]> GetAllItems(Guid playerId)
        {
            return _repository.GetAllItems(playerId);
        }
        Task<Item> UpdateItem(Guid playerId, Item item, ModifiedItem modifiedItem)
        {
            return _repository.UpdateItem(playerId, item, modifiedItem);
        }
        Task<Item> DeleteItem(Guid playerId, Item item)
        {
            return _repository.DeleteItem(playerId, item);
        }
    }
}