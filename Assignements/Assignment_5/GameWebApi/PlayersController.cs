using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dotnetKole
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController
    {
        private IRepository _repository;

        public PlayersController(IRepository i)
        {
            _repository = i;
        }

        [HttpGet("{id:guid}")]
        public Task<Player> Get(Guid id)
        {   
            return _repository.Get(id);
          
        }
        [HttpGet("{name}")]
        public Task<Player> Get(string name) // Mongoton etsi nimellää toteutus
        {
            return Task.Run( () => {
                    var players = _repository.GetAll().Result;
                    foreach(var player in players)
                    {
                        if(player.Name==name)
                        {
                            return player;
                        }
                    }
                    return null;
                    } );
        }
        [HttpGet]
        public Task<Player[]> GetAll([FromQuery]int minScore = 0)
        {
        
            if(minScore>0)
            { 
                return Task.Run( () => {
                    var players = _repository.GetAll().Result;
                    var playersOver = new List<Player>();
                    foreach(var player in players)
                    {
                        if(player.Score>=minScore)
                        {
                            playersOver.Add(player);
                        }
                    }
                    return playersOver.ToArray();
                    } );
            }
            else
            {
                return _repository.GetAll();
            }
            
        }

        [Route("withitem")]
        [HttpGet]
        public Task<Player[]> GetAll([FromQuery]ItemType type) // tehtava 4
        {
            return Task.Run( ()=> {
                var players = _repository.GetAll().Result;
                var playersWithItemType = new List<Player>();
                foreach(var player in players)
                {
                    foreach(var item in player.Items)
                    {
                        if(item.ItemType == type)
                        {
                            playersWithItemType.Add(player);
                            break;
                        }
                    }
                }
                return playersWithItemType.ToArray();

            });
        }
        [Route("topten")]
        [HttpGet]
        public Task<Player[]> GetAllTopTen() // tehtava 10
        {
            return Task.Run(()=>{
                return _repository.GetAllSortedByScoreDescending(); // Mongo toteutus
            });
            /*
                return Task.Run( () => {                            // Mongoton toteutus
                    var players = _repository.GetAll().Result;
                    var playersTopTen = new List<Player>();
                    foreach(var player in players)
                    {   
                        if(playersTopTen.Count<10)
                            playersTopTen.Add(player);
                    }
                    var playersTopTenSorted = playersTopTen.OrderByDescending(x => x.Score);
                return playersTopTenSorted.ToArray();
                } );
        */
        }
        
        [HttpPost]
        public Task<Player> Create([FromBody]NewPlayer player)
        {
            return _repository.Create(player);
        }

        [HttpPut("{id}")]
        public Task<Player> Modify(Guid id,[FromBody]ModifiedPlayer player)
        {
            return _repository.Modify(id, player);
        }
        [HttpDelete("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _repository.Delete(id);
        }
    }
}