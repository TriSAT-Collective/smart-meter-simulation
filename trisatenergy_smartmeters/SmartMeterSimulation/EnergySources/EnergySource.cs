using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Abstract base class for different energy sources.
///     All energy sources should implement the SimulateProduction method.
/// </summary>
public abstract class EnergySource
{
    protected readonly ILogger Logger;
    protected readonly AppSettings.EnergySourcesSettings Settings;

    protected EnergySource(ILogger logger, AppSettings.EnergySourcesSettings settings)
    {
        Logger = logger;
        Settings = settings;
    }


    public virtual EnergySourceType SourceType { get; }

    /// <summary>
    ///     Simulates the production of energy for a given hour.
    /// </summary>
    /// <param name="timeStamp">THe timestamp</param>
    /// <param name="rand">A Random object for generating fluctuations.</param>
    /// <returns>Amount of energy produced in kWh.</returns>
    public abstract double SimulateProduction(DateTime timeStamp, Random rand);
}

public enum EnergySourceType
{
    Solar,
    Wind,
    Other
}