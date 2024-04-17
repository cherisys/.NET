using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GenreEndpoints
    {
        public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
        {
            var routeGroup = app.MapGroup("genres").WithParameterValidation();

            routeGroup.MapGet("/", async (GameDbContext dbContext) =>
            {
                return await dbContext.Genres.Select(g => g.ToDto())
                                            .AsNoTracking().ToListAsync();
            });

            return routeGroup;
        }

    }
}
