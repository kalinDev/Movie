using MovieApi.Domain.Entities;

namespace MovieApi.Domain.Interfaces;

public interface IMovieService 
{
    Task AddAsync(Movie movie);
}