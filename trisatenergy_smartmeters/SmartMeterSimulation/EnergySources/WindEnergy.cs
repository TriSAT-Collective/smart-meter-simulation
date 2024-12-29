using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates wind energy production, which varies randomly throughout the day.
///     Wind production can peak at any time, but there's randomness throughout.
/// </summary>
public class WindEnergy : EnergySource
{
    public WindEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }

    public override EnergySourceType SourceType => EnergySourceType.Wind;

    /// <inheritdoc />
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        Logger.LogTrace("Simulating wind energy production for {TimeStamp}", timeStamp);
        var hour = timeStamp.Hour;
        return Math.Round(rand.NextDouble() * 3.0, Settings.ResultDecimalPlaces);
    }
}