namespace GameStore.Api.Dtos
{
    public record class GameDetailDto
    (
        int Id,
        string Name,
        string? Genre,
        decimal Price,
        DateOnly ReleaseDate
    );
}
