using System.ComponentModel.DataAnnotations;

namespace MovieApi.Application.DTOs.Request;

public record MovieRequestDto
{
    [Required]
    [MaxLength(100), MinLength(2)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Summary { get; set; }
    
    [Required]
    public bool InTheaters { get; set; }
    
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    [Required]
    public DateTime OffTheatersDate { get; set; }

    //image of poster
}