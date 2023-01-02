using FluentValidation;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.Validations;

public class MovieValidator : AbstractValidator<Movie>
{
    public MovieValidator()
    {
        RuleFor(x => x.Title).Length(2, 100).NotEmpty();
        RuleFor(x => x.Summary).Length(10, 500).NotEmpty();
        RuleFor(x => x.PosterUri).Must(uri => 
            Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.PosterUri)).NotEmpty();
        RuleFor(x => x.ReleaseDate).GreaterThan(DateTime.Now.AddDays(-1)).LessThan(DateTime.Now.AddYears(1)).NotEmpty();
        RuleFor(x => x.OffTheatersDate).GreaterThan(x => x.ReleaseDate).NotEmpty();
    }
}