using FluentAssertions;
using MovieApi.Application.Validations;
using MovieApi.Domain.Enums;
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
    public void ValidateMovie_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
    }
    
    #endregion

    #region ValidateTitle

    [Theory(DisplayName = "ValidateTitle Returns Error")]
    [InlineData("")]
    [InlineData("C")]
    [Trait("Validator", "Movies")]
    public void ValidateTitle_ReturnError(string title)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Title = title;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }
    
    [Theory(DisplayName = "ValidateTitle Returns Success")]
    [InlineData("Minions the rise of gru")]
    [Trait("Validator", "Movies")]
    public void ValidateTitle_ReturnSuccess(string title)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
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

    [Theory(DisplayName = "ValidateSummary Returns Error")]
    [InlineData("")]
    [InlineData("The Movie")]
    [Trait("Validator", "Movies")]
    public void ValidateSummary_ReturnError(string summary)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Summary = summary;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Summary");
    }
    
    [Theory(DisplayName = "ValidateSummary Returns Success")]
    [InlineData(@"As 12-year-old Gru summons up the courage to prove his worth,
 determined to live out his wicked dreams,
 the brazen theft of the powerful Zodiac Stone and the chance encounter with Vicious Six founder Wild Knuckles set a comic,
 high-energy adventure in motion.")]
    [InlineData("The Movie is a movie about a young boy")]
    [Trait("Validator", "Movies")]
    public void ValidateSummary_ReturnSuccess(string summary)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Summary = summary;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Summary");
    }

    #endregion
    
    #region ValidatePosterUri
    
    [Theory(DisplayName = "ValidatePosterUri Returns Success")]
    [InlineData("https://google.com/")]
    [Trait("Validator", "Movies")]
    public void ValidatePosterUri_ReturnSuccess(string posterUri)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.PosterUri = posterUri;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "PosterUri");
    }
    
    [Theory(DisplayName = "ValidatePosterUri Returns Error")]
    [InlineData("")]
    [InlineData("google.com")]
    [InlineData("www.google.com")]
    [Trait("Validator", "Movies")]
    public void ValidatePosterUri_ReturnError(string posterUri)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.PosterUri = posterUri;
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PosterUri");

    }
    
    #endregion
    
    #region ValidateReleaseDate
    
    [Fact(DisplayName = "ValidateReleaseDate With Current Date Apart Returns Success")]
    [Trait("Validator", "Movies")]
    public void ValidateReleaseDate_WithCurrentDate_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now.AddDays(-1).AddMinutes(1);
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "ReleaseDate");
    }

    
    [Fact(DisplayName = "ValidateReleaseDate With Up To 24 Hours Apart Returns Success")]
    [Trait("Validator", "Movies")]
    public void ValidateReleaseDate_WithUpTo24h_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now.AddDays(-1).AddMinutes(1);
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "ReleaseDate");
    }
    
    [Fact(DisplayName = "ValidateReleaseDate With More Than 24 Hours apart Returns Error")]
    [Trait("Validator", "Movies")]
    public void ValidateReleaseDate_WithMoreThan24h_ReturnError()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now.AddDays(-1);
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ReleaseDate");
    }
    
    [Fact(DisplayName = "ValidateReleaseDate With A Year Early apart Returns Error")]
    [Trait("Validator", "Movies")]
    public void ValidateReleaseDate_WithYearEarly_ReturnError()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now.AddYears(1).AddMinutes(1);
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ReleaseDate");
    }
    
    #endregion

    #region OffTheatersDate

    [Fact(DisplayName = "ValidateOffTheatersDate Returns Success")]
    [Trait("Validator", "Movies")]
    public void ValidateOffTheatersDate_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now;
        movie.OffTheatersDate = DateTime.Now.AddDays(1);
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "OffTheatersDate");
    }

    [Fact(DisplayName = "ValidateOffTheatersDate With Date Less Than Release Returns Error")]
    [Trait("Validator", "Movies")]
    public void ValidateOffTheatersDate_WithDateLessThanRelease_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.ReleaseDate = DateTime.Now;
        movie.OffTheatersDate = DateTime.Now.AddDays(-1);
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OffTheatersDate");
    }
    #endregion
    
    #region ValidateRoom

    [Fact(DisplayName = "ValidateRoom Returns Success")]
    [Trait("Validator", "Movies")]
    public void ValidateRoom_ReturnSuccess()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Room = Room.Imax;
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Room");
    }
    
    [Fact(DisplayName = "ValidateRoom Returns Error")]
    [Trait("Validator", "Movies")]
    public void ValidateRoom_ReturnError()
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Room = (Room)300;
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Room");
    }
    #endregion

    #region ValidateDuration

    [Theory(DisplayName = "ValidateDuration Returns Success")]
    [InlineData(10)]
    [InlineData(120)]
    [Trait("Validator", "Movies")]
    public void ValidateDuration_ReturnSuccess(int minutes)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Duration = TimeSpan.FromMinutes(minutes);      
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Duration");
    }

    [Theory(DisplayName = "ValidateDuration Returns Error")]
    [InlineData(0)]
    [InlineData(9)]
    [Trait("Validator", "Movies")]
    public void ValidateDuration_ReturnError(int minutes)
    {
        //Arrange
        var movie = _movieFixture.ValidMovie();
        movie.Duration = TimeSpan.FromMinutes(minutes);    
        
        var movieValidator = new MovieValidator();
        
        //Act
        var result = movieValidator.Validate(movie);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Duration");
    }
    
    #endregion
}