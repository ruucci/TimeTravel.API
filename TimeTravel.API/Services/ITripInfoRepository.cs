using System;
using System.Collections.Generic;
using TimeTravel.API.Entities;

namespace TimeTravel.API.Services
{
    public interface ITripInfoRepository
    {
        bool TripExists(int tripId);
        IEnumerable<Trip> GetTrips();
        Trip GetTrip(int tripId,bool includePointsOfInterest);
        IEnumerable<PointsOfInterest> GetPointsOfInterestForTrip(int tripId);
        PointsOfInterest GetPointOfInterestForTrip(int tripId, int pointOfInterestId);
        void AddPointOfInterestForTrip(int tripId, PointsOfInterest pointOfInterest);
        bool Save();
        void DeletePointOfInterest(PointsOfInterest pointOfInterest);
    }
}
