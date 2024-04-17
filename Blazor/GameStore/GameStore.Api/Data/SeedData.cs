using GameStore.Api.Entities;

namespace GameStore.Api.Data
{
    public static class SeedData
    {
        public static readonly List<Genre> Genres = new List<Genre>
        {
            new Genre { Id = 1, Name = "Fighting" },
            new Genre { Id = 2, Name = "Roleplaying" },
            new Genre { Id = 3, Name = "Sports" },
            new Genre { Id = 4, Name = "Racing" },
            new Genre { Id = 5, Name = "Kids & Family" }
        };

        public static readonly List<Game> Games = new List<Game>
        {

        };
    }
}
