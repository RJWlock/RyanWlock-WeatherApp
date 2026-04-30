namespace CanAmWeatherApp.Models
{

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
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int Temperature { get; set; }
        public string TemperatureUnit { get; set; } = "";
        public string WindSpeed { get; set; } = "";
        public string WindDirection { get; set; } = "";
        public string ShortForecast { get; set; } = "";
        public string DetailedForecast { get; set; } = "";
        public bool IsDaytime { get; set; }
    }

}
