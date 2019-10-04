
namespace dotnetKole
{
    using System.Collections.Generic;
    using System.Linq;
    public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players)
        {
            _players = players;
        }

        public T[] GetTop10Players()
        {
            
            var sortedPlayers = _players.OrderByDescending(x => x.Score);
            int topTen = (_players.Count<10) ? 10 : _players.Count();
            List<T> topPlayers = new List<T>();
            foreach(var player in sortedPlayers)
            {
                topPlayers.Add(player);
                if(topPlayers.Count>=10)
                {
                    continue;
                }
            }
        
            return topPlayers.ToArray();
        }
    }
}