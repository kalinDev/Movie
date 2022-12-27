using MovieApi.Application.Interfaces;
using MovieApi.Application.Notifications;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Application.Services;
using MovieApi.Data;
using MovieApi.Data.Repositories;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Infra.CrossCutting.IoC;

public static class DependencyInjectionConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<ApiDbContext>();
    }
}