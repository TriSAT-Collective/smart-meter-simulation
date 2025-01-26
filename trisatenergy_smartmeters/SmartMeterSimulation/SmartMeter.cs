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
    private readonly List<EnergySource> _energySources = [];
    private readonly ILogger<SmartMeter> _logger;
    private readonly Random _rand = new();

    private readonly AppSettings _settings;
    private IChannel _rabbitMQChannel;
    private IConnection _rabbitMQConnection;

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
    }

    public async Task Stop()
    {
        _logger.LogInformation("SmartMeter application stopped.");
        _logger.LogInformation($"Total Consumption: {lifetimeConsumption:0.00} kWh");
        _logger.LogInformation($"Total Production: {lifetimeProduction:0.00} kWh");
    }

    public async Task Start()
    {
        var factory = new ConnectionFactory
        {
            Uri = _settings.RabbitMQ.Uri,
            VirtualHost = _settings.RabbitMQ.VirtualHost,
            UserName = _settings.RabbitMQ.Username,
            Password = _settings.RabbitMQ.Password
        };

        _rabbitMQConnection = await factory.CreateConnectionAsync();
        _rabbitMQChannel = await _rabbitMQConnection.CreateChannelAsync();

        DateTime startTime = _settings.Misc.SimulationStartTime ?? DateTime.Now;
        if (_settings.Misc.ContinuousSimulation)
        {
            _logger.LogInformation("Starting continuous simulation...");
            await ContinuousSimulation(startTime);
        }
        else
        {
            _logger.LogInformation("Starting once-off simulation... Simulating {Hours} hours",
                _settings.Misc.OnceOffSimulationHours);
            await OnceOffSimulation(startTime, _settings.Misc.OnceOffSimulationHours);
        }
    }

    private async Task ContinuousSimulation(DateTime startTime)
    {
        startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0, startTime.Kind);
        while (true)
        {
            await OnceOffSimulation(startTime, 1);
            await Task.Delay(_settings.Misc.ContinuousSimulationIntervalMs);
            startTime = startTime.AddHours(1);
        }
    }


    /// <summary>
    ///     Simulates energy consumption and production for the specified number of hours.
    /// </summary>
    /// <param name="hours">Number of hours to simulate.</param>
    private async Task OnceOffSimulation(DateTime startTime, int hours)
    {
        var routingKeyBase = _settings.RabbitMQ.RoutingKeyBase;
        var routingKeySuffix = _settings.Misc.MaintenanceMode ? "MAINTENANCE" : "REGULAR";
        var routingKey = $"{routingKeyBase}.{routingKeySuffix}";

        // round down to clean hour keep timezone suffix
        startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0, startTime.Kind);

        for (var hour = 0; hour < hours; hour++)
        {
            DateTime timeStamp = startTime.AddHours(hour);

            // Simulate energy production for this hour
            var productionBySource = SimulateProductionAllEnergySources(timeStamp);
            var totalProduction = productionBySource.Values.Sum();

            // Simulate household energy consumption for this hour
            var consumption = Math.Round(SimulateConsumption(timeStamp), _settings.EnergySources.ResultDecimalPlaces);

            // Prepare a message to publish
            var payload = new SmartMeterResultPayload
            {
                Id = _settings.Misc.SmartMeterId,
                Timestamp = timeStamp,
                TotalConsumption = consumption,
                TotalProduction = productionBySource.Values.Sum(),
                ProductionBySource = productionBySource,
                MaintenanceMode = _settings.Misc.MaintenanceMode
            };

            var json = JsonSerializer.Serialize(payload);

            // Publish the message with the appropriate routing key
            await _rabbitMQChannel.BasicPublishAsync(
                _settings.RabbitMQ.ExchangeName,
                routingKey,
                Encoding.UTF8.GetBytes(json));

            _logger.LogInformation($"Sent message to routingKey '{routingKey}': {json}");

            lifetimeProduction += totalProduction;
            lifetimeConsumption += consumption;
        }
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

        foreach (EnergySourceType type in _settings.EnergySources.EnabledSources)
        {
            EnergySource energySource = EnergySourceFactory.CreateEnergySource(type, _logger, _settings.EnergySources);
            var production = energySource.SimulateProduction(timeStamp, _rand);
            productionBySource[energySource.SourceType] = production;
        }

        return productionBySource;
    }
}