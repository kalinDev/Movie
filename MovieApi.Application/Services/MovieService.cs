using MovieApi.Application.Interfaces;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Application.Services;

public class MovieService : BaseService, IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository, INotifier notifier) : base(notifier)
        => _movieRepository = movieRepository;
    
    public async Task AddAsync(Movie movie)
    {
        _movieRepository.Add(movie);
        await _movieRepository.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        _movieRepository.Remove(id);
        await _movieRepository.SaveChangesAsync();
    }
}