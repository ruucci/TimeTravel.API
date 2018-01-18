using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTravel.API.Models;
using TimeTravel.API.Services;

namespace TimeTravel.API.Controllers
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;
        private ILogger<TripsController> _logger;

        public TripsController(ITripInfoRepository tripInfoRepository, ILogger<TripsController> logger)
        {
            _logger = logger;
            _tripInfoRepository = tripInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetTrips()
        {
            var tripEntities = _tripInfoRepository.GetTrips();
            var results = Mapper.Map<IEnumerable<TripWithoutPointsOfInterestDto>>(tripEntities);

            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetTrip")]
        public IActionResult GetTrip(int id, bool includePointsOfInterest = false)
        {
            var trip = _tripInfoRepository.GetTrip(id, includePointsOfInterest);
            if(trip == null)
            {
                return NotFound();
            }

            if(includePointsOfInterest)
            {
                var tripResult = Mapper.Map<TripDto>(trip);

                return Ok(tripResult);
            }


            var tripWithoutPointsOfInterestResut = Mapper.Map<TripWithoutPointsOfInterestDto>(trip);
            return Ok(tripWithoutPointsOfInterestResut);

        }

        [HttpPost]
        public IActionResult CreateTrip([FromBody] TripWithPointsOfInterestCreatorDto trip, bool includePointsOfInterest = false)
        {
            try
            {
                if (trip == null)
                {
                    return BadRequest();
                }

                if (trip.Name.Trim() == trip.Description.Trim())
                {
                    ModelState.AddModelError("Description", "Please enter a description that is different from the name");
                }

                if (includePointsOfInterest)
                {
                    if (trip.PointsOfInterest==null)
                    {
                        ModelState.AddModelError("Description", "Please enter a Point Of Interest or change the includePointsOfInterest parameter to false");
                    }
                }
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var finaltrip = Mapper.Map<Entities.Trip>(trip);

                _tripInfoRepository.AddTrip(finaltrip);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var createdTripToReturn = Mapper.Map<Models.TripDto>(finaltrip);

                return CreatedAtRoute("GetTrip", new { id = createdTripToReturn.Id }, createdTripToReturn);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while creating trip.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTrip(int id, [FromBody] TripUpdaterDto trip)
        {
            try
            {

                if (trip == null)
                {
                    return BadRequest();
                }

                if (trip.Name != null && trip.Description != null)
                {
                    if (trip.Name.Trim() == trip.Description.Trim())
                    {
                        ModelState.AddModelError("Description", "Please enter a description that is different from the name");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_tripInfoRepository.TripExists(id))
                {
                    return NotFound();
                }

                var tripEntity = _tripInfoRepository.GetTrip(id,false);
                if (tripEntity == null)
                {
                    return NotFound();
                }

                Mapper.Map(trip, tripEntity);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting trip with Id {id}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdateTrip(int id, [FromBody] JsonPatchDocument<TripUpdaterDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                if (!_tripInfoRepository.TripExists(id))
                {
                    return NotFound();
                }

                var tripEntity = _tripInfoRepository.GetTrip(id, false);
                if (tripEntity == null)
                {
                    return NotFound();
                }

                var tripToPatch = Mapper.Map<TripUpdaterDto>(tripEntity);

                patchDoc.ApplyTo(tripToPatch, ModelState);


                if (tripToPatch.Name != null && tripToPatch.Description != null)
                {
                    if (tripToPatch.Name.Trim() == tripToPatch.Description.Trim())
                    {
                        ModelState.AddModelError("Description", "Please enter a description that is different from the name");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                TryValidateModel(tripToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(tripToPatch, tripEntity);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();

            }

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting trip with Id {id}.", ex);
                return StatusCode(500, "A problem occured while handling your request.");
            }
        }
    }
}