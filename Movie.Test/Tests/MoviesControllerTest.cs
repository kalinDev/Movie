using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApi.Application.Notifications;
using MovieApi.Controllers;
using MovieApi.Core.Shared.DTOs.Response;
using MovieApi.Domain.Interfaces;
using MovieApiTest.Fixtures;
using Newtonsoft.Json;

namespace MovieApiTest.Tests;

public class MoviesControllerTest : IClassFixture<MovieFixture>
{
    private readonly Mock<ICachingService> _cachingServiceMock;
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly MoviesController _controller;
    private readonly MovieFixture _movieFixture;
    
    public MoviesControllerTest(MovieFixture movieFixture)
    {
        _movieFixture = movieFixture;
        _cachingServiceMock = new Mock<ICachingService>();
        _movieServiceMock = new Mock<IMovieService>();
        _controller = new MoviesController(_cachingServiceMock.Object, _movieServiceMock.Object, new Notifier());
    }
    
    #region AddMovieAsync

    [Fact(DisplayName = "AddMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void AddMovieAsync_ReturnOk()
    {
        //Arrange
        var movieRequestDto = _movieFixture.ValidMovieRequestDto();

        //Act
        var result = await _controller.PostAsync(movieRequestDto);

        //Assert

        _movieServiceMock.Verify(service => service.AddAsync(movieRequestDto), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact(DisplayName = "AddMovieAsync Returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void AddMovieAsync_ReturnBadRequest()
    {
        //Arrange
        
        var movieRequestDto = _movieFixture.InvalidMovieRequestDto();
        _controller.ModelState.AddModelError("fakeError", "fakeError");

        //Act
        var result = await _controller.PostAsync(movieRequestDto);

        //Assert
        _movieServiceMock.Verify(service => service.AddAsync(movieRequestDto), Times.Never);
        result.Should().BeOfType<BadRequestObjectResult>();
        
    }
    
    #endregion
    
    #region GetMovieAsync

    [Fact(DisplayName = "GetMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetMovieAsync_ReturnOk()
    {
        //Arrange
        _movieServiceMock
            .Setup(service => service.FindAsync())
            .ReturnsAsync(It.IsAny<List<MovieResponseDto>>());
        
        //Act
        var result = await _controller.GetAsync();
        
        //Assert
        _movieServiceMock.Verify(service => service.FindAsync(), Times.Once);
        result.Should().BeOfType<ActionResult<List<MovieResponseDto>>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    #endregion

    #region GetMoviesInTheatersAsync
    [Fact(DisplayName = "GetMoviesInTheatersAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetMoviesInTheatersAsync_ReturnOk()
    {
        //Arrange
        _movieServiceMock
            .Setup(service => service.FindMovieInTheatersAsync())
            .ReturnsAsync(It.IsAny<List<MovieResponseDto>>());
        
        //Act
        var result = await _controller.GetMoviesInTheatersAsync();
        
        //Assert
        _movieServiceMock.Verify(service => service.FindMovieInTheatersAsync(), Times.Once);
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
        var movieDetailedResponseDto =  _movieFixture.ValidMovieDetailedResponseDto();
        _cachingServiceMock.Setup( cache => cache.GetAsync("movie.Id")).ReturnsAsync((string)null);
        
        _movieServiceMock
            .Setup(service => service.FindByIdAsync(movieDetailedResponseDto.Id))
            .ReturnsAsync(movieDetailedResponseDto);
        
        //Act
        var result = await _controller.GetOneAsync(movieDetailedResponseDto.Id);
        
        //Assert
        _cachingServiceMock.Verify(cache => cache.GetAsync($"movie_{movieDetailedResponseDto.Id}"), Times.Once);
        _movieServiceMock.Verify(service => service.FindByIdAsync(movieDetailedResponseDto.Id), Times.Once);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "GetOneMovieAsync With Cache Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_WithCache_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        
        _cachingServiceMock
            .Setup(cache => cache.GetAsync($"movie_{movie.Id}"))
            .ReturnsAsync(JsonConvert.SerializeObject(movie));
        
        //Act
        var result = await _controller.GetOneAsync(movie.Id);
        
        //Assert
        _cachingServiceMock.Verify(cache => cache.GetAsync($"movie_{movie.Id}"), Times.Once);
        _movieServiceMock.Verify(service => service.FindByIdAsync(movie.Id), Times.Never);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "GetOneMovieAsync Returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void GetOneMovieAsync_ReturnBadRequest()
    {
        //Arrange
        const int movieId = 22;
        _cachingServiceMock.Setup( cache => cache.GetAsync(movieId.ToString())).ReturnsAsync((string)null);
        _movieServiceMock.Setup(service => service.FindByIdAsync(movieId)).ReturnsAsync((MovieDetailedResponseDto)null);
        
        //Act
        var result = await _controller.GetOneAsync(movieId);
        
        //Assert
        _movieServiceMock.Verify(service => service.FindByIdAsync(movieId), Times.Once);
        result.Should().BeOfType<ActionResult<MovieDetailedResponseDto>>();
        result.Result.Should().BeOfType<NotFoundResult>();
        
    }

    #endregion
    /*
    #region UpdateMovieAsync

    [Fact(DisplayName = "UpdateMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void UpdateMovieAsync_ReturnOk()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        var updateMovieRequestDto = _movieFixture.CreateValidUpdateMovieRequestDto();
        _movieServiceMock.Setup(service => service.FindByIdAsync(movie.Id)).ReturnsAsync(movie);
        
        //Act
        var result = await _controller.PutAsync(movie.Id, updateMovieRequestDto);
        
        //Assert
        _movieServiceMock.Verify(service => service.FindByIdAsync(movie.Id), Times.Once);
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
        _movieServiceMock.Setup(service => service.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.PutAsync(33, updateMovieRequestDto);
        
        //Assert
        _movieServiceMock.Verify(service => service.FindByIdAsync(movieId), Times.Never);
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
        _movieServiceMock.Setup(service => service.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        _controller.ModelState.AddModelError("fakeError", "fakeError");
        
        //Act
        var result = await _controller.PutAsync(movieId, updateMovieRequestDto);
        
        //Assert
        _movieServiceMock.Verify(service => service.FindByIdAsync(movieId), Times.Never);
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
        _movieServiceMock.Setup(service => service.FindByIdAsync(movieId)).ReturnsAsync((Movie)null);
        
        //Act
        var result = await _controller.PutAsync(movieId, updateMovieRequestDto);
        
        //Assert
        _movieServiceMock.Verify(service => service.FindByIdAsync(movieId), Times.Once);
        _movieServiceMock.Verify(service => service.UpdateAsync(It.IsAny<Movie>()), Times.Never);
        _cachingServiceMock.Verify(cache => cache.SetAsync($"movie_{movieId}", It.IsAny<string>()), Times.Never);
        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion
    */
    #region DeleteMovieAsync
    
    [Fact(DisplayName = "DeleteOneMovieAsync Returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void DeleteOneMovieAsync_ReturnOk()
    {
        //Arrange
        const int movieId = 22;
        _movieServiceMock.Setup(service => service.AnyAsync(movieId)).ReturnsAsync(true);
        
        //Act
        var result = await _controller.DeleteAsync(movieId);
        
        //Assert
        _movieServiceMock.Verify(service => service.AnyAsync(movieId), Times.Once);
        _movieServiceMock.Verify(service => service.DeleteByIdAsync(movieId), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact(DisplayName = "DeleteOneMovieAsync Returns 404 NotFound")]
    [Trait("Controller", "Movies")]
    public async void DeleteOneMovieAsync_ReturnNotFound()
    {
        //Arrange
        const int movieId = 22;
        _movieServiceMock.Setup(service => service.AnyAsync(movieId)).ReturnsAsync(false);
        
        //Act
        var result = await _controller.DeleteAsync(movieId);
        
        //Assert
        _movieServiceMock.Verify(service => service.AnyAsync(movieId), Times.Once);
        _movieServiceMock.Verify(service => service.DeleteByIdAsync(movieId), Times.Never);
        result.Should().BeOfType<NotFoundResult>();
    }
    #endregion
}