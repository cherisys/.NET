using GameStore.Web.Models;

namespace GameStore.Web
{
    public static class GameClient
    {

       private static readonly List<Game> Games = new()
       {
            new Game {Id = 1, Name = "Road Fighter", Genre = "Classic", Price = 34.4M, ReleaseDate = new DateTime(1999,12,04) },
            new Game {Id = 2, Name = "Load Runner", Genre = "Classic", Price = 14.4M, ReleaseDate = new DateTime(2001,12,04) },
            new Game {Id = 3, Name = "Angry Birds", Genre = "Modern", Price = 78.4M, ReleaseDate = new DateTime(2022,12,04) }
        };

        public static Game[] GetGames()
        {
            return Games.ToArray();
        }

        public static void AddGame(Game game)
        {
            game.Id = Games.Max(x => x.Id) + 1;
            Games.Add(game);
        }

        public static Game GetGame(int id) 
        { 
            return Games.Find(x => x.Id == id) ?? throw new Exception("Game not found!");
        }

        public static void UpdateGame(Game updatedGame)
        {
            Game existingGame = GetGame(updatedGame.Id);
            existingGame.Name = updatedGame.Name;
            existingGame.Genre = updatedGame.Genre;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;
        }

        public static void DeleteGame(int id)
        {
            Game game = GetGame(id);
            Games.Remove(game);
        }
    }
}
