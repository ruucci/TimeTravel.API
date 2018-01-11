using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TimeTravel.API.Controllers
{
    [Route("api/trips")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{tripId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int tripId)
        {
            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);
            if ( trip == null )
            {
                return NotFound();
            }

            return Ok(trip.PointsOfInterest);
        }

        [HttpGet("{tripId}/pointsofinterest/{id}")]
        public IActionResult GetPointOfInterest (int tripId, int id)
        {
            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return NotFound();
            }

            var pointsOfInterest = trip.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointsOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointsOfInterest);
        }
    }
}
