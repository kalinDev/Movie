namespace MovieApi.Application.DTOs.Response;

public record MovieResponseDto
{
    public int Id { get; init; }

    public string Title { get; set; }
    
    public bool InTheaters { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
}