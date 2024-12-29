namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources
{
    /// <summary>
    /// Simulates small production from various other sources like geothermal or battery.
    /// This is typically consistent with minor fluctuations.
    /// </summary>
    public class OtherEnergy : EnergySource
    {
        public EnergySourceType SourceType = EnergySourceType.Other;
        
        /// <inheritdoc />
        public override double SimulateProduction(DateTime timeStamp, Random rand)
        {
            int hour = timeStamp.Hour;
            double baseProduction = 1.0;
            double fluctuation = rand.NextDouble() * 0.2;
            return baseProduction + fluctuation;
        }
    }
}


