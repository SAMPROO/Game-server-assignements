using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ShipGame
{
    [Route("api")]
    [ApiController]
    public class MatchController
    {
        
        private IRepository _repository;

        public MatchController(IRepository i)
        {
            _repository = i;
        }

        [Route("GetAll")]
        [HttpGet]
        public Task<Match[]> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id:guid}")]
        public Task<Match> Get(Guid id)
        {   
            return _repository.Get(id);
        }

        /* Example JSON
        [  
            {  
                "Name" : "Arnold Schwarzenegger"
            },
            {  
                "Name" : "Sylvester Stallone"
            }
        ]
        */
        [Route("create")]
        [HttpPost]
        public Task<Match> CreateMatch([FromBody]NewPlayer[] players)
        {   
            if(players.Length == 2)
                return _repository.CreateMatch(players[0], players[1]);
            else
            {
                throw new NotFoundException("Too many player being created. Only 2 allowed per game.");
            } 
        }
        [HttpGet("{matchId}/{playerId}")]
        public Task<Ship[]> GetPlayerShips(Guid matchId,Guid playerId)
        {
            return _repository.GetPlayerShips(matchId,playerId);
        }
        [HttpPost("{matchId}/{playerId}")]
        public Task<Ship[]> AddShip(Guid matchId,Guid playerId, [FromBody]Coordinate[] coordinates)
        {
            Console.WriteLine(coordinates.Length);
            if(coordinates.Length == 2)
            {
                
                return _repository.AddShip(matchId,playerId,coordinates[0],coordinates[1]);
            }
                
            else
                throw new Exception();
        }

        [HttpPut("{matchId}/{playerId}/Shoot")]
        public Task<Ship[]> Shoot(Guid matchId,Guid playerId, [FromQuery]Coordinate pos)
        {
            return _repository.DestroyPart(matchId,playerId,pos);
        }

        [HttpPut("DeleteAll")]
        public Task<bool> DeleteAll()
        {
            return _repository.DeleteAll();
        }

        [HttpPut("DeleteMatch/{id:guid}")]
        public Task<Match> DeleteMatch(Guid id)
        {
            return _repository.DeleteMatch(id);
        }

    }
}