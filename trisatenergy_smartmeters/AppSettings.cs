namespace trisatenergy_smartmeters;

public class AppSettings
{
    public RabbitMQSettings RabbitMQ { get; set; }
    public MiscSettings Misc { get; set; }
    public EnergySourcesSettings EnergySources { get; set; }
    
    public class RabbitMQSettings
    {
        public Uri Uri { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class MiscSettings
    {
        public bool MaintenanceMode { get; set; }
    }
    
    public class EnergySourcesSettings
    {
        public double SolarEnergyProduction { get; set; }
        public double WindEnergyProduction { get; set; }
        public double OtherEnergyProduction { get; set; }
    }
    
}