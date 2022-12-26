using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApi.Application.AutoMapper;
using MovieApi.Application.DTOs.Request;
using MovieApi.Application.Notifications;
using MovieApi.Controllers;
using MovieApi.Domain.Entities;
using MovieApi.Domain.Interfaces;

namespace MovieApiTest.Tests;

public class MoviesControllerTest
{
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly MoviesController _controller;
    
    public MoviesControllerTest()
    {
        _movieServiceMock = new Mock<IMovieService>();
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())));
        _controller = new MoviesController(_movieServiceMock.Object, mapper, new Notifier());
    }
    
    #region PostMovie

    [Fact(DisplayName = "PostMovie returns 200 Ok")]
    [Trait("Controller", "Movies")]
    public async void PostMovie_ReturnOk()
    {
        //Arrange
        var movie = new MovieRequestDto
        {
            Title = "Spirited Away",
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.
The family's car stops in front of a tunnel leading to what appears to be an abandoned amusement park, which Chihiro's father insists on exploring, despite his daughter's protest.
They find a seemingly empty restaurant still stocked with food, which Chihiro's parents immediately begin to eat.
While exploring further, Chihiro reaches an enormous bathhouse and meets a boy named Haku, who warns her to return across the riverbed before sunset.
However, Chihiro discovers that her parents have been transformed into pigs, and she is unable to cross the now-flooded river.",
            InTheaters = false,
            OffTheatersDate = DateTime.Now.AddDays(-10)
        };

        //Act
        var result = await _controller.PostAsync(movie);

        //Assert

        _movieServiceMock.Verify(x => x.AddAsync(It.IsAny<Movie>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }

    
    
    [Fact(DisplayName = "PostMovie returns 400 BadRequest")]
    [Trait("Controller", "Movies")]
    public async void PostMovie_ReturnBadRequest()
    {
        //Arrange
        
        //creating a movie without title
        var movie = new MovieRequestDto
        {
            ReleaseDate = DateTime.Now,
            Summary = @"Ten-year-old Chihiro Ogino and her parents are traveling to their new home when her father decides to take a shortcut.
The family's car stops in front of a tunnel leading to what appears to be an abandoned amusement park, which Chihiro's father insists on exploring, despite his daughter's protest.
They find a seemingly empty restaurant still stocked with food, which Chihiro's parents immediately begin to eat.
While exploring further, Chihiro reaches an enormous bathhouse and meets a boy named Haku, who warns her to return across the riverbed before sunset.
However, Chihiro discovers that her parents have been transformed into pigs, and she is unable to cross the now-flooded river.",
            InTheaters = true,
            OffTheatersDate = DateTime.Now.AddDays(+10)
        };
        _controller.ModelState.AddModelError("fakeError", "fakeError");

        //Act
        var result = await _controller.PostAsync(movie);

        //Assert
        _movieServiceMock.Verify(x => x.AddAsync(It.IsAny<Movie>()), Times.Never);
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    #endregion
    
}