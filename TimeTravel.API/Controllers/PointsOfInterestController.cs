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
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ITripInfoRepository _tripInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ITripInfoRepository tripInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _tripInfoRepository = tripInfoRepository;
        }

        [HttpGet("{tripId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int tripId)
        {
            try
            {
                if(!_tripInfoRepository.TripExists(tripId))
                {
                    _logger.LogInformation($"Trip with id {tripId} wasn't found when trying to access Points of Interest.");
                    return NotFound();
                }
                var pointsOfInterestForTrip = _tripInfoRepository.GetPointsOfInterestForTrip(tripId);

                var pointsOfInterestForTripResults = 
                    Mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsOfInterestForTrip);

                return Ok(pointsOfInterestForTripResults);

                //WITHOUT AutoMapper
                //var pointsOfInterestForTripResults = new List<PointsOfInterestDto>();
                //foreach (var poi in pointsOfInterestForTrip)
                //{
                //    pointsOfInterestForTripResults.Add(new PointsOfInterestDto()
                //    {
                //        Id = poi.Id,
                //        Name = poi.Name,
                //        Description = poi.Description
                //    });
                //}

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
                if (!_tripInfoRepository.TripExists(tripId))
                {
                    _logger.LogInformation($"Trip with id {tripId} wasn't found when trying to access Points of Interest.");
                    return NotFound();
                }

                var pointOfInterest = _tripInfoRepository.GetPointOfInterestForTrip(tripId,id);

                if(pointOfInterest == null)
                {
                    return NotFound();
                }

                var pointOfInterestResult = Mapper.Map<PointsOfInterestDto>(pointOfInterest);

                return Ok(pointOfInterestResult);

                //WITHOUT AutoMapper
                //var pointOfInterestResult = new PointsOfInterestDto()
                //{
                //    Id = pointOfInterest.Id,
                //    Name = pointOfInterest.Name,
                //    Description = pointOfInterest.Description
                //};
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

                if(!_tripInfoRepository.TripExists(tripId))
                {
                    return NotFound();
                }

                var finalPointOfInterest = Mapper.Map<Entities.PointsOfInterest>(pointOfInterest);
         
                _tripInfoRepository.AddPointOfInterestForTrip(tripId, finalPointOfInterest);

                if(!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var createdPointOfInterestToReturn = Mapper.Map<Models.PointsOfInterestDto>(finalPointOfInterest);

                return CreatedAtRoute("GetPointOfInterest", new { tripId = tripId , id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);

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

                if (!_tripInfoRepository.TripExists(tripId))
                {
                    return NotFound();
                }

                var pointOfInterestEntity = _tripInfoRepository.GetPointOfInterestForTrip(tripId, id);
                if(pointOfInterestEntity == null)
                {
                    return NotFound();
                }

                Mapper.Map(pointOfInterest, pointOfInterestEntity);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

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

                if (!_tripInfoRepository.TripExists(tripId))
                {
                    return NotFound();
                }

                var pointOfInterestEntity = _tripInfoRepository.GetPointOfInterestForTrip(tripId, id);
                if (pointOfInterestEntity == null)
                {
                    return NotFound();
                }

                var pointOfInterestToPatch = Mapper.Map<PointsOfInterestUpdaterDto>(pointOfInterestEntity);

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

                Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

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
                if (!_tripInfoRepository.TripExists(tripId))
                {
                    return NotFound();
                }

                var pointOfInterestEntity = _tripInfoRepository.GetPointOfInterestForTrip(tripId, id);
                if (pointOfInterestEntity == null)
                {
                    return NotFound();
                }

                _tripInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

                if (!_tripInfoRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
                _mailService.Send("Point of Interest has been deleted.", $"Point of Interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

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