using System;
using Xunit;
using SmartMeterSimulation;

namespace SmartMeterTests
{
    public class SmartMeterTests
    {
        private readonly SmartMeter _smartMeter;

        public SmartMeterTests()
        {
            _smartMeter = new SmartMeter(24);
        }

        [Fact]
        public void SimulateConsumption_ReturnsConsumptionWithinExpectedRange()
        {
            // Arrange
            var privateMethod = typeof(SmartMeter)
                .GetMethod("SimulateConsumption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            double consumption = (double)privateMethod.Invoke(_smartMeter, new object[] { 9 });

            // Assert
            Assert.InRange(consumption, 2.0, 2.5);
        }

        [Fact]
        public void SimulateTotalProduction_ReturnsPositiveValue()
        {
            // Arrange
            var privateMethod = typeof(SmartMeter)
                .GetMethod("SimulateTotalProduction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            double production = (double)privateMethod.Invoke(_smartMeter, new object[] { 10 });

            // Assert
            Assert.True(production > 0, "Total energy production should be positive.");
        }

        [Fact]
        public void Simulate_24Hours_SimulatesWithoutError()
        {
            // Act & Assert
            _smartMeter.SimulateAsync(24);
        }
    }
}