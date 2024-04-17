using GameStore.Web.Models;
using System.Net.Http.Json;

namespace GameStore.Web.Services
{
    public class GenreService : IGenreService
    {
        private readonly HttpClient httpClient;

        public GenreService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Genre[]> GetGenres()
        {
            return await httpClient.GetFromJsonAsync<Genre[]>("/genres") ?? [];
        }
    }
}
