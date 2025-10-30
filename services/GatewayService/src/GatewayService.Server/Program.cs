using GatewayService.Server.Extensions;
using Serilog;

namespace GatewayService.Server;

public class Program
{
    private const string _appsettingsFilename = "appsettings.json";

    public static async Task Main(string[] args)
    {
        Log.Logger = SerilogLoggerFactory.CreateProductionOrDefaultConfiguration(_appsettingsFilename);
        try
        {
            var host = CreateHostBuilder(args)
                .Build();
                
            await host.RunAsync();
                
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Stopped program because of exception!");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .SetupSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}