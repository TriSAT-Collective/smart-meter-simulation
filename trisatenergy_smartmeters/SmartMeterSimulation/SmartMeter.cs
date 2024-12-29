using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SmartMeterSimulation.EnergySources;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters.SmartMeterSimulation
{
    /// <summary>
    /// The SmartMeter class simulates household energy consumption and production from various sources.
    /// </summary>
    public class SmartMeter
    {

        private readonly AppSettings _settings;
        private readonly ILogger<SmartMeter> _logger;
        private readonly List<EnergySource> _energySources = [];
        private readonly double[] _consumption;
        private const string ExchangeName = "smartmetres_exchange";
        private readonly Random _rand = new Random();

        /// <summary>
        /// Initializes a new instance of the SmartMeter class with the specified number of hours.
        /// </summary>
        /// <param name="hours">The number of hours to simulate (typically 24 for one day).</param>
        public SmartMeter(IOptions<AppSettings> settings, ILogger<SmartMeter> logger )
        {
            _settings = settings.Value;
            _logger = logger;
            
            _energySources.Add(new SolarEnergy());
            _energySources.Add(new WindEnergy());
            _energySources.Add(new OtherEnergy());
            
            // Check the MAINTENANCE_MODE environment variable
        }

        /// <summary>
        /// Simulates energy consumption and production for the specified number of hours.
        /// </summary>
        /// <param name="hours">Number of hours to simulate.</param>
        public async Task SimulateAsync(int hours)
        {
            double totalConsumption = 0;
            double totalProduction = 0;
            

            // Setup RabbitMQ connection and channel
            var factory = new ConnectionFactory() {  
                Uri = _settings.RabbitMQ.Uri,
                VirtualHost = _settings.RabbitMQ.VirtualHost,   
                UserName = _settings.RabbitMQ.Username,
                Password = _settings.RabbitMQ.Password
            };

            await using IConnection connection = await factory.CreateConnectionAsync();
            await using IChannel channel = await connection.CreateChannelAsync();


            var routingKey = _settings.Misc.MaintenanceMode ? "MAINTENANCE" : "OK";
            
            for (var hour = 0; hour < hours; hour++)
            {
                // SimulateAsync household energy consumption for this hour
                _consumption[hour] = SimulateConsumption(hour);
                
                // Calculate total energy production from all sources
                var hourlyProduction = SimulateTotalProduction(hour);
                
                totalConsumption += _consumption[hour];
                totalProduction += hourlyProduction;
                
                // Prepare a message to publish
                var message = $"Hour {hour}: Consumption: {_consumption[hour]:0.00} kWh, Total Production: {hourlyProduction:0.00} kWh";
        
                // Optionally, append routing key (OK or MAINTENANCE) to the message body

                var body = Encoding.UTF8.GetBytes($"{message} | Maintenance Status: {routingKey}");

                // Publish the message with the appropriate routing key
                await channel.BasicPublishAsync(ExchangeName, routingKey, body: body);

                _logger.LogInformation($"Sent message to {routingKey}: {message} | Maintenance Status: {routingKey}");
            }

            _logger.LogInformation($"Total Consumption: {totalConsumption:0.00} kWh");
            _logger.LogInformation($"Total Production: {totalProduction:0.00} kWh");
        }

        /// <summary>
        /// Simulates the energy consumption for a given hour.
        /// </summary>
        /// <param name="hour">The hour of the day (0-23).</param>
        /// <returns>Amount of energy consumed in kWh.</returns>
        private double SimulateConsumption(int hour)
        {
            // Base consumption is higher in the morning and evening
            double baseConsumption = (hour >= 7 && hour <= 9) || (hour >= 18 && hour <= 21) ? 2.0 : 1.0;
            // Random fluctuation for realism
            return baseConsumption + _rand.NextDouble() * 0.5;
        }

        /// <summary>
        /// Aggregates the energy production from all energy sources for a given hour.
        /// </summary>
        /// <param name="hour">The hour of the day (0-23).</param>
        /// <returns>Total energy produced in kWh.</returns>
        private double SimulateTotalProduction(int hour)
        {
            double totalProduction = 0;
            foreach (var source in _energySources)
            {
                totalProduction += source.SimulateProduction(hour, _rand);
            }
            return totalProduction;
        }
    }
}

