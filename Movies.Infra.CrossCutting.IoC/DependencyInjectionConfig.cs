using MovieApi.Application.Interfaces;
using MovieApi.Application.Notifications;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Data;

namespace Movies.Infra.CrossCutting.IoC;

public static class DependencyInjectionConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<ApiDbContext>();
    }
}