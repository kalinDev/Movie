using FluentValidation;
using FluentValidation.Results;
using MovieApi.Application.Interfaces;
using MovieApi.Application.Notifications;
using MovieApi.Domain.Entities;

namespace MovieApi.Application.Services;

public abstract class BaseService
{
    private readonly INotifier _notifier;

    protected BaseService(INotifier notifier)
        => _notifier = notifier;


    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string message)
        => _notifier.Handle(new Notification(message));

    protected bool RunValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validation.Validate(entity);
        if (validator.IsValid) return true;
        Notify(validator);
        return false;
    }
}