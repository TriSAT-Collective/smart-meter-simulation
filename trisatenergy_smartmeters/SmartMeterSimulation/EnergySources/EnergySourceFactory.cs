using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

public static class EnergySourceFactory
{
    public static EnergySource CreateEnergySource(EnergySourceType type, ILogger logger,
        AppSettings.EnergySourcesSettings settings)
    {
        return type switch
        {
            EnergySourceType.Solar => new SolarEnergy(logger, settings),
            EnergySourceType.Wind => new WindEnergy(logger, settings),
            EnergySourceType.Other => new OtherEnergy(logger, settings),
            _ => throw new ArgumentException($"Unknown energy source type: {type}")
        };
    }
}