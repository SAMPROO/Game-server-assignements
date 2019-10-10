using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ShipGame
{
    [Route("api/matches")]
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
        [Route("Create/{p1}/{p2}")]
        [HttpPost]
        public Task<Match> CreateMatch(string p1, string p2)
        {   
            return _repository.CreateMatch(p1, p2);
        }

        [Route("Create/{playerOneId:guid}/{playerTwoId:guid}")]
        [HttpPost]
        public Task<Match> CreateMatch(Guid playerOneId, Guid playerTwoId)
        {   
            return _repository.CreateMatch(playerOneId, playerTwoId);
        }

        [HttpGet("{matchId}/{playerId}")]
        public Task<Ship[]> GetPlayerShips(Guid matchId,Guid playerId)
        {
            return _repository.GetPlayerShips(matchId,playerId);
        }

        [HttpGet("CheckIfInProgress/{matchId}")]
        public Task<bool> CheckIfInProgress(Guid matchId)
        {
            return _repository.CheckIfInProgress(matchId);
        }

        [Route("GetLiveMatches")]
        [HttpGet]
        public Task<Match[]> GetLiveMatches()
        {
            return _repository.GetLiveMatches();
        }

        /* Example JSON
            [{"X":1,"Y":1},{"X":1,"Y":3}]
        */
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