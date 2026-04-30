# CanAm Weather App

## Overview

This is a Blazor Server application built with MudBlazor that integrates with the National Weather Service (NWS) API to retrieve and present forecast data through an interactive weather dashboard.

The app allows users to:

* Input a U.S. state (e.g., CO)
* Retrieve local weather entities (zones or stations)
* View detailed forecast data (hourly or daily)

---

## Tech Stack

* **C# / .NET (Blazor)**
* **MudBlazor** (UI components)
* **National Weather Service API**
  https://www.weather.gov/documentation/services-web-api

---

## Features

* Search forecast zones by U.S. state
* Select weather stations within a forecast zone
* View daily forecast with high / low temperatures
* View 24-hour forecast for the selected day
* Interactive day selection dashboard
* Responsive top-header navigation
* Loading, validation, and error handling
* Component-based styling using Razor CSS isolation

---

## App Flow

1. User opens the app and clicks **Weather** in the navigation header.
2. User enters a U.S. state abbreviation, such as `CO`, `TX`, or `CA`.
3. App retrieves forecast zones for that state.
4. User selects a forecast zone.
5. App retrieves stations inside the selected zone.
6. User selects a station.
7. App extracts the station coordinates.
8. App calls the NWS `/points/{latitude},{longitude}` endpoint.
9. App uses the returned forecast URLs to retrieve:
   - Daily / period forecast
   - Hourly forecast
10. App displays the forecast dashboard.

---

## Project Structure

* `Components/Pages/` – Razor pages, code-behind files, and isolated CSS
* `Models/` – NWS API DTOs
* `Services/` – API integration logic (`WeatherService`)
* `Program.cs` – Application startup and dependency injection

---

## Getting Started

### Prerequisites

* .NET 10 SDK (or compatible)
* Internet connection (for NWS API access)

### Run the App

```bash
dotnet run
```

Then navigate to:

```
https://localhost:<port>
```

---

## API Notes

This project uses the public NWS API. No API key is required, but requests should include a User-Agent header per NWS guidelines.

Example endpoints:

* `/zones?area={state}`
* `/gridpoints/{office}/{gridX},{gridY}/forecast`


---

## Future Improvements

* Better entity filtering (zones vs stations)
* Caching API responses
* Unit tests for service layer
* More Options and stats on weather

---

## Development Plan & Progress Notes

### 1. Planning & Research (Time - 30min)
- [x] Review project requirements
- [x] Decide on app flow (Search: state -> Return: zones/stations -> Select/Display: forecast)
- [x] Outline basic architecture (Blazor UI + WeatherService)

### 1.5 Project Setup (Time - 30min)
- [x] Create Blazor app scaffold
- [x] Add MudBlazor
- [x] Create `.gitignore` / `README.md`
- [x] Initialize Git repository
- [x] Make initial commit

---

### 2 API Testing and Planning (Time - 120min)
- [x] Explore National Weather Service API documentation
- [x] Identify relevant endpoints:
  - `/zones/forecast?area={state}`
  - `/zones/forecast/{zoneId}/stations`
  - `/points/{latitude},{longitude}`
  - `/gridpoints/{office}/{gridX},{gridY}/forecast`
  - `/gridpoints/{office}/{gridX},{gridY}/forecast/hourly`
- [x] Create API flow:
  1. User enters a state abbreviation, e.g. `AL`
  2. `GET /zones/forecast?area=AL`
  3. User selects a forecast zone, e.g. `ALZ001 / Lauderdale`
  4. `GET /zones/forecast/ALZ001/stations`
  5. User selects a weather station
  6. Extract station coordinates from `geometry.coordinates`
     - API returns `[longitude, latitude]`
     - `/points` requires `{latitude},{longitude}`
  7. `GET /points/{latitude},{longitude}`
  8. Read forecast URLs:
     - `properties.forecast`
     - `properties.forecastHourly`
  9. Fetch and display:
     - Daily / 12-hour forecast
     - Hourly forecast

---

### 3. API Integration (Time - 30min)
- [x] Create `Services/WeatherService.cs`
- [x] Register `HttpClient` in `Program.cs`
- [x] Add required NWS `User-Agent` header 
- [x] Create DTO/model classes for NWS responses
- [x] Fetch zones/stations by state
- [x] Fetch forecast data for selected entity
- [x] Handle API errors and edge cases

---

### 4. Core App Workflow (Time - 30min)
- [x] Add state input field
- [x] Trigger API call on search
- [x] Display zones list
- [x] Handle selection of zone
- [x] Load stations dynamically under selected zone

---

### 5. Forecast Display (Time - 20min)
- [x] Show forecast period (name/time)
- [x] Display temperature and units
- [x] Display short forecast
- [x] Display detailed forecast
- [x] Implement daily view


---

### 6. UI Polish (Time - 30min)
- [x] Add loading indicators
- [x] Add input validation
- [x] Add error handling UI
- [x] Add empty-state messaging
- [x] Improve layout with MudBlazor components

---

### 7. Manual Testing & Validation (Time - 10min)
- [x] Test with multiple states (CO, TX, CA)
- [x] Test invalid inputs
- [x] Verify API failure handling
- [x] Confirm forecast renders correctly

### 7.5 UI Polish / Weather Dashboard (Time - 90min)
- [x] Replace side navigation with top header navigation
- [x] Display forecast zone name instead of station ID
- [x] Add low temperature beside current temperature
- [x] Add hourly mini-card layout with Show More / Show Less
- [x] Refactor pages into partial classes with CSS isolation

---

### 8. Final Cleanup (Time - 90min)
- [x] Refactor pages into partial classes (`.razor` + `.razor.cs`)
- [x] Add Razor CSS isolation (`.razor.css`)
- [x] Refactor code for readability and maintainability
- [x] Clean up service injection naming and class dependencies
- [x] Replace sidebar navigation with responsive top-header navigation
- [x] Replace station identifiers with forecast zone names in dashboard UI
- [x] Add current low temperature beside main temperature
- [x] Replace hourly chart with 24-hour mini-card layout
- [x] Add Show More / Show Less behavior for hourly forecast
- [x] Improve table performance with paging and dense rendering
- [x] Fix UI bugs, Razor parsing issues, and responsive layout edge cases
- [x] Clean up unused files and scaffolded components
- [x] Manually test final version across desktop and mobile layouts

### 8.2 Final Cleanup (Time - 60min)
- [x] Found bug that was allowing the user to search Territories containing 50 states and returning no stations. Added validation to prevent this and display an error message instead.
- [x] Fixed bug night time icon showing day emoji

### TOTAL TIME SPENT - 9 hours (540min)
