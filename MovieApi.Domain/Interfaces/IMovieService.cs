using MovieApi.Core.Shared.DTOs.Request;
using MovieApi.Core.Shared.DTOs.Response;

namespace MovieApi.Domain.Interfaces;

public interface IMovieService
{
    Task<List<MovieResponseDto>> FindAsync();
    Task<List<MovieResponseDto>> FindMovieInTheatersAsync();
    Task<MovieDetailedResponseDto> FindByIdAsync(int id);
    Task AddAsync(MovieRequestDto movieRequestDto);
    Task UpdateAsync(UpdateMovieRequestDto updateMovieRequestDto);
    Task<bool> AnyAsync(int id);
    Task DeleteByIdAsync(int id);
}