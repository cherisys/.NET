namespace GameStore.Api.Dtos
{
    public record class GameDto
    (
        int Id,
        string Name,
        int GenreId,
        decimal Price,
        DateOnly ReleaseDate
    );
}
