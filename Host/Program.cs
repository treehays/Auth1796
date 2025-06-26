using Auth1796.Core.Application;
using Auth1796.Infrastructure;
using Auth1796.Infrastructure.Common.Logging.Serilog;
using Auth1796.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.RegisterSerilog();
    var config = builder.Configuration;
    builder.Services.AddControllers();

    builder.Services.AddHttpClient();
    builder.Services.AddInfrastructure(config);
    builder.Services.AddApplication(config);

    var serviceProvider = builder.Services.BuildServiceProvider();
    var databaseSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;

    var app = builder.Build();
    app.UseSeeding(app, config);
    app.UseInfrastructure(config);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}