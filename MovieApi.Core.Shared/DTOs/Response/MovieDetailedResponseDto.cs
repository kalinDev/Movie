namespace MovieApi.Core.Shared.DTOs.Response;

public record MovieDetailedResponseDto : MovieResponseDto
{
    public string Summary { get; set; }
    
    public TimeSpan Duration { get; set; }
}