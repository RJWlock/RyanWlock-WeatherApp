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

### 3. API Integration (Time - ___)
- [x] Create `Services/WeatherService.cs`
- [x] Register `HttpClient` in `Program.cs`
- [ ] Add required NWS `User-Agent` header 
- [ ] Create DTO/model classes for NWS responses
- [ ] Fetch zones/stations by state
- [ ] Fetch forecast data for selected entity
- [ ] Handle API errors and edge cases

---

### 4. Core App Workflow (Time - ___)
- [ ] Add state input field
- [ ] Trigger API call on search
- [ ] Display zones/stations list
- [ ] Handle selection of entity
- [ ] Load forecast data

---

### 5. Forecast Display (Time - ___)
- [ ] Show forecast period (name/time)
- [ ] Display temperature and units
- [ ] Display short forecast
- [ ] Display detailed forecast
- [ ] Implement daily view
- [ ] (Optional) Implement hourly view

---

### 6. UI Polish (Time - ___)
- [ ] Add loading indicators
- [ ] Add input validation
- [ ] Add error handling UI
- [ ] Add empty-state messaging
- [ ] Improve layout with MudBlazor components

---

### 7. Testing & Validation (Time - ___)
- [ ] Test with multiple states (CO, TX, CA)
- [ ] Test invalid inputs
- [ ] Verify API failure handling
- [ ] Confirm forecast renders correctly

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