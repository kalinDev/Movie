namespace MovieApi.Application.DTOs.Response;

public record MovieDetailedResponseDto : MovieResponseDto
{
    public string Summary { get; set; }
}