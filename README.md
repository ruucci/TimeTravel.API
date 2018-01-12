# TimeTravel.API
Learning to develop a simple ASP.NET Core 2.0 API with simple operations to support my Angular 5 project's backend.
## Notes
The main difference between ASP.NET Core 1.0 and Core 2.0 is that the AspNetCore.Mvc.All package is:
1. Not referenced in the Core 1.0 framework, you need to import every package as required, but these will be available on the runtime store on publish.
2. Referenced in the Core 2.0 framework, so becomes easy to develop, but these will not be available at the runtime store after publish.
# Endpoints
## GET
```
/api/trips - All trips.
/api/trips/{id} - Only the passed in Trip Id.
/api/trips/{tripId}/pointsofinterest - All PointsOfInterest for the passed in Trip Id.
/api/trips/{tripId}/pointsofinterest/{id} - Only the passed in Trip Id and PointsOfInterest Id.
/api/trips/throwexception - Simply for the purpose of testing Nlog.Extensions.Logging features.
```
## POST - Create a new PointOfInterest for a trip
```
/api/trips/{tripId}/pointsofinterest
```
## PUT - Fully Update the PointOfInterest for a trip
```
/api/trips/{tripId}/pointsofinterest/{id}
```
## PATCH - Partially Update the PointOfInterest for a trip
```
/api/trips/{tripId}/pointsofinterest/{id}
```
## DELETE - Delete the PointOfInterest for a trip
```
/api/trips/{tripId}/pointsofinterest/{id}
```
# References
```
app.pluralsight.com
```
