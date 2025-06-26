using Auth1796.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auth1796.Infrastructure;

public static class DbInitializerCopy
{

    public static async void UseSeeding(this IHost host, IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
        if (!context.Roles.Any())
        {
            await serviceScope.ServiceProvider.GetRequiredService<IDatabaseInitiliser>().SeedDatas();
        }
    }
}
