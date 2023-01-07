using AutoMapper;
using MovieApi.Application.Interfaces;
using MovieApi.Application.Validations;
using MovieApi.Core.Shared.DTOs.Request;
using MovieApi.Core.Shared.DTOs.Response;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Application.Services;

public class MovieService : BaseService, IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<bool> AnyAsync(int id) =>
        await _movieRepository.AnyAsync(id);
    
    public async Task<MovieDetailedResponseDto> FindByIdAsync(int id) =>
        _mapper.Map<MovieDetailedResponseDto>(await _movieRepository.FindByIdAsync(id));
    
    
    public async Task<List<MovieResponseDto>> FindAsync()
    {
        var movies = await _movieRepository.FindAsync();
        return _mapper.Map<List<Movie>, List<MovieResponseDto>>(movies);
    }
    
    public async Task<List<MovieResponseDto>> FindMovieInTheatersAsync()
    {
        var movies = await _movieRepository.SearchAsync(movie => movie.OffTheatersDate > DateTime.Now);
        return _mapper.Map<List<Movie>, List<MovieResponseDto>>(movies.ToList());
    }

    public async Task AddAsync(MovieRequestDto movieRequestDto)
    {
        var movie = _mapper.Map<MovieRequestDto, Movie>(movieRequestDto);
        if (!RunValidation(new MovieValidator(), movie)) return;

        _movieRepository.Add(movie);
        await _movieRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateMovieRequestDto updateMovieRequestDto)
    {
        var movie = _mapper.Map<MovieRequestDto, Movie>(updateMovieRequestDto);
        if (!RunValidation(new MovieValidator(), movie)) return;

        _movieRepository.Update(movie);
        await _movieRepository.SaveChangesAsync();
    }
    
    public async Task DeleteByIdAsync(int id)
    {
        _movieRepository.Remove(id);
        await _movieRepository.SaveChangesAsync();
    }
    
}