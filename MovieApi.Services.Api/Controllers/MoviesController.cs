using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.DTOs.Request;
using MovieApi.Application.DTOs.Response;
using MovieApi.Application.Interfaces;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;
using Newtonsoft.Json;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
public class MoviesController : ApiController
{
    private readonly ICachingService _cachingService;
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;
    
    public MoviesController(
        ICachingService cachingService,
        IMovieRepository movieRepository,
        IMovieService movieService,
        IMapper mapper,
        INotifier notifier) : base (notifier)
    {
        _cachingService = cachingService;
        _movieRepository = movieRepository;
        _movieService = movieService;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetailedResponseDto>> GetOneAsync(int id)
    {
        var movieCache = await _cachingService.GetAsync($"movie_{id}");

        if (movieCache is not null)
        {
            return CustomResponse(JsonConvert.DeserializeObject<MovieDetailedResponseDto>(movieCache));
        }

        var movie = await _movieRepository.FindByIdAsync(id);
        if (movie is null) return NotFound();

        await _cachingService.SetAsync($"movie_{movie.Id}", JsonConvert.SerializeObject(movie));

        return CustomResponse(_mapper.Map<MovieDetailedResponseDto>(movie));
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieResponseDto>>> GetMoviesInTheatersAsync()
    {
        
        var movies = await _movieRepository.SearchAsync(movie => movie.OffTheatersDate > DateTime.Now);
        
        var moviesDto = _mapper.Map<List<Movie>, List<MovieResponseDto>>(movies.ToList());
        
        return CustomResponse(moviesDto);
    }

    [HttpGet("All")]
    public async Task<ActionResult<List<MovieResponseDto>>> GetAsync()
    {
        
        var movies = await _movieRepository.FindAsync();
        
        var moviesDto = _mapper.Map<List<Movie>, List<MovieResponseDto>>(movies);
        
        return CustomResponse(moviesDto);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MovieRequestDto movieRequestDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        var movie = _mapper.Map<MovieRequestDto, Movie>(movieRequestDto);

        await _movieService.AddAsync(movie);
        
        return CustomResponse();
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateMovieRequestDto updateMovieRequestDto)
    {
        if (id != updateMovieRequestDto.Id)
        {
            AddError("The id informed is different from the body.");
            return CustomResponse();
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);
        
        var movie = await _movieRepository.FindByIdAsync(id);
        if (movie is null) return NotFound();
        
        movie = _mapper.Map<MovieRequestDto, Movie>(updateMovieRequestDto, movie);

        await _movieService.UpdateAsync(movie);
        
        if (IsOperationValid()) await _cachingService.SetAsync($"movie_{movie.Id}", JsonConvert.SerializeObject(movie));
        
        return CustomResponse();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var movie = await _movieRepository.FindByIdAsync(id);
        if (movie is null) return NotFound();

        await _movieService.DeleteByIdAsync(movie.Id);
        
        return CustomResponse();
    }
}