using FluentValidation;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.Validations;

public class MovieValidator : AbstractValidator<Movie>
{
    public MovieValidator()
    {
        RuleFor(x => x.Title).Length(2, 100).NotEmpty();
        RuleFor(x => x.Summary).Length(10, 500).NotEmpty();
    }
}