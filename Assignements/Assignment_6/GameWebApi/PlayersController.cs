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
        public Task<Player[]> GetAll()
        {
            return _repository.GetAll();
        }

        // Assignment 6 Ex.1
        [Route("GetAllMinScore/{minScore:int}")]
        [HttpGet]
        public Task<List<Player>> GetAll(int minScore = 0)
        {
            return _repository.GetAll(minScore);
        }

        /*
        public Task<Player[]> GetAll()
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
        */

        // Assignment 6 Ex.2
        [HttpGet("{id:guid}")]
        public Task<Player> Get(Guid id)
        {   
            return _repository.Get(id);
        }
        /* 
        public Task<Player> Get(Guid id)
        {
            return Task.Run( () => {
                var players = _repository.GetAll().Result;
                foreach(var player in players)
                {
                    if(player.Id==id)
                    {
                        return player;
                    }
                }
                return null;
                } );
        */

        // Assignment 6 Ex.2
        [HttpGet("{name}")]
        public Task<List<Player>> Get(string name)
        {   
            return _repository.Get(name);
        }
        /*
        public Task<Player> Get(string name)
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
        */

        // Assignment 6 Ex.3
        [HttpGet("{tag:int}")]
        public Task<List<Player>> GetByTag(int tag)
        {   
            return _repository.GetByTag(tag);
        }

        // Assignment 6 Ex.4
        [Route("WithItem/{type:int}")]
        [HttpGet]
        public Task<List<Player>> GetWithItemType(int type)
        {   
            return _repository.GetWithItemType(type);
        }

        // Assignment 6 Ex.5
        // Needs fixing
        [Route("GetPlayersByItemAmount/{count:int}")]
        [HttpGet]
        public Task<List<Player>> GetPlayersByItemAmount(int count)
        {   
            return _repository.GetPlayersByItemAmount(count);
        }

        /*
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
        */

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

        // Assignment 6 Ex.8 in ItemController

        // Assignment 6 Ex.9
        [Route("{playerId:guid}/Sell/{itemId:guid}/{score:int}")]
        [HttpGet]
        public Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score)
        {
            return _repository.IncreasePlayerScoreAndRemoveItem(playerId, itemId, score);
        }
        
        // Assignment 6 Ex.10
        [Route("topten")]
        [HttpGet]
        public Task<Player[]> GetAllTopTen()
        {
            return _repository.GetAllSortedByScoreDescending();
        }
        /*
        public Task<Player[]> GetAllTopTen()
        {
            return Task.Run( () => {
                var players = _repository.GetAll().Result;
                var playersTopTen = new List<Player>();
                foreach(var player in players)
                    if(playersTopTen.Count<10)
                        playersTopTen.Add(player);
                }
                var playersTopTenSorted = playersTopTen.OrderByDescending(x => x.Score);
                return playersTopTenSorted.ToArray();
            } );
        */

        // Assignment 6 Ex.11
        [Route("GetMostCommonLevel")]
        [HttpGet]
        public Task<int> GetMostCommonLevel()
        {
            return _repository.GetMostCommonLevel();
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