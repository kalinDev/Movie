using AutoMapper;
using MovieApi.Application.DTOs;
using MovieApi.Application.DTOs.Request;
using MovieApi.Application.DTOs.Response;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<MovieRequestDto, Movie>();
        CreateMap<UpdateMovieRequestDto, Movie>();
        CreateMap<Movie, MovieResponseDto>();
        CreateMap<Movie, MovieDetailedResponseDto>();
    }
}
