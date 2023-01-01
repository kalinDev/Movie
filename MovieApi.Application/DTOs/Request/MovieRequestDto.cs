using System.ComponentModel.DataAnnotations;

namespace MovieApi.Application.DTOs.Request;

public record MovieRequestDto
{
    [Required]
    [StringLength(500, MinimumLength = 2)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Summary { get; set; }
    
    [Required]
    public bool InTheaters { get; set; }
    
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    [Required]
    public DateTime OffTheatersDate { get; set; }

}