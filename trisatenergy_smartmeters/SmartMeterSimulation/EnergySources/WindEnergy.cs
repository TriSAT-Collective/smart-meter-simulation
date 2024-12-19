using System;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources
{
    /// <summary>
    /// Simulates wind energy production, which varies randomly throughout the day.
    /// Wind production can peak at any time, but there's randomness throughout.
    /// </summary>
    public class WindEnergy : EnergySource
    {
        /// <inheritdoc />
        public override double SimulateProduction(int hour, Random rand)
        {
            // Wind is more unpredictable but typically fluctuates throughout the day
            return rand.NextDouble() * 3.0;
        }
    }
}
