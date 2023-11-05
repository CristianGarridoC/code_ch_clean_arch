using Application.Abstractions;
using Infrastructure.Cache;
using Infrastructure.Persistence;
using Infrastructure.Provider;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("Redis:Server");
            options.InstanceName = configuration.GetValue<string>("Redis:InstanceName");
        });
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<ICacheService, CacheService>();
        return services;
    }
}