using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using trisatenergy_smartmeters;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace SmartMeterTests
{
    public class SmartMeterTests
    {
        private readonly Mock<IOptions<AppSettings>> _mockOptions;
        private readonly Mock<ILogger<SmartMeter>> _mockLogger;
        private readonly SmartMeter _smartMeter;

        public SmartMeterTests()
        {
            // Mock AppSettings
            _mockOptions = new Mock<IOptions<AppSettings>>();
            _mockOptions.Setup(o => o.Value).Returns(new AppSettings
            {
                RabbitMQ = new AppSettings.RabbitMQSettings
                {
                    Uri = new Uri("amqp://localhost"),
                    VirtualHost = "/smartmetres",
                    Username = "guest",
                    Password = "guest"
                },
                Misc = new AppSettings.MiscSettings
                {
                    MaintenanceMode = false
                },
                EnergySources = new AppSettings.EnergySourcesSettings
                {
                    SolarEnergyProduction = 5.0,
                    WindEnergyProduction = 7.5,
                    OtherEnergyProduction = 2.5
                }
            });

            // Mock Logger
            _mockLogger = new Mock<ILogger<SmartMeter>>();

            // Create an instance of SmartMeter with mocked dependencies
            _smartMeter = new SmartMeter(_mockOptions.Object, _mockLogger.Object);
        }

        [Fact]
        public void SimulateConsumption_ReturnsConsumptionWithinExpectedRange()
        {
            // Act
            var privateMethod = typeof(SmartMeter)
                .GetMethod("SimulateConsumption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            double consumption = (double)privateMethod.Invoke(_smartMeter, new object[] { 9 });

            // Assert
            Assert.InRange(consumption, 2.0, 2.5);
        }

        [Fact]
        public void SimulateTotalProduction_ReturnsPositiveValue()
        {
            // Act
            var privateMethod = typeof(SmartMeter)
                .GetMethod("SimulateTotalProduction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            double production = (double)privateMethod.Invoke(_smartMeter, new object[] { 10 });

            // Assert
            Assert.True(production > 0, "Total energy production should be positive.");
        }

        [Fact]
        public async void Simulate_24Hours_SimulatesWithoutError()
        {
            // Act & Assert
            await _smartMeter.SimulateAsync(24);
        }
    }
}
