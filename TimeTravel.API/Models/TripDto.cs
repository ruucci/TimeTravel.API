using System;
using System.Collections.Generic;

namespace TimeTravel.API.Models
{
    public class TripDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterest { get
            {
                return PointsOfInterest.Count;
            }
        }
        public ICollection<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();
    }
}
