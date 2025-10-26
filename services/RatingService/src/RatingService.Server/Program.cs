using RatingService.Core.Interfaces;
using RatingService.Server.Extensions;
using Serilog;

namespace RatingService.Server;

public class Program
{
    private const string _appsettingsFilename = "appsettings.json";

    public static async Task Main(string[] args)
    {
        Log.Logger = SerilogLoggerFactory.CreateProductionOrDefaultConfiguration(_appsettingsFilename);
        try
        {
            var host = CreateHostBuilder(args)
                .Build()
                .MigrateDatabase();
            
            if (Environment.GetEnvironmentVariable("INIT_DATABASE") == "true")
            {
                using var scope = host.Services.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
                await initializer.InitializeAsync();
            }
            
            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Stopped program because of exception!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .SetupSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}