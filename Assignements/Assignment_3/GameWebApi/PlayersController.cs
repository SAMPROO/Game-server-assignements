using System.Threading.Tasks;
using System;

namespace dotnetKole
{
    
    public class PlayersController
    {
        private IRepository _repository;

        public PlayersController(IRepository i)
        {
            _repository = i;
        }

        public Task<Player> Get(Guid id)
        {
            return _repository.Get(id);
        }
        public Task<Player[]> GetAll()
        {
            return _repository.GetAll();
        }
        public Task<Player> Create(NewPlayer player)
        {
            return _repository.Create(player);
        }
        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _repository.Modify(id, player);
        }
        public Task<Player> Delete(Guid id)
        {
            return _repository.Delete(id);
        }
    }
}