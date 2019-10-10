using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ShipGame
{
    public interface IRepository
    {
        Task<Match> Get(Guid id);
        Task<Match[]> GetAll();
        Task<Match> CreateMatch(string player1, string player2);
        Task<Match> CreateMatch(Guid playerOneId, Guid playerTwoId);
        Task<Match[]> GetLiveMatches();
        Task<Ship[]> GetPlayerShips(Guid matchId, Guid playerId);
        Task<bool> GetPosition(Guid matchId, Guid playerId, Coordinate pos);
        Task<bool> CheckIfInProgress(Guid matchId);
        Task<Ship[]> AddShip(Guid matchId, Guid playerId,Coordinate pos1, Coordinate pos2);
        Task<Ship[]> DestroyPart(Guid matchId,Guid playerId,Coordinate pos);
        Task<Match> DeleteMatch(Guid matchId);
        Task<bool> DeleteAll();

        /////////////////////////////////////////////////

        Task<Player> GetPlayer(Guid id);
        Task<Player[]> GetPlayer(string name);
        Task<Player[]> GetAllPlayers();
        Task<Player[]> GetTopPlayer(int top = 1);
        Task<Player> CreatePlayer(string name);
        Task<bool> CheckIfInMatch(Guid playerId);
        Task<Player> DeletePlayer(Guid matchId);
        Task<bool> DeleteAllPlayers();



    }
}