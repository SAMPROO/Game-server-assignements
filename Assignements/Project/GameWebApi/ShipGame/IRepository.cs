using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ShipGame
{
    public interface IRepository
    {
        Task<Match> CreateMatch(Player player1, Player player2);
        Task<Match[]> GetAll();
        Task<Match> Get(Guid id);
        Task<Match> GetMatchStatus(Guid matchId);
        Task<Match> DeleteMatch(Guid matchId);
        Task<bool> GetPosition(Guid matchId, Guid playerId, Coordinate pos);
        Task<ActionReport> DestroyPiece(Guid matchId,Coordinate pos);
        Task<Ship[]> GetPlayerShips(Guid matchId, Guid playerId);
    }
}