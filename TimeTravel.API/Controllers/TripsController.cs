using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TimeTravel.API.Controllers
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        [HttpGet()]
        public IActionResult GetTrips()
        {
            return Ok(TripsDataStore.Current.Trips);
        }

        [HttpGet("{id}")]
        public IActionResult GetTrip(int id)
        {
            var tripsToReturn = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == id);
            if (tripsToReturn == null)
            {
                return NotFound();
            }

            return Ok(tripsToReturn);
        }
    }
}
