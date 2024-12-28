using System;

namespace SmartMeterSimulation
{
    /// <summary>
    /// Entry point of the application that runs the smart meter simulation.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // Retrieve RabbitMQ credentials from environment variables
            string rabbitMqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            string rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

            if (string.IsNullOrEmpty(rabbitMqUsername) || string.IsNullOrEmpty(rabbitMqPassword))
            {
                Console.WriteLine("RabbitMQ credentials are missing. Please check your environment variables.");
                return; //  Exit the application if credentials are not found
            }

            Console.WriteLine($"Using RabbitMQ Username: {rabbitMqUsername}");

            // SimulateAsync 24 hours (1 day)
            int hours = 24;
            SmartMeter smartMeter = new SmartMeter(hours);
            await smartMeter.SimulateAsync(hours);
        }
    }
}