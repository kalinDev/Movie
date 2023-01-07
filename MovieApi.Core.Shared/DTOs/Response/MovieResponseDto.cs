using MovieApi.Domain.Enums;

namespace MovieApi.Core.Shared.DTOs.Response;

public record MovieResponseDto
{
    public int Id { get; init; }

    public string Title { get; set; }
    
    public string PosterUri { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public DateTime OffTheatersDate { get; set; }
    
    public Room Room { get; set; } 
}