namespace ShipGame
{
    public interface IRepository
    {
        Task<Match> CreateMatch(Player player1, Player player2);
        Task<Match[]> GetAll();
        Task<Match> GetMatchStatus(Guid matchId);
        Task<Match> DeleteMatch(Guid matchId);
        Task<Coordinate> GetPosition(Guid matchId, Guid playerId, Coordinate pos);
        Task<Coordinate> DestroyPiece(Guid matchId,Coordinate pos);
        Task<Ship[]> GetPlayersShips(Guid matchId, Guid playerId);
    }
}