using System;

namespace Assignment_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set;}
        public int Score{get; set;}
        public int Level {get;set;}
        public bool IsBanned{set;get;}
        public DateTime CreationTime{get;set;}

    }
    public class NewPlayer
    {
        public string Name{get;set;}
    }
    public class ModifiedPlayer
    {
        public int Score{get;set;}
    }

    public interface IRepository
    {
        Task<Player> GetTask(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);
    }
    public class PlayersController 
    {
        Player[] players;
        public IRepository repository;
        public PlayersController(IRepository repository)
        {
            this.repository = repository;
        }
        public Task<Player> Get(Guid id)
        {
            
            return Task.Run(()=>{

                foreach(var player in players)
                {
                    if(player.Guid == id)
                    {
                        return player;
                    }
                }

            });
        }
        public Task<Player[]> GetAll()
        {
            return Task.Run(()=>{
                return players;
            });
        }

        public Task<Player> Create(NewPlayer np)
        {
            return Task.Run(()=>{
                Player player = NewPlayer;
                player.Name = np.Name;
                return player;
            });
        }
        public Task<Player> Modify(Guid id,ModifiedPlayer player)
        {
            
        }

        public Task<Player> Delete(Guid id)
        {
            
        }
    }
}
