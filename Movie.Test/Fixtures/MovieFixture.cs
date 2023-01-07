using MovieApi.Core.Shared.DTOs.Request;
using MovieApi.Core.Shared.DTOs.Response;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Enums;

namespace MovieApiTest.Fixtures;

public class MovieFixture : IDisposable
{
    public MovieRequestDto ValidMovieRequestDto()
    {
        return new MovieRequestDto
        {
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.",
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(+10),
            Room = Room.Imax,
            Duration = TimeSpan.FromMinutes(30)
        };
    }

    public UpdateMovieRequestDto ValidUpdateMovieRequestDto()
    {
        return new UpdateMovieRequestDto
        {
            Id = 22,
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.",
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(+10),
            Room = Room.Imax,
            Duration = TimeSpan.FromMinutes(30)
        };
    }

    public MovieDetailedResponseDto ValidMovieDetailedResponseDto()
    {
        return new MovieDetailedResponseDto
        {
            Id = 22,
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.",
            PosterUri = "https://m.media-amazon.com/images/M/MV5BZDQyODUwM2MtNzA0YS00ZjdmLTgzMjItZWRjN2YyYWE5ZTNjXkEyXkFqcGdeQXVyMTI2MzY1MjM1._V1_.jpg",
            OffTheatersDate = DateTime.Now.AddDays(+10),
            Room = Room.Imax,
            Duration = TimeSpan.FromMinutes(30),
        };

    }
    
    public MovieRequestDto InvalidMovieRequestDto()
    {
        var invalidMovieRequestDto = ValidMovieRequestDto();
        invalidMovieRequestDto.Title = null;
        invalidMovieRequestDto.Summary = null;

        return invalidMovieRequestDto;
    }

    public Movie ValidMovie()
    {
        return new Movie()
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
    
    public UpdateMovieRequestDto InvalidUpdateMovieRequestDto()
    {
        return new UpdateMovieRequestDto()
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
    
    public List<Movie> ValidMovies()
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