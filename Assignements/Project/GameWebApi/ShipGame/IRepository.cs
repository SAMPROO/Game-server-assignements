using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ShipGame
{
    public interface IRepository
    {
        Task<Match> CreateMatch(NewPlayer player1, NewPlayer player2);
        Task<Match[]> GetAll();
        Task<Match> Get(Guid id);
        Task<Match[]> GetLiveMatches();
        Task<bool> CheckIfInProgress(Guid matchId);
        Task<Ship[]> AddShip(Guid matchId, Guid playerId,Coordinate pos1, Coordinate pos2);
        Task<Match> DeleteMatch(Guid matchId);
        Task<bool> GetPosition(Guid matchId, Guid playerId, Coordinate pos);
        Task<Ship[]> DestroyPart(Guid matchId,Guid playerId,Coordinate pos);
        Task<Ship[]> GetPlayerShips(Guid matchId, Guid playerId);
        Task<bool> DeleteAll();

    }
}