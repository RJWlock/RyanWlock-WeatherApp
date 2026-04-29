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