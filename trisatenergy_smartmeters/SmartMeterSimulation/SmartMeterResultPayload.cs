using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters.SmartMeterSimulation;
/// <summary>
/// Represents the payload of the smart meter result.
/// </summary>
public class SmartMeterResultPayload
{
<<<<<<< HEAD
    /// <summary>
    /// Gets or sets the unique identifier of the smart meter.
    /// </summary>
    public Guid SmartMeterId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp of the result.
    /// </summary>
=======
    public Guid Id { get; set; }
>>>>>>> f872b13a524e4089330b494258d17b1b6ac08da9
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Gets or sets the total energy production.
    /// </summary>
    public double TotalProduction { get; set; }
    /// <summary>
    /// Gets or sets the energy production by source.
    /// </summary>
    public Dictionary<EnergySourceType, double> ProductionBySource { get; set; }
    /// <summary>
    /// Gets or sets the total energy consumption.
    /// </summary>
    public double TotalConsumption { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the maintenance mode is enabled.
    /// </summary>
    public bool MaintenanceMode { get; set; }
}