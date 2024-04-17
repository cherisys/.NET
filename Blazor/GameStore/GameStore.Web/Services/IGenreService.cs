using GameStore.Web.Models;

namespace GameStore.Web.Services
{
    public interface IGenreService
    {
        Task<Genre[]> GetGenres();
    }
}
