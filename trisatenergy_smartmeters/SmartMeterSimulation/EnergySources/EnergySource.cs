using Microsoft.Extensions.Logging;

namespace trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

/// <summary>
///     Abstract base class for different energy sources.
///     All energy sources should implement the SimulateProduction method.
/// </summary>
public abstract class EnergySource
{
    /// <summary>
    /// The logger instance for logging information.
    /// </summary>
    protected readonly ILogger Logger;
    /// <summary>
    /// The settings for the energy sources.
    /// </summary>
    protected readonly AppSettings.EnergySourcesSettings Settings;
    /// <summary>
    /// Initializes a new instance of the <see cref="EnergySource"/> class with the specified logger and settings.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="settings">The energy sources settings.</param>
    protected EnergySource(ILogger logger, AppSettings.EnergySourcesSettings settings)
    {
        Logger = logger;
        Settings = settings;
    }

    /// <summary>
    /// Gets the type of the energy source.
    /// </summary>
    public virtual EnergySourceType SourceType { get; }

    /// <summary>
    ///     Simulates the production of energy for a given hour.
    /// </summary>
    /// <param name="timeStamp">THe timestamp</param>
    /// <param name="rand">A Random object for generating fluctuations.</param>
    /// <returns>Amount of energy produced in kWh.</returns>
    public abstract double SimulateProduction(DateTime timeStamp, Random rand);
}
/// <summary>
/// Enumeration of different types of energy sources.
/// </summary>
public enum EnergySourceType
{
    /// <summary>
    /// Solar energy source.
    /// </summary>
    Solar,
    /// <summary>
    /// Wind energy source.
    /// </summary>
    Wind,
    /// <summary>
    /// Other types of energy sources.
    /// </summary>
    Other
}