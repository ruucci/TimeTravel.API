using System;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TimeTravel.API.Models;

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

        [HttpGet("{tripId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
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

        [HttpPost("{tripId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int tripId,
                                                   [FromBody] PointsOfInterestCreatorDto pointOfInterest)
        {
            if (pointOfInterest==null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name.Trim() == pointOfInterest.Description.Trim())
            {
                ModelState.AddModelError("Description", "Please enter a description that is different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = TripsDataStore.Current.Trips.SelectMany(
                t => trip.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointsOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            trip.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { tripId = tripId , id = finalPointOfInterest.Id}, finalPointOfInterest);
        }

        [HttpPut("{tripId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int tripId, int id, [FromBody] PointsOfInterestUpdaterDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if(pointOfInterest.Name!=null && pointOfInterest.Description!=null)
            {
                if (pointOfInterest.Name.Trim() == pointOfInterest.Description.Trim())
                {
                    ModelState.AddModelError("Description", "Please enter a description that is different from the name");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = trip.PointsOfInterest.FirstOrDefault((p => p.Id == id));
            if(pointOfInterestFromStore==null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{tripId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int tripId, int id, [FromBody] JsonPatchDocument<PointsOfInterestUpdaterDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = trip.PointsOfInterest.FirstOrDefault((p => p.Id == id));
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = 
                new PointsOfInterestUpdaterDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (pointOfInterestToPatch.Name != null && pointOfInterestToPatch.Description != null)
            {
                if (pointOfInterestToPatch.Name.Trim() == pointOfInterestToPatch.Description.Trim())
                {
                    ModelState.AddModelError("Description", "Please enter a description that is different from the name");
                }
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{tripId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int tripId, int id)
        {
            var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = trip.PointsOfInterest.FirstOrDefault((p => p.Id == id));
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            trip.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }

    }
}
