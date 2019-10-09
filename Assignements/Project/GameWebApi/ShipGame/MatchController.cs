namespace ShipGame
{
    public class MatchController
    {
        private IRepository _repository;

        public MatchController(IRepository i)
        {
            _repository = i;
        }
    }
}