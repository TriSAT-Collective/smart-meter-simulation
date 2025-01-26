using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates wind energy production, which varies randomly throughout the day.
///     Wind production can peak at any time, but there's randomness throughout.
/// </summary>
public class WindEnergy : EnergySource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindEnergy"/> class with the specified logger and settings.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="settings">The energy sources settings.</param>
    public WindEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }
    /// <summary>
    /// Gets the type of the energy source, which is wind.
    /// </summary>
    public override EnergySourceType SourceType => EnergySourceType.Wind;

    /// <summary>
    /// Simulates the wind energy production for a given timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp for which to simulate energy production.</param>
    /// <param name="rand">The random number generator.</param>
    /// <returns>The simulated wind energy production in kWh.</returns>
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        Logger.LogTrace("Simulating wind energy production for {TimeStamp}", timeStamp);
        var hour = timeStamp.Hour;
        return Math.Round(rand.NextDouble() * 3.0, Settings.ResultDecimalPlaces);
    }
}