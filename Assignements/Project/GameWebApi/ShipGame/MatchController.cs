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

        [HttpGet]
        public Task<Match[]> GetAll()
        {
            Console.WriteLine("Tried to get");
            return _repository.GetAll();
        }
        [Route("create")]
        [HttpPut]
        public Task<Match> CreateMatch([FromBody]Player[] players)
        {
            if(players.Length == 2)
                return _repository.CreateMatch(players[0],players[1]);
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

        [HttpPut("{matchId}/{playerId}")]
        public Task<ActionReport> Shoot(Guid matchId,Guid playerId, [FromBody]Coordinate pos)
        {
            if(_repository.GetPosition(matchId,playerId,pos).Result)
            {
                return _repository.DestroyPiece(matchId,pos);
            }
            else
            {
                return Task.Run( () => {
                    return new ActionReport(0);
                });
            }
            
        }

    }
}