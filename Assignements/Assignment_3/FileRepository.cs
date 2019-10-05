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
        //string json = JsonConverter.SerializeObject(account, Formatting.Indented);
        //Console.WriteLine(json);
        Player[] players;

        /*
        Player p = new Player
        {
            Id = new Guid(),
            Name = "",
            Score = 10,
            Level = 10,
            IsBanned = true,
            CreationTime = new DateTime()
        };
         */
        public Task<Player> Create(NewPlayer np)
        {
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
            var json = JsonConvert.SerializeObject(new { player = newPlayer }, Formatting.Indented);
            Console.WriteLine(json);
            // Write
            System.IO.File.AppendAllText(dataFilePath, json);

            return null;
        }

        public Task<Player> Delete(Guid id)
        {
            var jsonData = System.IO.File.ReadAllText(dataFilePath);
            Players playerList = JsonConvert.DeserializeObject<Players>(jsonData);

            var myJson =  JArray.Parse(jsonData);
            myJson.Descendants()
                .OfType<JProperty>()
                .Where(attr => attr.Name.StartsWith("Id"))
                .ToList();
                .ForEach(attr => attr.Remove());

            JObject jo = JObject.Parse(jsonData);
            jo.Property("ResponseType").Remove();
            json = jo.ToString();

            Console.WriteLine(playerList);

            /*
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
                    Console.WriteLine("Id found, deleting..");
                    idFound = true;
                    break;
                }
                else
                {
                    // Id found, "delete".
                    Console.WriteLine("Id found, deleting..");
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
             */
        
            return null;
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
            [JsonProperty("player")]
            public Players[] players;
        }

        class Players
        {
            [JsonProperty("Id")]
            public Guid Id;
            [JsonProperty("Name")]
            public string name;
            [JsonProperty("Score")]
            public int Score;
            [JsonProperty("Level")]
            public int Level;
            [JsonProperty("IsBanned")]
            public bool IsBanned;
            [JsonProperty("CreationTime")]
            public DateTime CreationTime;
        }
    }
}