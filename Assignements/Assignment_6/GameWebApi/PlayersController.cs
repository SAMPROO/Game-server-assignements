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

        // Assignment 6 Ex.1
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

        // Assignment 6 Ex.2
        [HttpGet("{id:guid}")]
        public Task<Player> Get(Guid id)
        {   
            return _repository.Get(id);
        }

        // Assignment 6 Ex.2
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

        // Assignment 6 Ex.3
        [HttpGet("{tag:int}")]
        public Task<List<Player>> GetByTag(int tag)
        {
            return Task.Run( () => {

                List<Player> playersList = new List<Player>();
                var players = _repository.GetAll().Result;
                foreach(var player in players)
                {
                    if((int)player.Tag == tag)
                    {
                        playersList.Add(player);
                    }
                }
                return playersList;
            } );
        }

        // Assignment 6 Ex.4
        [Route("withitem")]
        [HttpGet]
        public Task<Player[]> GetAll([FromQuery]ItemType type)
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

        // Assignment 6 Ex.5
        [Route("GetPlayersByItemAmount/{count:int}")]
        [HttpGet]
        public Task<List<Player>> GetPlayersByItemAmount(int count)
        {
            return Task.Run( () => {
                    var players = _repository.GetAll().Result;
                    List<Player> playersWithCountItems = new List<Player>();
                    foreach(var player in players)
                    {
                        if(player.Items.Count >= count)
                        {
                            playersWithCountItems.Add(player);
                        }
                    }
                    return playersWithCountItems;
                    } );
        }

        // Assignment 6 Ex.6
        [Route("{id:guid}/updatename/{name}")]
        [HttpPut]
        public Task<Player> UpdateName(Guid id, string name)
        {
            return _repository.UpdateNameDirect(id,name);
        }
        
        // Assignment 6 Ex.7
        [Route("{id}/IncrementScore/{increment:int}")]
        [HttpPut]
        public Task<Player> IncrementPlayerScore(Guid id, int increment)
        {
            return _repository.IncrementPlayerScore(id, increment);
        }
        
        // Assignment 6 Ex.10
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