using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Data.Repositories;


public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(ApiDbContext context) : base(context)
    {

    }
}