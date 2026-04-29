# CanAm Weather App

## Overview

This is a simple Blazor (MudBlazor) application that integrates with the National Weather Service (NWS) API to retrieve and display weather forecast data.

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

## Features (Planned / In Progress)

* [ ] Country_State lookup of weather zones/stations
* [ ] Selection of a specific zone or station
* [ ] Display of forecast data (temperature highs/lows, hourly/daily)
* [ ] Clean, responsive UI using MudBlazor

---

## Project Structure

* `Components/` – Blazor UI components and pages
* `Services/` – API integration logic (WeatherService)
* `Program.cs` – Application entry point and service registration

---

## Getting Started

### Prerequisites

* .NET 8 SDK (or compatible)
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

* Error handling and loading states
* Better entity filtering (zones vs stations)
* Caching API responses
* Unit tests for service layer

---

## Development Plan & Progress

### 1. Planning & Research (Time - 0.5 hour)
- [x] Review project requirements
- [x] Decide on app flow (Search: state -> Return: zones/stations -> Select/Display: forecast)
- [x] Outline basic architecture (Blazor UI + WeatherService)

### 1.5 Project Setup (Time - 0.5 hour)
- [x] Create Blazor app scaffold
- [x] Add MudBlazor
- [x] Create `.gitignore` / `README.md`
- [x] Initialize Git repository
- [x] Make initial commit

---

### 2 API Testing and Planning (Time - 2 hours)
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

### 3. API Integration (Time - 30)
- [x] Create `Services/WeatherService.cs`
- [x] Register `HttpClient` in `Program.cs`
- [x] Add required NWS `User-Agent` header 
- [x] Create DTO/model classes for NWS responses
- [x] Fetch zones/stations by state
- [x] Fetch forecast data for selected entity
- [x] Handle API errors and edge cases

---

### 4. Core App Workflow (Time - 30)
- [x] Add state input field
- [x] Trigger API call on search
- [x] Display zones list
- [x] Handle selection of zone
- [x] Load stations dynamically under selected zone

---

### 5. Forecast Display (Time - 20)
- [x] Show forecast period (name/time)
- [x] Display temperature and units
- [x] Display short forecast
- [x] Display detailed forecast
- [x] Implement daily view


---

### 6. UI Polish (Time - 30)
- [x] Add loading indicators
- [x] Add input validation
- [x] Add error handling UI
- [x] Add empty-state messaging
- [x] Improve layout with MudBlazor components

---

### 7. Manual Testing & Validation (Time - 10)
- [x] Test with multiple states (CO, TX, CA)
- [x] Test invalid inputs
- [x] Verify API failure handling
- [x] Confirm forecast renders correctly

### 7.5 UI Polish / Weather Dashboard (Time - 10)
- [ ] Replace table-heavy layout with weather dashboard cards
- [ ] Add current forecast summary card
- [ ] Add selected state/zone/station header
- [ ] Convert zone/station selection to dropdowns or cleaner cards
- [ ] Add 7-day forecast cards
- [ ] Add selected day behavior
- [ ] Add hourly forecast section

---

### 8. Final Submission (Time - ___)
- [ ] Update README with final details
- [ ] Fill in total time spent
- [ ] Clean up unused files
- [ ] Final code review
- [ ] Push to GitHub
- [ ] Submit repo link

--- 

### TOTAL TIME SPENT - ___

- testing