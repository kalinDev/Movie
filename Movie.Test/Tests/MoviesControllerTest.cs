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
    private readonly Mock<ICachingService> _cachingServiceMock;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly MoviesController _controller;
    private readonly MovieFixture _movieFixture;
    
    public MoviesControllerTest(MovieFixture movieFixture)
    {
        _movieFixture = movieFixture;
        _cachingServiceMock = new Mock<ICachingService>();
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieServiceMock = new Mock<IMovieService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())));
        _controller = new MoviesController(_cachingServiceMock.Object, _movieRepositoryMock.Object, _movieServiceMock.Object, mapper, new Notifier());
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

        _movieServiceMock.Verify(service => service.AddAsync(It.IsAny<Movie>()), Times.Once);
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
        _movieServiceMock.Verify(service => service.AddAsync(It.IsAny<Movie>()), Times.Never);
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
        _movieRepositoryMock.Verify(repository => repository.FindAsync(), Times.Once);
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
        _cachingServiceMock.Setup( cache => cache.GetAsync("movie.Id")).ReturnsAsync((string)null);
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movie.Id)).ReturnsAsync(movie);
        
        //Act
        var result = await _controller.GetOneAsync(movie.Id);
        
        //Assert
        _cachingServiceMock.Verify(cache => cache.GetAsync($"movie_{movie.Id}"), Times.Once);
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
        _cachingServiceMock.Setup( cache => cache.GetAsync($"movie_{movie.Id}")).ReturnsAsync(JsonConvert.SerializeObject(movie));
        
        //Act
        var result = await _controller.GetOneAsync(movie.Id);
        
        //Assert
        _cachingServiceMock.Verify(cache => cache.GetAsync($"movie_{movie.Id}"), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Never);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "GetOneMovieAsync Returns 400 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_ReturnBadRequest()
    {
        //Arrange
        const int movieId = 22;
        _cachingServiceMock.Setup( cache => cache.GetAsync(movieId.ToString())).ReturnsAsync((string)null);
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.GetOneAsync(movieId);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movieId), Times.Once);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<NotFoundResult>();
        
    }

    #endregion

    #region UpdateMovieAsync

    [Fact(DisplayName = "UpdateMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void UpdateMovieAsync_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        var updateMovieRequestDto = _movieFixture.CreateValidUpdateMovieRequestDto();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movie.Id)).ReturnsAsync(movie);
        
        //Act
        var result = await _controller.PutAsync(movie.Id, updateMovieRequestDto);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Once);
        _movieServiceMock.Verify(service => service.UpdateAsync(It.IsAny<Movie>()), Times.Once);
        _cachingServiceMock.Verify(cache => cache.SetAsync($"movie_{movie.Id}", JsonConvert.SerializeObject(movie)), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "UpdateMovieAsync with divergent ids Returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void UpdateMovieAsync_WitDivergentIds_ReturnBadRequest()
    {
        //Arrange
        const int movieId = 22;
        var updateMovieRequestDto = _movieFixture.CreateValidUpdateMovieRequestDto();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.PutAsync(33, updateMovieRequestDto);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movieId), Times.Never);
        _movieServiceMock.Verify(service => service.UpdateAsync(It.IsAny<Movie>()), Times.Never);
        _cachingServiceMock.Verify(cache => cache.SetAsync($"movie_{movieId}", It.IsAny<string>()), Times.Never);
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact(DisplayName = "UpdateMovieAsync Returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void UpdateMovieAsync_ReturnBadRequest()
    {
        //Arrange
        const int movieId = 22;
        var updateMovieRequestDto = _movieFixture.CreateValidUpdateMovieRequestDto();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        _controller.ModelState.AddModelError("fakeError", "fakeError");
        
        //Act
        var result = await _controller.PutAsync(movieId, updateMovieRequestDto);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movieId), Times.Never);
        _movieServiceMock.Verify(service => service.UpdateAsync(It.IsAny<Movie>()), Times.Never);
        _cachingServiceMock.Verify(cache => cache.SetAsync($"movie_{movieId}", It.IsAny<string>()), Times.Never);
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact(DisplayName = "UpdateMovieAsync Returns 404 NotFound")]
    [Trait("Controller", "Movies")]
    public async void UpdateMovieAsync_ReturnNotFound()
    {
        //Arrange
        const int movieId = 22;
        var updateMovieRequestDto = _movieFixture.CreateValidUpdateMovieRequestDto();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.PutAsync(movieId, updateMovieRequestDto);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movieId), Times.Once);
        _movieServiceMock.Verify(service => service.UpdateAsync(It.IsAny<Movie>()), Times.Never);
        _cachingServiceMock.Verify(cache => cache.SetAsync($"movie_{movieId}", It.IsAny<string>()), Times.Never);
        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion
    
    
    #region DeleteMovieAsync
    
    [Fact(DisplayName = "DeleteOneMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void DeleteOneMovieAsync_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movie.Id)).ReturnsAsync(movie);
        
        //Act
        var result = await _controller.DeleteAsync(movie.Id);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Once);
        _movieServiceMock.Verify(service => service.DeleteByIdAsync(movie.Id), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "DeleteOneMovieAsync Returns 404 NotFound")]
    [Trait("Controller", "Movies")]
    public async void DeleteOneMovieAsync_ReturnNotFound()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        _movieRepositoryMock.Setup(repository => repository.FindByIdAsync(movie.Id)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.DeleteAsync(movie.Id);
        
        //Assert
        _movieRepositoryMock.Verify(repository => repository.FindByIdAsync(movie.Id), Times.Once);
        _movieServiceMock.Verify(service => service.DeleteByIdAsync(movie.Id), Times.Never);
        result.Should().BeOfType<NotFoundResult>();
    }
    #endregion
}