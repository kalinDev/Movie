using MovieApi.Application.DTOs.Request;
using MovieApi.Domain.Entities;

namespace MovieApiTest.Fixtures;

public class MovieFixture : IDisposable
{
    public MovieRequestDto CreateValidMovieRequestDto()
    {
        return new MovieRequestDto
        {
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.",
            InTheaters = false,
            OffTheatersDate = DateTime.Now.AddDays(-10)
        };
    }

    public MovieRequestDto CreateInvalidMovieRequestDto()
    {
        var invalidMovieRequestDto = CreateValidMovieRequestDto();
        invalidMovieRequestDto.Title = null;
        invalidMovieRequestDto.Summary = null;

        return invalidMovieRequestDto;
    }

    public Movie CreateValidMovie()
    {
        return new Movie
        {
            Id = 22,
            Title = "Movie 22",
            ReleaseDate = DateTime.Now,
            Summary = "Summary of movie",
            InTheaters = false,
            OffTheatersDate = DateTime.Now.AddDays(-10)
        };
    }
    
    public List<Movie> CreateValidMovies()
    {
        var movies = new List<Movie>();

        for (int i = 0; i < 10; i++)
        {
            movies.Add(new Movie
            {
                Id = i,
                Title = $"Movie {i}",
                ReleaseDate = DateTime.Now,
                Summary = $"Summary {i}",
                InTheaters = false,
                OffTheatersDate = DateTime.Now.AddDays(-10)
            });
        }

        return movies;
    }

    public void Dispose()
    {
    }
}