using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetKole
{
    public class FileRepository : IRepository
    {

        String dataFilePath = "game-dev.txt";
        //string json = Newtonsoft.Json.JsonConverter.SerializeObject(account, Formatting.Indented);
        //Console.WriteLine(json);
        Player[] players;

        Player p = new Player
        {
            Id = new Guid(),
            Name = "",
            Score = 10,
            Level = 10,
            IsBanned = true,
            CreationTime = new DateTime()
        };

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

                    // Read
                    var jsonData = System.IO.File.ReadAllText(dataFilePath);
                    // Convert to JSON
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { player = newPlayer });
                    // Write
                    System.IO.File.WriteAllText(json, jsonData);

                    return newPlayer;
            });
        }

        public Task<Player> Delete(Guid id)
        {
            return Task.Run(()=>{
                var jsonData = System.IO.File.ReadAllText(dataFilePath);
                PlayersList playerList = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayersList>(jsonData);

                List<Player> listWithoutDeletedPlayer = new List<Player>();

                PlayersList newPlayerList = new PlayersList();
                newPlayerList.players = new Players[playerList.players.Length - 1];
                bool idFound = false;

                for (int i = 0; i < playerList.players.Length; i++)
                {
                    if (playerList.players[i].Id != id && i != playerList.players.Length - 1)
                    {
                        newPlayerList.players[i] = playerList.players[i];
                    }
                    else if (playerList.players[i].Id == id && i == playerList.players.Length - 1)
                    {
                        // If last index has same id, "delete"
                        idFound = true;
                        break;
                    }
                    else
                    {
                        // Id found, "delete".
                        idFound = true;
                        continue;
                    }
                }

                if (idFound == true)
                {
                    // New list to JSON and replace old JSON
                    return null;
                }
                else
                {
                    // Id not found, do nothing
                    Console.WriteLine("Id not found, player does not exist");
                }
            });
        }

        public Task<Player> Get(Guid id)
        {
            return Task.Run(()=>{

                

                foreach(var player in players)
                {
                    if(player.Id == id)
                    {
                        return player;
                    }
                }
                return null;
            });
        }

        public Task<Player[]> GetAll()
        {
            return Task.Run(()=>{
                return players;
            });
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            throw new NotImplementedException();
        }

        class PlayersList
        {
            [Newtonsoft.Json.JsonProperty("player")]
            public Players[] players;
        }

        class Players
        {
            [Newtonsoft.Json.JsonProperty("Id")]
            public Guid Id;
            [Newtonsoft.Json.JsonProperty("Name")]
            public string name;
            [Newtonsoft.Json.JsonProperty("Score")]
            public int Score;
            [Newtonsoft.Json.JsonProperty("Level")]
            public int Level;
            [Newtonsoft.Json.JsonProperty("IsBanned")]
            public bool IsBanned;
            [Newtonsoft.Json.JsonProperty("CreationTime")]
            public DateTime CreationTime;
        }
    }
}