using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters.SmartMeterSimulation;
/// <summary>
/// Represents the payload of the smart meter result.
/// </summary>
public class SmartMeterResultPayload
{
    /// <summary>
    /// Gets or sets the unique identifier of the smart meter.
    /// </summary>
    public Guid Id { get; set; }


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