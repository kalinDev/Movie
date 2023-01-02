namespace MovieApi.Application.DTOs.Response;

public record MovieResponseDto
{
    public int Id { get; init; }

    public string Title { get; set; }
    
    public string PosterUri { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
}