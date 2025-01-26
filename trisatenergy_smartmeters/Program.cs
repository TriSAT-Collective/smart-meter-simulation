using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_smartmeters;

/// <summary>
///     Entry point of the application that runs the smart meter simulation.
/// </summary>
internal class Program
{
    private static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Prevent immediate termination
            cancellationTokenSource.Cancel();
        };

        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("config.json", false, true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<AppSettings>(context.Configuration.GetSection(nameof(AppSettings)));
                services.AddLogging(builder =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("AppSettings:Logging"));
                    builder.AddConsole();
                });
                services.AddTransient<SmartMeter>();
            })
            .Build();

        using IServiceScope scope = host.Services.CreateScope();
        var smartMeter = scope.ServiceProvider.GetRequiredService<SmartMeter>();

        // Start the application
        Task smartMeterTask = smartMeter.Start();

        // Wait for the application to complete or the shutdown signal
        await Task.WhenAny(smartMeterTask, Task.Delay(Timeout.Infinite, cancellationTokenSource.Token));

        // Perform cleanup tasks
        await smartMeter.Stop();
    }
}