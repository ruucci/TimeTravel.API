# TimeTravel.API
Learning to develop a simple ASP.NET Core 2.0 API with simple operations to support my Angular 5 project's backend.
## Notes
The main difference between ASP.NET Core 1.0 and Core 2.0 is that the AspNetCore.Mvc.All package is:
1. Not referenced in the Core 1.0 framework, you need to import every package as required, but these will be available on the runtime store on publish.
2. Referenced in the Core 2.0 framework, so becomes easy to develop, but these will not be available at the runtime store after publish.
# Endpoints
## GET
1. /api/trips - All trips.
2. /api/trips/{id} - Only the passed in Trip Id.
3. /api/trips/{tripId}/pointsofinterest - All PointsOfInterest for the passed in Trip Id.
4. /api/{tripId}/pointsofinterest/{id} - Only the passed in Trip Id and PointsOfInterest Id.
## POST
