using System;
using System.Threading.Tasks;

namespace dotnetKole
{
    public class PlayersController : IRepository
    {
        public Task<Player> Create(NewPlayer player)
        {
            //Task<Player> p = new Task<Player>();
            //p.Name = player.Name;
            //return p;
            return null;
        }

        public Task<Player> Delete(Guid id)
        {
            return null;
        }

        public Task<Player> Get(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Player[]> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}