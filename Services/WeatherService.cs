using System.Net.Http.Json;

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
}


// ==========================
// MODELS
// ==========================

public class ZoneResponse
{
    public List<ZoneFeature> Features { get; set; } = [];
}

public class ZoneFeature
{
    public ZoneProperties Properties { get; set; } = new();
}

public class ZoneProperties
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

// Station models
public class StationResponse
{
    public List<StationFeature> Features { get; set; } = [];
}

public class StationFeature
{
    public StationProperties Properties { get; set; } = new();
    public StationGeometry Geometry { get; set; } = new();
}

public class StationProperties
{
    public string StationIdentifier { get; set; } = "";
    public string Name { get; set; } = "";
}

public class StationGeometry
{
    public List<double> Coordinates { get; set; } = [];
}

// Forecast models
public class PointResponse
{
    public PointProperties Properties { get; set; } = new();
}

public class PointProperties
{
    public string Forecast { get; set; } = "";
    public string ForecastHourly { get; set; } = "";
}

public class ForecastResponse
{
    public ForecastProperties Properties { get; set; } = new();
}

public class ForecastProperties
{
    public List<ForecastPeriod> Periods { get; set; } = [];
}

public class ForecastPeriod
{
    public string Name { get; set; } = "";
    public int Temperature { get; set; }
    public string TemperatureUnit { get; set; } = "";
    public string ShortForecast { get; set; } = "";
    public string DetailedForecast { get; set; } = "";
}