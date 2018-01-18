using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeTravel.API.Entities;

namespace TimeTravel.API.Services
{
    public class TripInfoRepository : ITripInfoRepository
    {
        private TripInfoContext _context;

        public TripInfoRepository(TripInfoContext context)
        {
            _context = context;
        }

        public void AddPointOfInterestForTrip(int tripId, PointsOfInterest pointOfInterest)
        {
            var trip = GetTrip(tripId, false);
            trip.PointsOfInterest.Add(pointOfInterest);
        }

        public void AddTrip(Trip trip)
        {
            _context.Trips.Add(trip);
        }


        public bool TripExists(int tripId)
        {
            return _context.Trips.Any(t => t.Id == tripId);
        }

        public PointsOfInterest GetPointOfInterestForTrip(int tripId, int pointOfInterestId)
        {
            return _context.PointsOfInterest.Where(p => p.TripId == tripId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointsOfInterest> GetPointsOfInterestForTrip(int tripId)
        {
            return _context.PointsOfInterest.Where(p => p.TripId == tripId).ToList();
        }

        public Trip GetTrip(int tripId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return _context.Trips.Include(c => c.PointsOfInterest)
                               .Where(c => c.Id == tripId).FirstOrDefault();
            }
            return _context.Trips.Where(c => c.Id == tripId).FirstOrDefault();
        }

        public IEnumerable<Trip> GetTrips()
        {
            return _context.Trips.OrderBy(c => c.Name).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeletePointOfInterest(PointsOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }
    }
}
