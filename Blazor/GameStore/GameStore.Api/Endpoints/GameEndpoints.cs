using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GameEndpoints
    {
        public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
        {
            var routeGroup = app.MapGroup("/games").WithParameterValidation();
            const string GetGameEndpointName = "GetGame";

            routeGroup.MapGet("/", async (GameDbContext dbContext) =>
            {
                return await dbContext.Games.Include(g => g.Genre)
                                            .Select(g => g.ToDetailDto())
                                            .AsNoTracking().ToListAsync();
            });

            routeGroup.MapGet("/{id}", async (int id, GameDbContext dbContext) => 
            {
                Game? game = await dbContext.Games.FindAsync(id);
                return game is null ? Results.NotFound() : Results.Ok(game.ToDto());

            }).WithName(GetGameEndpointName);

            routeGroup.MapPost("/", async (CreateGameDto gameToCreate, GameDbContext dbContext) =>
            {
                Game game = gameToCreate.ToEntity();
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
            });

            routeGroup.MapPut("/{id}", async (UpdateGameDto game, int id, GameDbContext dbContext) => 
            {
                Game? existingGame = await dbContext.Games.FindAsync(id);
                
                if(existingGame is null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame).CurrentValues.SetValues(game.ToEntity(id));
                await dbContext.SaveChangesAsync();
                
                return Results.NoContent();
            });

            routeGroup.MapDelete("/{id}", async (int id, GameDbContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });

            return routeGroup;
        }

    }
}
