using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_smartmeters;

/// <summary>
///     Entry point of the application that runs the smart meter simulation.
/// </summary>
internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config
                    .SetBasePath(AppDomain.CurrentDomain
                        .BaseDirectory) // Set the base path to the current directory
                    .AddJsonFile("config.json", false, true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Register AppSettings as a configuration instance
                services.Configure<AppSettings>(context.Configuration.GetSection(nameof(AppSettings)));
                // Add logging
                services.AddLogging();
                // Register the SmartMeter service
                services.AddTransient<SmartMeter>();
            })
            .Build();

        using IServiceScope scope = host.Services.CreateScope();
        var smartMeter = scope.ServiceProvider.GetRequiredService<SmartMeter>();
        DateTime startTime = DateTime.Now;
        await smartMeter.SimulateAndPublish(startTime, 24);
    }
}