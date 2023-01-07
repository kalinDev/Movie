using AutoMapper;
using MovieApi.Core.Shared.DTOs.Request;
using MovieApi.Core.Shared.DTOs.Response;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<MovieRequestDto, Movie>();
        CreateMap<UpdateMovieRequestDto, Movie>();
        CreateMap<Movie, MovieResponseDto>();
        CreateMap<Movie, MovieDetailedResponseDto>().ReverseMap();
    }
}
