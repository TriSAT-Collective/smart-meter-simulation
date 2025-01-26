using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters.SmartMeterSimulation;

public class SmartMeterResultPayload
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double TotalProduction { get; set; }
    public Dictionary<EnergySourceType, double> ProductionBySource { get; set; }
    public double TotalConsumption { get; set; }
    public bool MaintenanceMode { get; set; }
}