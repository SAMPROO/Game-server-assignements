using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnetKole
{
    public class FileRepository : IRepository
    {
        String dataFilePath = "game-dev.txt";
        
        public Task<Player> Create(NewPlayer np)
        {
            return Task.Run(()=>{
                Player newPlayer = new Player();
                Guid playerId = Guid.NewGuid();

                newPlayer.Id = playerId;
                newPlayer.Name = np.Name;
                newPlayer.CreationTime = new DateTime();
                newPlayer.Level = 0;
                newPlayer.Score = 0;
                newPlayer.IsBanned = false;

                string jsonData = System.IO.File.ReadAllText(dataFilePath);

                PlayersList players = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                PlayersList addPlayer;

                if (players != null)
                {
                    addPlayer = new PlayersList(players.players.Length + 1);

                    for(int i = 0; i < players.players.Length; i++)
                    {
                        addPlayer.players[i] = players.players[i];
                    }
                    addPlayer.players[players.players.Length] = newPlayer;
                }
                else
                {
                    addPlayer = new PlayersList(1);
                    addPlayer.players[0] = newPlayer;
                }

                var json = JsonConvert.SerializeObject(addPlayer,Formatting.Indented);

                System.IO.File.WriteAllText(dataFilePath,json);
                Console.WriteLine("(CREATE) Player created: " + newPlayer.Id);

                return newPlayer;
            });
        }

        public Task<Player> Delete(Guid id)
        {
            return Task.Run(()=>{

                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);
                PlayersList removePlayer = new PlayersList(playerList.players.Length - 1);
                Player deletedPlayer = new Player();
                
                bool playerDeleted = false;
                for(int i = 0; i < removePlayer.players.Length; i++)
                {
                    if(playerList.players[i].Id == id || playerDeleted)
                    {
                        removePlayer.players[i] = playerList.players[i + 1];
                        if(playerDeleted == false)
                        {
                            deletedPlayer = playerList.players[i];
                        }
                        playerDeleted = true;
                    }
                    else
                    {
                        removePlayer.players[i] = playerList.players[i];
                    }
                }

                if(playerList.players[playerList.players.Length - 1].Id == id)
                {
                    playerDeleted = true;
                }

                if(playerDeleted)
                {
                    var json = JsonConvert.SerializeObject(removePlayer, Formatting.Indented);

                    System.IO.File.WriteAllText(dataFilePath,json);
                    Console.WriteLine("(DELETE) Deleted: " + id);
                    return deletedPlayer;
                }
                else
                {
                    Console.WriteLine("(DELETE) Player not found: " + id);
                    return null;
                }
            });
        }

        public Task<Player> Get(Guid id)
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                foreach(var player in playerList.players)
                {
                    if(player.Id == id)
                    {
                        Console.WriteLine("(GET) Player found: " + id);
                        return player;
                    }
                }
                
                Console.WriteLine("(GET) Player not found: " + id);
                return null;
            });
        }

        public Task<Player[]> GetAll()
        {
            return Task.Run(()=>{
                
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);

                if (playerList == null)
                {
                    return null;
                }

                return playerList.players;
            });
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return Task.Run(()=>{
                
                PlayersList playersList = new PlayersList();
                var players = GetAll().Result;

                foreach (var p in players)
                {
                    if (p.Id == id)
                    {
                        p.Score = player.Score;

                        playersList.players = players;
                        var json = JsonConvert.SerializeObject(playersList, Formatting.Indented);
                        System.IO.File.WriteAllText(dataFilePath, json);
                        return p;
                    }
                }
                return null;
            });
        }

        public class PlayersList
        {
            public PlayersList()
            {

            }

            public PlayersList(int i)
            {
                players = new Player[i];
            }

            [JsonProperty("players")]
            public Player[] players;
        }
    }
}