using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth1796.Core.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddMediatR(assembly);
    }
}