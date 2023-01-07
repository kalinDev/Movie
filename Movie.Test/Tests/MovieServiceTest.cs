using AutoMapper;
using FluentAssertions;
using Moq;
using MovieApi.Application.AutoMapper;
using MovieApi.Application.Notifications;
using MovieApi.Application.Services;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;
using MovieApiTest.Fixtures;

namespace MovieApiTest.Tests;

public class MovieServiceTest: IClassFixture<MovieFixture>
{

    private readonly MovieFixture _movieFixture;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly IMovieService _movieService;
    private readonly Notifier _notifier;
    
    public MovieServiceTest(MovieFixture movieFixture)
    {
        _notifier = new Notifier();
        _movieFixture = movieFixture;
        _movieRepositoryMock = new Mock<IMovieRepository>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())));
        _movieService = new MovieService(_movieRepositoryMock.Object, mapper, _notifier);
    }

    #region AddMovieAsync
    
    [Fact(DisplayName = "AddMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void AddMovieAsync_Success()
    {
        //Arrange
        var movieRequestDto = _movieFixture.ValidMovieRequestDto();
        
        //Act
        await _movieService.AddAsync(movieRequestDto);

        //Assert
        _notifier.HasNotification().Should().BeFalse();
        _movieRepositoryMock.Verify(repository => repository.Add(It.IsAny<Movie>()), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "AddMovieAsync Fail")]
    [Trait("Service", "Movies")]
    public async void AddMovieAsync_Fail()
    {
        //Arrange
        var movieRequestDto = _movieFixture.InvalidMovieRequestDto();
        
        //Act
        await _movieService.AddAsync(movieRequestDto);

        //Assert
        _notifier.HasNotification().Should().BeTrue();
        _movieRepositoryMock.Verify(repository => repository.Add(It.IsAny<Movie>()), Times.Never);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region UpdateMovieAsync

    [Fact(DisplayName = "UpdateMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void UpdateMovieAsync_Success()
    {
        //Arrange
        var updateMovieRequestDto = _movieFixture.ValidUpdateMovieRequestDto();
        
        //Act
        await _movieService.UpdateAsync(updateMovieRequestDto);

        //Assert
        _notifier.HasNotification().Should().BeFalse();
        _movieRepositoryMock.Verify(repository => repository.Update(It.IsAny<Movie>()), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "UpdateMovieAsync Fail")]
    [Trait("Service", "Movies")]
    public async void UpdateMovieAsync_Fail()
    {
        //Arrange
        var invalidMovie = _movieFixture.InvalidUpdateMovieRequestDto();
        
        //Act
        await _movieService.UpdateAsync(invalidMovie);

        //Assert
        _notifier.HasNotification().Should().BeTrue();
        _movieRepositoryMock.Verify(repository => repository.Update(It.IsAny<Movie>()), Times.Never);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region RemoveMovieAsync

    [Fact(DisplayName = "RemoveMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void RemoveMovieAsync_Success()
    {
        //Arrange
        const int movieId = 22;
        
        //Act
        await _movieService.DeleteByIdAsync(movieId);

        //Assert
        _movieRepositoryMock.Verify(repository => repository.Remove(movieId), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }

    #endregion
}