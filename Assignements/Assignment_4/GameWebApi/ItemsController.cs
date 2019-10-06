using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using static CustomValidation;
using System.Diagnostics.Contracts;

namespace dotnetKole
{
    public class ItemsController
    {
        private IRepository _repository;

        public ItemsController(IRepository i)
        {
            _repository = i;
        }

        //[SwordMinLevel]
        Task<Item> CreateItem(Guid playerId, Item item)
        {
            try
            {
                ValidatePlayerLevelTooLowForSwordException(playerId, item);
            }
            catch (PlayerLevelTooLowForSwordException e)
            {
                Console.WriteLine(e.Message);
            }

            return _repository.CreateItem(playerId, item);
        }

        private void ValidatePlayerLevelTooLowForSwordException(Guid playerId, Item item)
        {
            if (_repository.Get(playerId).Result.Level < 3 && item.ItemType == ItemType.Sword)
                throw new PlayerLevelTooLowForSwordException("Player level is too low");
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