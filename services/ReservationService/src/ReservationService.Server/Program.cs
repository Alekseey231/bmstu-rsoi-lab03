using ReservationService.Server.Extensions;
using Serilog;

namespace ReservationService.Server;

public class Program
{
    private const string _appsettingsFilename = "appsettings.json";

    public static void Main(string[] args)
    {
        Log.Logger = SerilogLoggerFactory.CreateProductionOrDefaultConfiguration(_appsettingsFilename);
        try
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase()
                .Run();
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