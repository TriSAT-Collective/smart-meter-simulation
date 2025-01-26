<h1 align="center">smart-meter-simulation</h1>

<p align="center">
  <a href="#about">About</a> •
  <a href="#features">Features</a> •
  <a href="#dependencies">Dependencies</a> •
  <a href="#building">Building</a> •
  <a href="#installation">Installation</a> •
  <a href="#usage">Usage</a> •
  <a href="#configuration">Configuration</a>
</p>

---

## About

The `smart-meter-simulation` project simulates energy production from various sources such as wind, solar, and other renewable sources. It is designed to help in understanding and analyzing the behavior of different energy sources in a smart meter environment.

## Features

- Simulates energy production from wind, solar, and other sources.
- Provides realistic variability in energy production.
- Configurable settings for different energy sources.
- Logging support for detailed simulation analysis.

## Building

To build the project, use the following command:

```bash
dotnet build
```

## Installation

To install the project, clone the repository and navigate to the project directory:
```bash
git clone https://github.com/yourusername/smart-meter-simulation.git
cd smart-meter-simulation
```

## Usage

To run the simulation, use the following command:
```bash
dotnet run
```

## Configuration

The project can be configured using the appsettings.json file. Below is an example configuration:
```JSON
{
  "EnergySourcesSettings": {
    "WindEnergyProduction": 7.5,
    "SolarEnergyProduction": 5.0,
    "OtherEnergyProduction": 3.0,
    "ResultDecimalPlaces": 2
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

