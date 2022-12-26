using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.DTOs.Request;
using MovieApi.Application.Interfaces;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Controllers;

public class MoviesController : ApiController
{
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;
    
    public MoviesController(IMovieService movieService, IMapper mapper, INotifier notifier) : base (notifier)
    {
        _movieService = movieService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MovieRequestDto movieRequestDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        var movie = _mapper.Map<MovieRequestDto, Movie>(movieRequestDto);

        await _movieService.AddAsync(movie);
        
        return CustomResponse();
    }
}