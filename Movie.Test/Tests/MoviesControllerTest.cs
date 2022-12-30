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
using Newtonsoft.Json;

namespace MovieApiTest.Tests;

public class MoviesControllerTest : IClassFixture<MovieFixture>
{
    private readonly Mock<ICachingService> _cachingService;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly MoviesController _controller;
    private readonly MovieFixture _movieFixture;
    
    public MoviesControllerTest(MovieFixture movieFixture)
    {
        _movieFixture = movieFixture;
        _cachingService = new Mock<ICachingService>();
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieServiceMock = new Mock<IMovieService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())));
        _controller = new MoviesController(_cachingService.Object, _movieRepositoryMock.Object, _movieServiceMock.Object, mapper, new Notifier());
    }
    
    #region AddMovieAsync

    [Fact(DisplayName = "AddMovieAsync Returns 200 Ok")]
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

    [Fact(DisplayName = "AddMovieAsync Returns 400 BadRequest")]
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

    [Fact(DisplayName = "GetMovieAsync Returns 200 Ok")]
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
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    #endregion
    
    #region GetMovieAsync

    [Fact(DisplayName = "GetOneMovieAsync Without Cache Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_WithoutCache_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        _cachingService.Setup( cache => cache.GetAsync("movie.Id")).ReturnsAsync((string)null);
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movie.Id)).ReturnsAsync(movie);
        
        //Act
        var result = await _controller.GetOneAsync(movie.Id);
        
        //Assert
        _cachingService.Verify(cache => cache.GetAsync($"movie_{movie.Id}"), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Once);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "GetOneMovieAsync With Cache Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_WithCache_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        _cachingService.Setup( cache => cache.GetAsync($"movie_{movie.Id}")).ReturnsAsync(JsonConvert.SerializeObject(movie));
        
        //Act
        var result = await _controller.GetOneAsync(movie.Id);
        
        //Assert
        _cachingService.Verify(cache => cache.GetAsync($"movie_{movie.Id}"), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Never);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "GetOneMovieAsync Returns 400 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_ReturnBadRequest()
    {
        //Arrange
        _cachingService.Setup( cache => cache.GetAsync("22")).ReturnsAsync((string)null);
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(22)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.GetOneAsync(22);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(22), Times.Once);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<NotFoundResult>();
        
    }

    #endregion
}