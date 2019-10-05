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
            Console.WriteLine("Player created: " + newPlayer.Id);

            return null;
        }

        public Task<Player> Delete(Guid id)
        {
            var jsonData = System.IO.File.ReadAllText(dataFilePath);
            PlayersList playerList = JsonConvert.DeserializeObject<PlayersList>(jsonData);
            PlayersList removePlayer = new PlayersList(playerList.players.Length - 1);
            Console.WriteLine(playerList.players.Length);
            Console.WriteLine(removePlayer.players.Length);
            
            bool playerDeleted = false;
            for(int i = 0; i < removePlayer.players.Length; i++)
            {
                if(playerList.players[i].Id == id || playerDeleted)
                {
                    removePlayer.players[i] = playerList.players[i++];
                    playerDeleted = true;
                }
                else
                {
                    removePlayer.players[i] = playerList.players[i];
                }
            }
            if(playerDeleted)
            {
                var json = JsonConvert.SerializeObject(removePlayer, Formatting.Indented);

                System.IO.File.WriteAllText(dataFilePath,json);
                Console.WriteLine("Deleted: " + id);
            }
            else
            {
                Console.WriteLine("Player not found: " + id);
            }
            

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