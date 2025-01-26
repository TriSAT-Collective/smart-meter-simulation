using Microsoft.Extensions.Logging;
using Moq;
using trisatenergy_smartmeters;
using trisatenergy_smartmeters.SmartMeterSimulation.EnergySources;

namespace SmartMeterTests.SmartMeterSimulation.EnergySources;
/// <summary>
/// Unit tests for the energy sources in the smart meter simulation.
/// </summary>
public class EnergySourceTest
{
    private readonly OtherEnergy _otherEnergy;
    private readonly SolarEnergy _solarEnergy;
    private readonly WindEnergy _windEnergy;
    /// <summary>
    /// Initializes a new instance of the <see cref="EnergySourceTest"/> class.
    /// </summary>
    public EnergySourceTest()
    {
        Mock<ILogger<WindEnergy>> mockWindLogger = new();
        Mock<ILogger<SolarEnergy>> mockSolarLogger = new();
        Mock<ILogger<OtherEnergy>> mockOtherLogger = new();
        var settings = new AppSettings.EnergySourcesSettings
        {
            WindEnergyProduction = 7.5,
            SolarEnergyProduction = 5.0,
            OtherEnergyProduction = 3.0
        };
        _windEnergy = new WindEnergy(mockWindLogger.Object, settings);
        _solarEnergy = new SolarEnergy(mockSolarLogger.Object, settings);
        _otherEnergy = new OtherEnergy(mockOtherLogger.Object, settings);
    }
    /// <summary>
    /// Tests that the wind energy production simulation returns a non-negative value.
    /// </summary>
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
    /// <summary>
    /// Tests that the solar energy production simulation returns a non-negative value.
    /// </summary>
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
    /// <summary>
    /// Tests that the other energy production simulation returns a non-negative value.
    /// </summary>
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