using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Interfaces;
using MovieApi.Core.Shared.DTOs.Request;
using MovieApi.Core.Shared.DTOs.Response;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;
using Newtonsoft.Json;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
public class MoviesController : ApiController
{
    private readonly ICachingService _cachingService;
    private readonly IMovieService _movieService;
    
    public MoviesController(
        ICachingService cachingService,
        IMovieService movieService,
        INotifier notifier) : base (notifier)
    {
        _cachingService = cachingService;
        _movieService = movieService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetailedResponseDto>> GetOneAsync(int id)
    {
        var movieCache = await _cachingService.GetAsync($"movie_{id}");

        if (movieCache is not null)
        {
            return CustomResponse(JsonConvert.DeserializeObject<MovieDetailedResponseDto>(movieCache));
        }

        var movieDto = await _movieService.FindByIdAsync(id);
        if (movieDto is null) return NotFound();

        await _cachingService.SetAsync($"movie_{movieDto.Id}", JsonConvert.SerializeObject(movieDto));

        return CustomResponse(movieDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieResponseDto>>> GetMoviesInTheatersAsync()
    {
        var moviesDto = await _movieService.FindMovieInTheatersAsync();
        return CustomResponse(moviesDto);
    }

    [HttpGet("All")]
    public async Task<ActionResult<List<MovieResponseDto>>> GetAsync()
    {
        
        var moviesDto = await _movieService.FindAsync();
        
        return CustomResponse(moviesDto);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MovieRequestDto movieRequestDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _movieService.AddAsync(movieRequestDto);
        
        return CustomResponse();
    }
    /*
    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] UpdateMovieRequestDto updateMovieRequestDto)
    {
        if (id != updateMovieRequestDto.Id)
        {
            AddError("The id informed is different from the body.");
            return CustomResponse();
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);
        
        var movieDto = await _movieService.FindByIdAsync(id);
        if (movieDto is null) return NotFound();
        
        movie = _mapper.Map<MovieRequestDto, Movie>(updateMovieRequestDto, movie);

        await _movieService.UpdateAsync(updateMovieRequestDto);
        
        if (IsOperationValid()) await _cachingService.SetAsync($"movie_{movie.Id}", JsonConvert.SerializeObject(movie));
        
        return CustomResponse();
    }
    */
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var exist = await _movieService.AnyAsync(id);
        if (!exist) return NotFound();

        await _movieService.DeleteByIdAsync(id);
        
        return CustomResponse();
    }
}