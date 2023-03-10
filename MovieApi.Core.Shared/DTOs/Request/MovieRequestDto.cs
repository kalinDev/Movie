using System.ComponentModel.DataAnnotations;
using MovieApi.Domain.Enums;

namespace MovieApi.Core.Shared.DTOs.Request;

public record MovieRequestDto
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Summary { get; set; }
    
    [Required]
    public string PosterUri { get; set; }
    
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    [Required]
    public DateTime OffTheatersDate { get; set; }
    
    [Required]
    public Room Room { get; set; }
    
    [Required]
    public TimeSpan Duration { get; set; }

}