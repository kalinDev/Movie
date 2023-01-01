using FluentAssertions;
using MovieApi.Application.Validations;
using MovieApiTest.Fixtures;

namespace MovieApiTest.Tests;

public class MovieValidatorTest : IClassFixture<MovieFixture>
{
    private readonly MovieFixture _movieFixture;
    public MovieValidatorTest( MovieFixture movieFixture)
    {
        _movieFixture = movieFixture;
    }

    #region AllPropertiesAreValid
    
    [Fact(DisplayName = "Validate All Properties Of Movie")]
    [Trait("Validator", "Movies")]
    public void MovieValidatorTest_ValidateMovie()
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        Assert.True(result.IsValid);
    }
    
    #endregion

    #region ValidateTitle

    [Theory(DisplayName = "ValidateTitle Returns False")]
    [InlineData("")]
    [InlineData("C")]
    [Trait("Validator", "Movies")]
    public void MovieValidatorTest_ValidateTitle_ReturnFalse(string title)
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        movie.Title = title;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }
    
    [Theory(DisplayName = "ValidateTitle Returns True")]
    [InlineData("Minions the rise of gru")]
    [Trait("Validator", "Movies")]
    public void MovieValidatorTest_ValidateTitle(string title)
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        movie.Title = title;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Title");
    }
    
    #endregion
    
    #region ValidateSummary

    [Theory(DisplayName = "ValidateSummary Returns False")]
    [InlineData("")]
    [InlineData("The Movie")]
    [Trait("Validator", "Movies")]
    public void MovieValidatorTest_ValidateSummary_ReturnFalse(string summary)
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        movie.Summary = summary;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Summary");
    }
    
    [Theory(DisplayName = "ValidateSummary Returns True")]
    [InlineData(@"As 12-year-old Gru summons up the courage to prove his worth,
 determined to live out his wicked dreams,
 the brazen theft of the powerful Zodiac Stone and the chance encounter with Vicious Six founder Wild Knuckles set a comic,
 high-energy adventure in motion.")]
    [InlineData("The Movie is a movie about a young boy")]
    [Trait("Validator", "Movies")]
    public void MovieValidatorTest_ValidateSummary(string summary)
    {
        //Arrange
        var movie = _movieFixture.CreateValidMovie();
        movie.Summary = summary;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Summary");
    }

    #endregion
    
}