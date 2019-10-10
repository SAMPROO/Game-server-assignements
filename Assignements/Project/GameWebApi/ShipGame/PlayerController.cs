using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ShipGame
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController
    {
        private IRepository _repository;

        public PlayerController(IRepository i)
        {
            _repository = i;
        }

        [Route("GetAll")]
        [HttpGet]
        public Task<Player[]> GetAllPlayers()
        {
            return _repository.GetAllPlayers();
        }

        [HttpGet("{id:guid}")]
        public Task<Player> GetPlayer(Guid id)
        {   
            return _repository.GetPlayer(id);
        }

        [Route("{name}")]
        [HttpGet]
        public Task<Player[]> GetPlayer(string name)
        {
            return _repository.GetPlayer(name);
        }

        [Route("Create/{name}")]
        [HttpPost]
        public Task<Player> CreatePlayer(string name)
        {   
            return _repository.CreatePlayer(name);
        }

        [Route("CheckIfInMatch/{id:guid}")]
        [HttpPost]
        public Task<bool> CheckIfInMatch(Guid id)
        {   
            return _repository.CheckIfInMatch(id);
        }


        [HttpPut("Delete/{id:guid}")]
        public Task<Player> DeletePlayer(Guid id)
        {
            return _repository.DeletePlayer(id);
        }

        [HttpPut("DeleteAll")]
        public Task<bool> DeleteAllPlayers()
        {
            return _repository.DeleteAllPlayers();
        }

    }
}