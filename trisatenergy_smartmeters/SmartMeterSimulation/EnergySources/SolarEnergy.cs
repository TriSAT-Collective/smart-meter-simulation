using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates solar energy production based on daytime hours.
///     Production peaks between 6:00 and 18:00.
/// </summary>
public class SolarEnergy : EnergySource
{
    public SolarEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }

    public override EnergySourceType SourceType => EnergySourceType.Solar;

    /// <inheritdoc />
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        Logger.LogTrace("Simulating solar energy production for {TimeStamp}", timeStamp);
        // Solar production is based on daylight hours
        var hour = timeStamp.Hour;
        // Base solar production: more during daylight hours (6 AM - 6 PM)
        var baseProduction = hour >= 6 && hour <= 18 ? 5.0 : 0.0;
        // Random fluctuation for realistic variability
        var fluctuation = rand.NextDouble() * 1.0;
        return baseProduction + fluctuation;
    }
}