using System.Net.Http.Json;

namespace CanAmWeatherApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ZoneResponse?> GetForecastZonesByStateAsync(string state)
    {
        state = state.Trim().ToUpper();

        return await _httpClient.GetFromJsonAsync<ZoneResponse>(
            $"zones/forecast?area={state}");
    }

    public async Task<StationResponse?> GetStationsByZoneAsync(string zoneId)
    {
        return await _httpClient.GetFromJsonAsync<StationResponse>(
            $"zones/forecast/{zoneId}/stations");
    }

    public async Task<PointResponse?> GetPointDataAsync(double latitude, double longitude)
    {
        return await _httpClient.GetFromJsonAsync<PointResponse>(
            $"points/{latitude},{longitude}");
    }

    public async Task<ForecastResponse?> GetForecastAsync(string forecastUrl)
    {
        return await _httpClient.GetFromJsonAsync<ForecastResponse>(forecastUrl);
    }

}

//testing models TODO: move to models folder if successful

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

// Station response modelS
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