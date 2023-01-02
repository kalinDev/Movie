namespace MovieApi.Application.DTOs.Response;

public record MovieDetailedResponseDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Summary { get; set; }
    
    public string Poster { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public DateTime OffTheatersDate { get; set; }
}