using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters.SmartMeterSimulation;

/// <summary>
///     The SmartMeter class simulates household energy consumption and production from various sources.
/// </summary>
public class SmartMeter
{
    private const string ExchangeName = "smartmetres_exchange";
    private readonly List<EnergySource> _energySources = [];
    private readonly ILogger<SmartMeter> _logger;
    private readonly Random _rand = new();

    private readonly AppSettings _settings;

    private double lifetimeConsumption;
    private double lifetimeProduction;

    /// <summary>
    ///     Initializes a new instance of the SmartMeter class with the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours to simulate (typically 24 for one day).</param>
    public SmartMeter(IOptions<AppSettings> settings, ILogger<SmartMeter> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        _energySources.Add(new SolarEnergy());
        _energySources.Add(new WindEnergy());
        _energySources.Add(new OtherEnergy());
    }

    /// <summary>
    ///     Simulates energy consumption and production for the specified number of hours.
    /// </summary>
    /// <param name="hours">Number of hours to simulate.</param>
    public async Task SimulateAndPublish(DateTime startTime, int hours)
    {
        // Setup RabbitMQ connection and channel
        var factory = new ConnectionFactory
        {
            Uri = _settings.RabbitMQ.Uri,
            VirtualHost = _settings.RabbitMQ.VirtualHost,
            UserName = _settings.RabbitMQ.Username,
            Password = _settings.RabbitMQ.Password
        };

        await using IConnection connection = await factory.CreateConnectionAsync();
        await using IChannel channel = await connection.CreateChannelAsync();

        var routingKey = _settings.Misc.MaintenanceMode ? "MAINTENANCE" : "REGULAR";

        for (var hour = 0; hour < hours; hour++)
        {
            DateTime timeStamp = startTime.AddHours(hour);

            // Simulate energy production for this hour
            var productionBySource = SimulateProductionAllEnergySources(timeStamp);
            var totalProduction = productionBySource.Values.Sum();

            // Simulate household energy consumption for this hour
            var consumption = SimulateConsumption(timeStamp);

            // Prepare a message to publish
            var payload = new SmartMeterResultPayload
            {
                Timestamp = timeStamp,
                TotalConsumption = consumption,
                TotalProduction = productionBySource.Values.Sum(),
                ProductionBySource = productionBySource,
                MaintenanceMode = _settings.Misc.MaintenanceMode
            };

            var json = JsonSerializer.Serialize(payload);

            // var message =
            //     $"Hour {hour}: Consumption: {_consumption[hour]:0.00} kWh, Total Production: {hourlyProduction:0.00} kWh";

            // Optionally, append routing key (OK or MAINTENANCE) to the message body

            // var body = Encoding.UTF8.GetBytes($"{message} | Maintenance Status: {routingKey}");

            // Publish the message with the appropriate routing key
            await channel.BasicPublishAsync(
                ExchangeName,
                routingKey,
                Encoding.UTF8.GetBytes(json));

            _logger.LogInformation($"Sent message to {routingKey}: {json}");

            lifetimeProduction += totalProduction;
            lifetimeConsumption += consumption;
        }

        _logger.LogInformation($"Total Consumption: {lifetimeConsumption:0.00} kWh");
        _logger.LogInformation($"Total Production: {lifetimeProduction:0.00} kWh");
    }

    /// <summary>
    ///     Simulates household energy consumption for a given timestamp.
    /// </summary>
    /// <param name="timeStamp">the timestamp for which to simulate energy production</param>
    /// <returns>Total energy consumed in kWh.</returns>
    private double SimulateConsumption(DateTime timeStamp)
    {
        // Base consumption is higher in the morning and evening
        var baseConsumption =
            (timeStamp.Hour >= 7 && timeStamp.Hour <= 9) || (timeStamp.Hour >= 18 && timeStamp.Hour <= 21) ? 2.0 : 1.0;
        // Random fluctuation for realism
        return baseConsumption + _rand.NextDouble() * 0.5;
    }


    /// <summary>
    ///     Aggregates the energy production from all energy sources for a given hour.
    /// </summary>
    /// <param name="timeStamp">the timestamp for which to simulate energy production</param>
    /// <returns>Total energy produced in kWh.</returns>
    private Dictionary<EnergySourceType, double> SimulateProductionAllEnergySources(DateTime timeStamp)
    {
        var productionBySource = new Dictionary<EnergySourceType, double>();
        foreach (EnergySource source in _energySources)
        {
            var production = source.SimulateProduction(timeStamp, _rand);
            productionBySource[source.SourceType] = production;
        }

        return productionBySource;
    }
}