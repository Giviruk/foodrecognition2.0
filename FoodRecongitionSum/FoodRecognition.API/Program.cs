using FoodRecognition.Database;
using Serilog;
using Serilog.Events;

namespace FoodRecognition.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        //Logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .WriteTo.Console()
            .WriteTo.File("/FoodRecognitionLogs/api/log.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 10485760)
            .CreateLogger();
        var host = CreateHostBuilder(args).Build();
        using var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        await SeedData.EnsureSeedData(scope.ServiceProvider);
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}