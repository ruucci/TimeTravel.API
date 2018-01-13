using System;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTravel.API.Models;
using TimeTravel.API.Services;

namespace TimeTravel.API.Controllers
{
    [Route("api/trips")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet("{tripId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int tripId)
        {
            try
            {
                var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);
                if (trip == null)
                {
                    _logger.LogInformation($"Trip with id {tripId} wasn't found.");
                    return NotFound();
                }

                return Ok(trip.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpGet("throwexception")]
        public IActionResult ThrowingException()
        {
            try
            {
                throw new Exception("Checking if nlog works on Windows server at least.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliberate Exception thrown.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpGet("{tripId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest (int tripId, int id)
        {
            try
            {
                var trip = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == tripId);

                if (trip == null)
                {
                    _logger.LogInformation($"Trip with id {tripId} wasn't found.");
                    return NotFound();
                }

                var pointsOfInterest = trip.PointsOfInterest.FirstOrDefault(p => p.Id == id);

                if (pointsOfInterest == null)
                {
                    _logger.LogInformation($"Point of interest with id {id} wasn't found.");
                    return NotFound();
                }

                return Ok(pointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId} and pointsOfInterest Id {id}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
            
        }

        [HttpPost("{tripId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int tripId,
                                                   [FromBody] PointsOfInterestCreatorDto pointOfInterest)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpPut("{tripId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int tripId, int id, [FromBody] PointsOfInterestUpdaterDto pointOfInterest)
        {
            try
            {

                if (pointOfInterest == null)
                {
                    return BadRequest();
                }

                if (pointOfInterest.Name != null && pointOfInterest.Description != null)
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
                if (pointOfInterestFromStore == null)
                {
                    return NotFound();
                }

                pointOfInterestFromStore.Name = pointOfInterest.Name;
                pointOfInterestFromStore.Description = pointOfInterest.Description;

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId} and pointsOfInterest Id {id}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpPatch("{tripId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int tripId, int id, [FromBody] JsonPatchDocument<PointsOfInterestUpdaterDto> patchDoc)
        {
            try
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

                if (!ModelState.IsValid)
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

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId} and pointsOfInterest Id {id}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpDelete("{tripId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int tripId, int id)
        {

            try
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

                _mailService.Send("Point of Interest has been deleted.", $"Point of Interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for trip with Id {tripId} and pointsOfInterest Id {id}.", ex);
                return StatusCode(500, ex);
            }

        }

    }
}