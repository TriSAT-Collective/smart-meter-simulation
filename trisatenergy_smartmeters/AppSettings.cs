using System.ComponentModel;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters;

public class AppSettings
{
    public RabbitMqSettings RabbitMq { get; init; }
    public MiscSettings Misc { get; init; }
    public EnergySourcesSettings EnergySources { get; init; }

    public class RabbitMqSettings
    {
        public Uri Uri { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKeyBase { get; set; }
    }

    public class MiscSettings
    {
        public bool MaintenanceMode { get; set; }
        public bool ContinuousSimulation { get; set; }

        [DefaultValue(3000)] public int ContinuousSimulationIntervalMs { get; set; }

        [DefaultValue(24)] public int OnceOffSimulationHours { get; set; }

        public DateTime? SimulationStartTime { get; set; }
    }

    public class EnergySourcesSettings
    {
        public EnergySourceType[] EnabledSources { get; set; }
        public double SolarEnergyProduction { get; set; }
        public double WindEnergyProduction { get; set; }
        public double OtherEnergyProduction { get; set; }
        public int ResultDecimalPlaces { get; set; }
    }
}