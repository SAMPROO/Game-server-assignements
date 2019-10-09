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
        [HttpGet("{id}")]
        public Task<Match> Get(Guid id)
        {
            return _repository.Get(id);
        }

        [Route("create")]
        [HttpPost]
        public Task<Match> CreateMatch([FromBody]NewPlayer[] players)
        {   
            if(players.Length == 2)
                return _repository.CreateMatch(players[0], players[1]);
            else
            {
                throw new Exception();
                //throw new InvalidMatchParametersException;
                Console.WriteLine("Invalid Match parameters");
            } 
        }
        [HttpGet("{matchId}/{playerId}")]
        public Task<Ship[]> GetPlayerShips(Guid matchId,Guid playerId)
        {
            return _repository.GetPlayerShips(matchId,playerId);
        }
        [HttpPost("{matchId}/{playerId}")]
        public Task<Ship[]> AddShip(Guid matchId,Guid playerId,[FromBody]Coordinate[] coordinates)
        {
            Console.WriteLine(coordinates.Length);
            if(coordinates.Length == 2)
            {
                
                return _repository.AddShip(matchId,playerId,coordinates[0],coordinates[1]);
            }
                
            else
                throw new Exception();
        }

        [HttpPut("{matchId}/{playerId}")]
        public Task<ActionReport> Shoot(Guid matchId,Guid playerId, [FromBody]Coordinate pos)
        {

            return _repository.DestroyPiece(matchId,pos);


            return Task.Run( () => {
                    return new ActionReport(0);
                });
            
            
        }
        [HttpPut("DeleteAll")]
        public Task<bool> DeleteAll()
        {
            return _repository.DeleteAll();
        }

    }
}