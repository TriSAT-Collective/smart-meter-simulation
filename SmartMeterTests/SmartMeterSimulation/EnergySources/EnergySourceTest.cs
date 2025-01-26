using Microsoft.Extensions.Logging;
using Moq;
using trisatenergy_smartmeters;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace SmartMeterTests.SmartMeterSimulation.EnergySources;

public class EnergySourceTest
{
    private readonly OtherEnergy _otherEnergy;
    private readonly AppSettings.EnergySourcesSettings _settings;
    private readonly SolarEnergy _solarEnergy;
    private readonly WindEnergy _windEnergy;

    public EnergySourceTest()
    {
        Mock<ILogger<WindEnergy>> mockWindLogger = new();
        Mock<ILogger<SolarEnergy>> mockSolarLogger = new();
        Mock<ILogger<OtherEnergy>> mockOtherLogger = new();
        _settings = new AppSettings.EnergySourcesSettings
        {
            WindEnergyProduction = 7.5,
            SolarEnergyProduction = 5.0,
            OtherEnergyProduction = 3.0
        };
        _windEnergy = new WindEnergy(mockWindLogger.Object, _settings);
        _solarEnergy = new SolarEnergy(mockSolarLogger.Object, _settings);
        _otherEnergy = new OtherEnergy(mockOtherLogger.Object, _settings);
    }

    [Fact]
    public void WindEnergy_SimulateProduction_ReturnsPositiveValue()
    {
        // Arrange
        var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
        var rand = new Random();

        // Act
        var production = _windEnergy.SimulateProduction(timeStamp, rand);

        // Assert
        Assert.True(production >= 0, "Wind energy production should be non-negative.");
    }

    [Fact]
    public void SolarEnergy_SimulateProduction_ReturnsPositiveValue()
    {
        // Arrange
        var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
        var rand = new Random();

        // Act
        var production = _solarEnergy.SimulateProduction(timeStamp, rand);

        // Assert
        Assert.True(production >= 0, "Solar energy production should be non-negative.");
    }

    [Fact]
    public void OtherEnergy_SimulateProduction_ReturnsPositiveValue()
    {
        // Arrange
        var timeStamp = new DateTime(2023, 1, 1, 12, 0, 0);
        var rand = new Random();

        // Act
        var production = _otherEnergy.SimulateProduction(timeStamp, rand);

        // Assert
        Assert.True(production >= 0, "Other energy production should be non-negative.");
    }
}