using System.ComponentModel.DataAnnotations;

namespace MovieApi.Core.Shared.DTOs.Request;

public record UpdateMovieRequestDto : MovieRequestDto
{
    [Required]
    public int Id { get; init; }
}