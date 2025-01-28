using System.ComponentModel;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace trisatenergy_smartmeters;
/// <summary>
/// Represents the application settings.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the RabbitMQ settings.
    /// </summary>
    public RabbitMqSettings RabbitMq { get; init; }
    /// <summary>
    /// Gets or sets the miscellaneous settings.
    /// </summary>
    public MiscSettings Misc { get; init; }
    /// <summary>
    /// Gets or sets the energy sources settings.
    /// </summary>
    public EnergySourcesSettings EnergySources { get; init; }
    /// <summary>
    /// Represents the RabbitMQ settings.
    /// </summary>
    public class RabbitMqSettings
    {
        /// <summary>
        /// Gets or sets the URI of the RabbitMQ server.
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// Gets or sets the virtual host of the RabbitMQ server.
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// Gets or sets the username for RabbitMQ authentication.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password for RabbitMQ authentication.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the name of the RabbitMQ exchange.
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// Gets or sets the base routing key for RabbitMQ messages.
        /// </summary>
        public string RoutingKeyBase { get; set; }
    }
    /// <summary>
    /// Represents miscellaneous settings.
    /// </summary>
    public class MiscSettings
    {
        /// <summary>
        /// Gets or sets the unique identifier for the smart meter.
        /// </summary>
        public Guid SmartMeterId { get; set; } = Guid.NewGuid();

        public bool MaintenanceMode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the simulation runs continuously.
        /// </summary>
        public bool ContinuousSimulation { get; set; }
        /// <summary>
        /// Gets or sets the interval in milliseconds for continuous simulation.
        /// </summary>
        [DefaultValue(3000)] public int ContinuousSimulationIntervalMs { get; set; }
        /// <summary>
        /// Gets or sets the number of hours for once-off simulation.
        /// </summary>
        [DefaultValue(24)] public int OnceOffSimulationHours { get; set; }
        /// <summary>
        /// Gets or sets the start time for the simulation.
        /// </summary>
        public DateTime? SimulationStartTime { get; set; }
    }
    /// <summary>
    /// Represents the settings for energy sources.
    /// </summary>

    public class EnergySourcesSettings
    {
        /// <summary>
        /// Gets or sets the enabled energy sources.
        /// </summary>
        public EnergySourceType[] EnabledSources { get; set; }
        /// <summary>
        /// Gets or sets the solar energy production value.
        /// </summary>
        public double SolarEnergyProduction { get; set; }
        /// <summary>
        /// Gets or sets the wind energy production value.
        /// </summary>
        public double WindEnergyProduction { get; set; }
        /// <summary>
        /// Gets or sets the other energy production value.
        /// </summary>
        public double OtherEnergyProduction { get; set; }
        /// <summary>
        /// Gets or sets the number of decimal places for the result.
        /// </summary>
        public int ResultDecimalPlaces { get; set; }
    }
}