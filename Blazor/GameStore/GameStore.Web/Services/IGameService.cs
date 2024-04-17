using GameStore.Web.Models;

namespace GameStore.Web.Services
{
    public interface IGameService
    {
        Task<Game[]> GetGames();
        Task<EditGameModel> GetGame(int id);
        Task CreateGame(EditGameModel game);
        Task UpdateGame(EditGameModel game);
        Task DeleteGame(int id);
    }
}
