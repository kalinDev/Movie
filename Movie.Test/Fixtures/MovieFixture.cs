using MovieApi.Application.DTOs.Request;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Enums;

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
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(-10),
            Room = Room.Imax,
        };
    }

    public UpdateMovieRequestDto CreateValidUpdateMovieRequestDto()
    {
        return new UpdateMovieRequestDto
        {
            Id = 22,
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.",
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(+10),
            Room = Room.Imax
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
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(+10),
            Room = Room.Imax,
            Duration = TimeSpan.FromMinutes(30)
        };
    }
    
    public Movie CreateInvalidMovie()
    {
        return new Movie
        {
            Id = 22,
            Title = "",
            ReleaseDate = DateTime.Now,
            Summary = "movie",
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(-10),
            Room = Room.Imax
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
                PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
                OffTheatersDate = DateTime.Now.AddDays(+10),
                Room = Room.Imax,
                Duration = TimeSpan.FromMinutes(30)
            });
        }

        return movies;
    }

    public void Dispose()
    {
    }
}