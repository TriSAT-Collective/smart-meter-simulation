using System;
using System.Collections;
using System.Collections.Generic;

using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;
using SmartMeterSimulation.EnergySources;
using RabbitMQ.Client;
using System.Text;

namespace SmartMeterSimulation
{
    /// <summary>
    /// The SmartMeter class simulates household energy consumption and production from various sources.
    /// </summary>
    public class SmartMeter
    {
        private readonly List<EnergySource> energySources = new List<EnergySource>();
        private readonly double[] consumption;
        private readonly bool isMaintenanceMode;
        private const string ExchangeName = "smartmetres_exchange";
        private readonly Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the SmartMeter class with the specified number of hours.
        /// </summary>
        /// <param name="hours">The number of hours to simulate (typically 24 for one day).</param>
        public SmartMeter(int hours)
        {
            consumption = new double[hours];
            energySources.Add(new SolarEnergy());
            energySources.Add(new WindEnergy());
            energySources.Add(new OtherEnergy());
            
            // Check the MAINTENANCE_MODE environment variable
            isMaintenanceMode = Environment.GetEnvironmentVariable("MAINTENANCE_MODE") == "1";
        }

        /// <summary>
        /// Simulates energy consumption and production for the specified number of hours.
        /// </summary>
        /// <param name="hours">Number of hours to simulate.</param>
        public async Task SimulateAsync(int hours)
        {
            double totalConsumption = 0;
            double totalProduction = 0;
            
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                Console.WriteLine("######################################################################");
                Console.WriteLine($"{entry.Key}: {entry.Value}");
                Console.WriteLine("######################################################################");
            }
            
            
            
            // Retrieve credentials from environment variables
            string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");


            // Setup RabbitMQ connection and channel
            var factory = new ConnectionFactory() {  
                Uri = new Uri("amqp://localhost") // Use AMQP URI for messaging
                , VirtualHost = "/smartmetres"   // Custom virtual host (optional)
                , UserName = username
                , Password = password
            }; 
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            // The exchange name
            string exchangeName = "smartmetres_exchange";
            
            for (int hour = 0; hour < hours; hour++)
            {
                // SimulateAsync household energy consumption for this hour
                consumption[hour] = SimulateConsumption(hour);
                // Calculate total energy production from all sources
                double hourlyProduction = SimulateTotalProduction(hour);
                
                totalConsumption += consumption[hour];
                totalProduction += hourlyProduction;
                
                // Prepare a message to publish
                var message = $"Hour {hour}: Consumption: {consumption[hour]:0.00} kWh, Total Production: {hourlyProduction:0.00} kWh";
        
                // Optionally, append routing key (OK or MAINTENANCE) to the message body
                string routingKey = isMaintenanceMode ? "MAINTENANCE" : "OK";
                var body = Encoding.UTF8.GetBytes($"{message} | Maintenance Status: {routingKey}");

                // Publish the message with the appropriate routing key
                channel.BasicPublishAsync(exchangeName, routingKey, body: body);

                Console.WriteLine($"Sent message to {routingKey}: {message} | Maintenance Status: {routingKey}");

               
            }

            Console.WriteLine($"\nTotal Consumption: {totalConsumption:0.00} kWh");
            Console.WriteLine($"Total Production: {totalProduction:0.00} kWh");
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
            return baseConsumption + rand.NextDouble() * 0.5;
        }

        /// <summary>
        /// Aggregates the energy production from all energy sources for a given hour.
        /// </summary>
        /// <param name="hour">The hour of the day (0-23).</param>
        /// <returns>Total energy produced in kWh.</returns>
        private double SimulateTotalProduction(int hour)
        {
            double totalProduction = 0;
            foreach (var source in energySources)
            {
                totalProduction += source.SimulateProduction(hour, rand);
            }
            return totalProduction;
        }
    }
}

