using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates solar energy production based on daytime hours.
///     Production peaks between 6:00 and 18:00.
/// </summary>
public class SolarEnergy : EnergySource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolarEnergy"/> class with the specified logger and settings.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="settings">The energy sources settings.</param>
    public SolarEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }
    /// <summary>
    /// Gets the type of the energy source, which is solar.
    /// </summary>
    public override EnergySourceType SourceType => EnergySourceType.Solar;

    /// <summary>
    /// Simulates the solar energy production for a given timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp for which to simulate energy production.</param>
    /// <param name="rand">The random number generator.</param>
    /// <returns>The simulated solar energy production in kWh.</returns>
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        Logger.LogTrace("Simulating solar energy production for {TimeStamp}", timeStamp);
        // Solar production is based on daylight hours
        var hour = timeStamp.Hour;
        // Base solar production: more during daylight hours (6 AM - 6 PM)
        var baseProduction = hour >= 6 && hour <= 18 ? 5.0 : 0.0;
        // Random fluctuation for realistic variability
        var fluctuation = rand.NextDouble() * 1.0;
        return Math.Round(baseProduction + fluctuation, Settings.ResultDecimalPlaces);
    }
}