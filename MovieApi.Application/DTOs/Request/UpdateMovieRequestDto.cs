using System.ComponentModel.DataAnnotations;

namespace MovieApi.Application.DTOs.Request;

public record UpdateMovieRequestDto : MovieRequestDto
{
    [Required]
    public int Id { get; init; }
}