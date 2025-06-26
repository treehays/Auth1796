using Auth1796.Infrastructure;
using Auth1796.Infrastructure.Auth;
using Auth1796.Infrastructure.Common;
using Auth1796.Infrastructure.Common.Cors;
using Auth1796.Infrastructure.Common.Middleware;
using Auth1796.Infrastructure.Common.OpenApi;
using Auth1796.Infrastructure.Persistence;
using Auth1796.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;

namespace Auth1796.Infrastructure;

public static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(Core.Application.Startup).GetTypeInfo().Assembly;
        return services
            .AddApiVersioning()
            .AddAuth(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddOpenApiDocumentation(config)
            .AddDatabase(config)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseSecurityHeaders(config)
            .UseStaticFiles()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        return builder;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddDbContext<ApplicationDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                var isLive = bool.Parse(configuration.GetSection("AppSettings:IsLive").Value ?? "false");
                m.UseSqlServer(isLive ? databaseSettings.ProdConnectionString : databaseSettings.Auth1796,
                    options => options.EnableRetryOnFailure().MigrationsAssembly("Auth1796.Infrastructure"));
            });
    }
}