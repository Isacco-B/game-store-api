using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    [Required] [StringLength(50)] string Name,
    int GenreId,
    [Required] [Range(1, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);
