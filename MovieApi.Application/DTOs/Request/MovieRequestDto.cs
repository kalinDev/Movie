using System.ComponentModel.DataAnnotations;

namespace MovieApi.Application.DTOs.Request;

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

}