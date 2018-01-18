using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTravel.API.Models;
using TimeTravel.API.Services;

namespace TimeTravel.API.Controllers
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;

        public TripsController(ITripInfoRepository tripInfoRepository)
        {
            _tripInfoRepository = tripInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetTrips()
        {
            var tripEntities = _tripInfoRepository.GetTrips();
            var results = Mapper.Map<IEnumerable<TripWithoutPointsOfInterestDto>>(tripEntities);

            //WITHOUT AutoMapper
            //var results = new List<TripWithoutPointsOfInterestDto>();
            //foreach(var tripEntity in tripEntities)
            //{
            //    results.Add(new TripWithoutPointsOfInterestDto
            //    {
            //        Id = tripEntity.Id,
            //        Description = tripEntity.Description,
            //        Name = tripEntity.Name
            //    });
            //}

            return Ok(results);
        }

        [HttpGet("{id}")]
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

                //WITHOUT AutoMapper
                //var tripResult = new TripDto()
                //{
                //    Id = trip.Id,
                //    Name = trip.Name,
                //    Description = trip.Description
                //};

                //foreach(var poi in trip.PointsOfInterest)
                //{
                //    tripResult.PointsOfInterest.Add(
                //        new PointsOfInterestDto()
                //        {
                //            Id = poi.Id,
                //            Name= poi.Name,
                //            Description = poi.Description
                //        });
                //}

                return Ok(tripResult);
            }

            //WITHOUT AutoMapper
            //var tripWithoutPointsOfInterestResut = new TripWithoutPointsOfInterestDto()
            //{
            //    Id = trip.Id,
            //    Name = trip.Name,
            //    Description = trip.Description
            //};

            var tripWithoutPointsOfInterestResut = Mapper.Map<TripWithoutPointsOfInterestDto>(trip);
            return Ok(tripWithoutPointsOfInterestResut);

            //var tripsToReturn = TripsDataStore.Current.Trips.FirstOrDefault(t => t.Id == id);
            //if (tripsToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(tripsToReturn);
        }
    }
}
