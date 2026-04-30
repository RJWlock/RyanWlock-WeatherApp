using CanAmWeatherApp.Models;
using CanAmWeatherApp.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace CanAmWeatherApp.Components.Pages;

public partial class Forecast
{
    [Inject] private WeatherService WeatherApi { get; set; } = default!;

    [Parameter] public string? Latitude { get; set; }
    [Parameter] public string? Longitude { get; set; }
    [Parameter] public string? StationId { get; set; }

    private List<ForecastPeriod>? forecastPeriods;
    private List<ForecastPeriod>? hourlyForecastPeriods;
    private ForecastPeriod? selectedPeriod;

    private bool showAllHourly;
    private bool isLoadingForecast;
    private string? forecastError;
    private string? forecastEmpty;

    protected override async Task OnInitializedAsync()
    {
        await LoadForecast();
    }

    private async Task LoadForecast()
    {
        if (!double.TryParse(Latitude, CultureInfo.InvariantCulture, out var latitude) ||
            !double.TryParse(Longitude, CultureInfo.InvariantCulture, out var longitude))
        {
            forecastError = "Invalid station coordinates.";
            return;
        }

        isLoadingForecast = true;

        try
        {
            var pointData = await WeatherApi.GetPointDataAsync(latitude, longitude);

            var forecast = await WeatherApi.GetForecastAsync(
                pointData.Properties.Forecast);

            forecastPeriods = forecast.Properties.Periods;
            selectedPeriod = forecastPeriods.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(
                pointData.Properties.ForecastHourly))
            {
                var hourly = await WeatherApi.GetHourlyForecastAsync(
                    pointData.Properties.ForecastHourly);

                hourlyForecastPeriods = hourly.Properties.Periods;
            }

            if (forecastPeriods.Count == 0)
            {
                forecastEmpty = "No forecast periods were returned.";
            }
        }
        catch (Exception ex)
        {
            forecastError = $"Failed to load forecast. {ex.Message}";
        }
        finally
        {
            isLoadingForecast = false;
        }
    }

    private void SelectPeriod(ForecastPeriod period)
    {
        selectedPeriod = period;
        showAllHourly = false;
    }

    private void ToggleHourlyCards()
    {
        showAllHourly = !showAllHourly;
    }

    private IEnumerable<DailyCard> GetDailyCards()
    {
        if (forecastPeriods is null)
            yield break;

        for (var i = 0; i < forecastPeriods.Count; i++)
        {
            var period = forecastPeriods[i];

            if (period.Name.Contains(
                "night",
                StringComparison.OrdinalIgnoreCase))
                continue;

            int? low = null;

            var nextPeriod =
                forecastPeriods.ElementAtOrDefault(i + 1);

            if (nextPeriod is not null &&
                nextPeriod.Name.Contains(
                    "night",
                    StringComparison.OrdinalIgnoreCase))
            {
                low = nextPeriod.Temperature;
            }

            yield return new DailyCard(period, low);
        }
    }

    private IEnumerable<ForecastPeriod>
        GetSelectedDayHourlyPeriods()
    {
        if (hourlyForecastPeriods is null ||
            selectedPeriod is null)
            return [];

        var selectedDate = selectedPeriod.StartTime.Date;

        return hourlyForecastPeriods
            .Where(x => x.StartTime.Date == selectedDate)
            .Take(24);
    }

    private string GetDailyCardClass(
        ForecastPeriod period)
    {
        var selectedClass =
            selectedPeriod == period
                ? " daily-card-selected"
                : "";

        return $"daily-card{selectedClass}";
    }

    private string GetDayLabel(
        ForecastPeriod period)
    {
        if (period.StartTime != default)
        {
            return period.StartTime.ToString(
                "ddd",
                CultureInfo.InvariantCulture);
        }

        return period.Name;
    }

    private int? GetLowForCurrentPeriod()
    {
        if (forecastPeriods is null ||
            selectedPeriod is null)
            return null;

        var index =
            forecastPeriods.IndexOf(selectedPeriod);

        if (index < 0)
            return null;

        var next =
            forecastPeriods.ElementAtOrDefault(
                index + 1);

        if (next is not null &&
            next.Name.Contains(
                "night",
                StringComparison.OrdinalIgnoreCase))
        {
            return next.Temperature;
        }

        return null;
    }

    private string GetWeatherIcon(ForecastPeriod period)
    {
        var forecast =
            period.ShortForecast.ToLowerInvariant();

        if (forecast.Contains("sun") ||
            forecast.Contains("clear"))
        {
            return period.IsDaytime
                ? "☀️"
                : "🌙";
        }

        if (forecast.Contains("storm"))
            return "⛈️";

        if (forecast.Contains("rain"))
            return "🌧️";

        if (forecast.Contains("snow"))
            return "❄️";

        if (forecast.Contains("fog"))
            return "🌫️";

        if (forecast.Contains("cloud"))
        {
            return period.IsDaytime
                ? "☁️"
                : "🌙☁️";
        }

        if (forecast.Contains("sun") ||
            forecast.Contains("clear"))
        {
            return period.IsDaytime
                ? "☀️"
                : "🌙";
        }

        return period.IsDaytime
            ? "🌤️"
            : "🌙";
    }

    private record DailyCard(
        ForecastPeriod Period,
        int? LowTemperature);
}