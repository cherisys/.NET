using GameStore.Web.Models;
using System.Net.Http.Json;

namespace GameStore.Web.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient httpClient;

        public GameService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateGame(EditGameModel game)
        {
            await httpClient.PostAsJsonAsync("/games", game);
        }

        public async Task DeleteGame(int id)
        {
            await httpClient.DeleteAsync($"/games/{id}");
        }

        public async Task<EditGameModel> GetGame(int id)
        {
            var game = await httpClient.GetFromJsonAsync<EditGameModel>($"/games/{id}");
            return game is not null ? game : new EditGameModel { Id = 0, Name = "", GenreId = -1, Price = 0.00M, ReleaseDate = DateOnly.FromDateTime(DateTime.UtcNow) };
        }

        public async Task<Game[]> GetGames()
        {
            return await httpClient.GetFromJsonAsync<Game[]>("/games") ?? [];
        }

        public async Task UpdateGame(EditGameModel game)
        {
            await httpClient.PutAsJsonAsync($"/games/{game.Id}",game);
        }
    }
}
