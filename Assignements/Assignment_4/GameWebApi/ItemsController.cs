using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace dotnetKole
{
    [Route("api/players/{playerId}/items")]
    [ApiController]
    public class ItemsController
    {
        private IRepository _repository;

        public ItemsController(IRepository i)
        {
            _repository = i;
        }
     
        
        //[SwordMinLevel]
        [HttpPost]
        [PlayerLevelTooLowForSwordExceptionFilter]
        public Task<NewItem> CreateItem(Guid playerId,[FromBody] NewItem item)
        {
            Console.WriteLine(item.Price+" "+playerId);
            Player player = _repository.Get(playerId).Result;
            if(player.Level < 3 && item.ItemType == ItemType.Sword)
            {
                throw new PlayerLevelTooLowForSwordException();
            }
            //ValidatePlayerLevelTooLowForSwordException(playerId, item);
            return _repository.CreateItem(playerId, item); 
                
        }      
      

        private void ValidatePlayerLevelTooLowForSwordException(Guid playerId, NewItem item)
        {
            if (_repository.Get(playerId).Result.Level < 3 && item.ItemType == ItemType.Sword)
                throw new PlayerLevelTooLowForSwordException();
        }
        [HttpGet("{itemId}")]
        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return _repository.GetItem(playerId, itemId);
        }
        [HttpGet]
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            Console.WriteLine("Getting items");
            return _repository.GetAllItems(playerId);
        }

        //public Task<Item> UpdateItem(Guid playerId, Item item, ModifiedItem modifiedItem)
        //{
        //    return _repository.UpdateItem(playerId, item, modifiedItem);
        //}
        [HttpDelete("{itemId}")]
        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            return _repository.DeleteItem(playerId, itemId);
        }
        
    }
}