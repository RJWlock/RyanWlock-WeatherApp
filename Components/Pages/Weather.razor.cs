using CanAmWeatherApp.Models;
using CanAmWeatherApp.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace CanAmWeatherApp.Components.Pages;

public partial class Weather
{
    [Inject] private WeatherService WeatherService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILogger<Weather> Logger { get; set; } = default!;

    private string stateInput = "";

    private List<ZoneFeature>? zones;
    private List<StationFeature>? stations;

    private string? selectedZoneId;
    private ZoneFeature? selectedZone;

    private bool isLoadingZones;
    private bool isLoadingStations;

    private string? zonesError;
    private string? zonesEmpty;

    private string? stationsError;
    private string? stationsEmpty;

    private async Task SearchZones()
    {
        zonesError = null;
        zonesEmpty = null;
        stationsError = null;
        stationsEmpty = null;

        zones = null;
        stations = null;
        selectedZoneId = null;
        selectedZone = null;

        var stateCode = stateInput.Trim().ToUpperInvariant();

        if (stateCode.Length != 2 || !stateCode.All(char.IsLetter))
        {
            zonesError = "Please enter a valid 2-letter U.S. state code, such as CO, TX, or CA.";
            return;
        }

        isLoadingZones = true;

        try
        {
            var result = await WeatherService.GetForecastZonesByStateAsync(stateCode);
            zones = result?.Features;

            if (zones == null || zones.Count == 0)
            {
                zonesEmpty = $"No forecast zones found for '{stateCode}'.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load forecast zones for state input {StateInput}", stateInput);

            zonesError = "We could not load forecast zones for that state. Please check the state abbreviation and try again.";
        }
        finally
        {
            isLoadingZones = false;
        }
    }

    private async Task SelectZone(ZoneFeature zone)
    {
        selectedZone = zone;
        selectedZoneId = zone.Properties.Id;

        stationsError = null;
        stationsEmpty = null;
        stations = null;

        isLoadingStations = true;

        try
        {
            var result = await WeatherService.GetStationsByZoneAsync(selectedZoneId);
            stations = result?.Features;

            if (stations == null || stations.Count == 0)
            {
                stationsEmpty = "No stations found for this zone.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load stations for zone {ZoneId}", selectedZoneId);

            stationsError = "We could not load stations for this zone. Please try another zone or search again.";
        }
        finally
        {
            isLoadingStations = false;
        }
    }

    private void GoToForecastPage(StationFeature station)
    {
        var longitude = station.Geometry.Coordinates[0].ToString(CultureInfo.InvariantCulture);
        var latitude = station.Geometry.Coordinates[1].ToString(CultureInfo.InvariantCulture);

        var zoneName = Uri.EscapeDataString(
            selectedZone?.Properties.Name ?? selectedZoneId ?? "Selected Zone");

        NavigationManager.NavigateTo($"/forecast/{latitude}/{longitude}/{zoneName}");
    }
}