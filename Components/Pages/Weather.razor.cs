using CanAmWeatherApp.Models;
using CanAmWeatherApp.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace CanAmWeatherApp.Components.Pages;

public partial class Weather
{
    [Inject] private WeatherService WeatherService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

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

        if (string.IsNullOrWhiteSpace(stateInput) || stateInput.Trim().Length != 2)
        {
            zonesError = "Please enter a valid 2-letter state code, such as CO, TX, or CA.";
            return;
        }

        isLoadingZones = true;

        try
        {
            var result = await WeatherService.GetForecastZonesByStateAsync(stateInput.Trim().ToUpper());
            zones = result?.Features;

            if (zones == null || zones.Count == 0)
            {
                zonesEmpty = $"No forecast zones found for '{stateInput}'.";
            }
        }
        catch (Exception ex)
        {
            zonesError = $"Could not load zones. {ex.Message}";
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
            stationsError = $"Failed to load stations. {ex.Message}";
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