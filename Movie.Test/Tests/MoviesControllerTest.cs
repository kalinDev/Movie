using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApi.Application.AutoMapper;
using MovieApi.Application.DTOs.Response;
using MovieApi.Application.Notifications;
using MovieApi.Controllers;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;
using MovieApiTest.Fixtures;

namespace MovieApiTest.Tests;

public class MoviesControllerTest : IClassFixture<MovieFixture>
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly MoviesController _controller;
    private readonly MovieFixture _movieFixture;
    
    public MoviesControllerTest(MovieFixture movieFixture)
    {
        _movieFixture = movieFixture;
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieServiceMock = new Mock<IMovieService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())));
        _controller = new MoviesController(_movieRepositoryMock.Object, _movieServiceMock.Object, mapper, new Notifier());
    }
    
    #region AddMovieAsync

    [Fact(DisplayName = "AddMovieAsync returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void AddMovieAsync_ReturnOk()
    {
        //Arrange
        var movieRequestDto = _movieFixture.CreateValidMovieRequestDto();

        //Act
        var result = await _controller.PostAsync(movieRequestDto);

        //Assert

        _movieServiceMock.Verify(x => x.AddAsync(It.IsAny<Movie>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }

    
    
    [Fact(DisplayName = "AddMovieAsync returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void AddMovieAsync_ReturnBadRequest()
    {
        //Arrange
        
        var movieRequestDto = _movieFixture.CreateInvalidMovieRequestDto();
        _controller.ModelState.AddModelError("fakeError", "fakeError");

        //Act
        var result = await _controller.PostAsync(movieRequestDto);

        //Assert
        _movieServiceMock.Verify(x => x.AddAsync(It.IsAny<Movie>()), Times.Never);
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    #endregion
    
    #region GetMovieAsync

    [Fact(DisplayName = "GetMovieAsync returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetMovieAsync_ReturnOk()
    {
        //Arrange
        var movies = _movieFixture.CreateValidMovies();
        _movieRepositoryMock.Setup(repository => repository.FindAsync()).ReturnsAsync(movies);
        
        //Act
        var result = await _controller.GetAsync();
        
        //Assert
        _movieRepositoryMock.Verify(x => x.FindAsync(), Times.Once);
        result.Should().BeOfType<ActionResult<List<MovieResponseDto>>>();
        
    }

    #endregion
}