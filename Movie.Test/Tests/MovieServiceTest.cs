using FluentAssertions;
using Moq;
using MovieApi.Application.Notifications;
using MovieApi.Application.Services;
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
        _movieService = new MovieService(_movieRepositoryMock.Object, _notifier);
    }

    #region AddMovieAsync
    
    [Fact(DisplayName = "AddMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void AddMovieAsync_Success()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        
        //Act
        await _movieService.AddAsync(movie);

        //Assert
        _notifier.HasNotification().Should().BeFalse();
        _movieRepositoryMock.Verify(repository => repository.Add(movie), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "AddMovieAsync Fail")]
    [Trait("Service", "Movies")]
    public async void AddMovieAsync_Fail()
    {
        //Arrange
        var invalidMovie = _movieFixture.CreateInvalidMovie();
        
        //Act
        await _movieService.AddAsync(invalidMovie);

        //Assert
        _notifier.HasNotification().Should().BeTrue();
        _movieRepositoryMock.Verify(repository => repository.Add(invalidMovie), Times.Never);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region UpdateMovieAsync

    [Fact(DisplayName = "UpdateMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void UpdateMovieAsync_Success()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        
        //Act
        await _movieService.UpdateAsync(movie);

        //Assert
        _notifier.HasNotification().Should().BeFalse();
        _movieRepositoryMock.Verify(repository => repository.Update(movie), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "UpdateMovieAsync Fail")]
    [Trait("Service", "Movies")]
    public async void UpdateMovieAsync_Fail()
    {
        //Arrange
        var invalidMovie = _movieFixture.CreateInvalidMovie();
        
        //Act
        await _movieService.UpdateAsync(invalidMovie);

        //Assert
        _notifier.HasNotification().Should().BeTrue();
        _movieRepositoryMock.Verify(repository => repository.Update(invalidMovie), Times.Never);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region RemoveMovieAsync

    [Fact(DisplayName = "RemoveMovieAsync Success")]
    [Trait("Service", "Movies")]
    public async void RemoveMovieAsync_Success()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        
        //Act
        await _movieService.DeleteByIdAsync(movie.Id);

        //Assert
        _movieRepositoryMock.Verify(repository => repository.Remove(movie.Id), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }

    #endregion
}