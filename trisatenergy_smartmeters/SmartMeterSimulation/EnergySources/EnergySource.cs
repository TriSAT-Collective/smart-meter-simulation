using System;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources
{
    /// <summary>
    /// Abstract base class for different energy sources.
    /// All energy sources should implement the SimulateProduction method.
    /// </summary>
    public abstract class EnergySource
    {
        /// <summary>
        /// Simulates the production of energy for a given hour.
        /// </summary>
        /// <param name="hour">The hour of the day (0-23).</param>
        /// <param name="rand">A Random object for generating fluctuations.</param>
        /// <returns>Amount of energy produced in kWh.</returns>
        public abstract double SimulateProduction(int hour, Random rand);
    }
}
