using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates small production from various other sources like geothermal or battery.
///     This is typically consistent with minor fluctuations.
/// </summary>
public class OtherEnergy : EnergySource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OtherEnergy"/> class with the specified logger and settings.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="settings">The energy sources settings.</param>
    public OtherEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }
    /// <summary>
    /// Gets the type of the energy source, which is other.
    /// </summary>
    public override EnergySourceType SourceType => EnergySourceType.Other;

    /// <summary>
    /// Simulates the other energy production for a given timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp for which to simulate energy production.</param>
    /// <param name="rand">The random number generator.</param>
    /// <returns>The simulated other energy production in kWh.</returns>
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        Logger.LogTrace("Simulating other energy production for {TimeStamp}", timeStamp);
        var baseProduction = 1.0;
        var hour = timeStamp.Hour;
        var fluctuation = rand.NextDouble() * 0.2;
        return Math.Round(baseProduction + fluctuation, Settings.ResultDecimalPlaces);
    }
}