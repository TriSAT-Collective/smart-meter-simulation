using System;
using System.Collections.Generic;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;


namespace SmartMeterSimulation.EnergySources
{
    /// <summary>
    /// Simulates small production from various other sources like geothermal or battery.
    /// This is typically consistent with minor fluctuations.
    /// </summary>
    public class OtherEnergy : EnergySource
    {
        /// <inheritdoc />
        public override double SimulateProduction(int hour, Random rand)
        {
            // Constant base production with small fluctuations
            double baseProduction = 1.0;
            double fluctuation = rand.NextDouble() * 0.2;
            return baseProduction + fluctuation;
        }
    }
}


