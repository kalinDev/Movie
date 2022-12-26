using AutoMapper;
using MovieApi.Application.DTOs;
using MovieApi.Application.DTOs.Request;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<MovieRequestDto, Movie>().ReverseMap();
    }
}
