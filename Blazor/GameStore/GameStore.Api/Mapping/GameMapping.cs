using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping
{
    public static class GameMapping
    {
        public static Game ToEntity(this CreateGameDto game)
        {
            return new Game {Name = game.Name, GenreId = game.GenreId, Price = game.Price, ReleaseDate = game.ReleaseDate};
        }
        public static GameDto ToDto(this Game game)
        {
            return new GameDto(game.Id,game.Name,game.GenreId,game.Price,game.ReleaseDate);
        }

        public static GameDetailDto ToDetailDto(this Game game)
        {
            return new GameDetailDto(game.Id, game.Name, game.Genre?.Name, game.Price, game.ReleaseDate);
        }
        public static Game ToEntity(this UpdateGameDto game, int Id)
        {
            return new Game { Id = Id, Name = game.Name, GenreId = game.GenreId, Price = game.Price, ReleaseDate = game.ReleaseDate };
        }
    }
}
