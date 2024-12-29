using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Simulates small production from various other sources like geothermal or battery.
///     This is typically consistent with minor fluctuations.
/// </summary>
public class OtherEnergy : EnergySource
{
    public OtherEnergy(ILogger logger, AppSettings.EnergySourcesSettings settings) : base(logger, settings)
    {
    }

    public override EnergySourceType SourceType => EnergySourceType.Other;

    /// <inheritdoc />
    public override double SimulateProduction(DateTime timeStamp, Random rand)
    {
        var baseProduction = 1.0;
        var hour = timeStamp.Hour;
        var fluctuation = rand.NextDouble() * 0.2;
        return baseProduction + fluctuation;
    }
}