using System;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources
{
    /// <summary>
    /// Simulates wind energy production, which varies randomly throughout the day.
    /// Wind production can peak at any time, but there's randomness throughout.
    /// </summary>
    public class WindEnergy : EnergySource
    {
        public EnergySourceType SourceType = EnergySourceType.Wind;
        
        /// <inheritdoc />
        public override double SimulateProduction(DateTime timeStamp, Random rand)
        {
            var hour = timeStamp.Hour;
            return rand.NextDouble() * 3.0;
        }
    }
}
