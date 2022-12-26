using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieApi.Application.Interfaces;
using FluentValidation.Results;
using MovieApi.Application.Notifications;

namespace MovieApi.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private readonly INotifier _notifier;
    protected ApiController(INotifier notifier) 
        => _notifier = notifier;
        

    protected ActionResult CustomResponse(object? result = null)
    {
        if (IsOperationValid())
        {
            return Ok(result);
        }

        return BadRequest(new
        {
            sucess = false,
            errors = _notifier.GetNotifications().Select(n => n.Message)
        }); 
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            AddError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            AddError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }

    protected void AddError(string erro)
    {
        _notifier.Handle(new Notification(erro));
    }

}