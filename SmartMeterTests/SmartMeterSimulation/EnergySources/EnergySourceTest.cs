using Microsoft.Extensions.Logging;
using Moq;
using trisatenergy_smartmeters;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace SmartMeterTests.SmartMeterSimulation.EnergySources;

public class EnergySourceTest
{
    private readonly Mock<ILogger<OtherEnergy>> _mockOtherLogger;
    private readonly Mock<ILogger<SolarEnergy>> _mockSolarLogger;
    private readonly Mock<ILogger<WindEnergy>> _mockWindLogger;
    private readonly OtherEnergy _otherEnergy;
    private readonly AppSettings.EnergySourcesSettings _settings;
    private readonly SolarEnergy _solarEnergy;
    private readonly WindEnergy _windEnergy;

    public EnergySourceTest()
    {
        _mockWindLogger = new Mock<ILogger<WindEnergy>>();
        _mockSolarLogger = new Mock<ILogger<SolarEnergy>>();
        _mockOtherLogger = new Mock<ILogger<OtherEnergy>>();
        _settings = new AppSettings.EnergySourcesSettings
        {
            WindEnergyProduction = 7.5,
            SolarEnergyProduction = 5.0,
            OtherEnergyProduction = 3.0
        };
        _windEnergy = new WindEnergy(_mockWindLogger.Object, _settings);
        _solarEnergy = new SolarEnergy(_mockSolarLogger.Object, _settings);
        _otherEnergy = new OtherEnergy(_mockOtherLogger.Object, _settings);
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