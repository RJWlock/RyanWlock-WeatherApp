using System.Net.Http.Json;
using CanAmWeatherApp.Models;

namespace CanAmWeatherApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Generic API handler (centralized error handling)
    private async Task<T> GetFromApiAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"NWS API request failed: {(int)response.StatusCode} {response.ReasonPhrase}");
        }

        var data = await response.Content.ReadFromJsonAsync<T>();

        if (data == null)
        {
            throw new Exception("NWS API returned empty or invalid data.");
        }

        return data;
    }

    public async Task<ZoneResponse> GetForecastZonesByStateAsync(string state)
    {
        state = state.Trim().ToUpper();

        return await GetFromApiAsync<ZoneResponse>(
            $"zones/forecast?area={state}");
    }

    public async Task<StationResponse> GetStationsByZoneAsync(string zoneId)
    {
        return await GetFromApiAsync<StationResponse>(
            $"zones/forecast/{zoneId}/stations");
    }

    public async Task<PointResponse> GetPointDataAsync(double latitude, double longitude)
    {
        return await GetFromApiAsync<PointResponse>(
            $"points/{latitude},{longitude}");
    }

    public async Task<ForecastResponse> GetForecastAsync(string forecastUrl)
    {
        return await GetFromApiAsync<ForecastResponse>(forecastUrl);
    }

    public async Task<ForecastResponse> GetHourlyForecastAsync(string hourlyForecastUrl)
    {
        return await GetFromApiAsync<ForecastResponse>(hourlyForecastUrl);
    }
}