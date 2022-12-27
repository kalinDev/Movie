﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.DTOs.Request;
using MovieApi.Application.DTOs.Response;
using MovieApi.Application.Interfaces;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
public class MoviesController : ApiController
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;
    
    public MoviesController(IMovieRepository movieRepository ,IMovieService movieService, IMapper mapper, INotifier notifier) : base (notifier)
    {
        _movieRepository = movieRepository;
        _movieService = movieService;
        _mapper = mapper;
    }

    [HttpGet("/{id:int}")]
    public async Task<ActionResult<MovieDetailedResponseDto>> GetOneAsync(int id)
    {
        var movie = await _movieRepository.FindByIdAsync(id);
        if (movie is null) return NotFound();

        return CustomResponse(_mapper.Map<MovieDetailedResponseDto>(movie));
    }

    [HttpGet]
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
}