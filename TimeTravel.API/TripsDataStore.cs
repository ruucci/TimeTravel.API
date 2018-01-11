using System;
using System.Collections.Generic;
using TimeTravel.API.Models;

namespace TimeTravel.API
{
    public class TripsDataStore
    {
        public static TripsDataStore Current { get; } = new TripsDataStore();
        public List<TripDto> Trips { get; set; }

        public TripsDataStore()
        {
            Trips = new List<TripDto>()
            {
                new TripDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the Mumbai feels.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto() {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Was covered in snow!!!"
                        },
                        new PointsOfInterestDto() {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "Too high expectations. Not a night thing."
                        },
                        new PointsOfInterestDto() {
                            Id = 3,
                            Name = "Brooklyn Bridge",
                            Description = "Simply Amazing. Do not miss this!"
                        },
                        new PointsOfInterestDto() {
                            Id = 4,
                            Name = "Rockfellar Center",
                            Description = "Do not miss this if you're coming around Christmas"
                        }
                        
                    }
                        
                },
                new TripDto()
                {
                    Id=2,
                    Name="Grand Canyon National Park",
                    Description="Hot! Terrific Colors.",
                    PointsOfInterest= new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto() {
                            Id = 1,
                            Name = "Mather Point",
                            Description = "Was covered in snow!!!"
                        },
                        new PointsOfInterestDto() {
                            Id = 2,
                            Name = "Horshoe Bend",
                            Description = "Was covered in snow!!!"
                        },
                        new PointsOfInterestDto() {
                            Id = 3,
                            Name = "Antelope Canyon",
                            Description = "Was covered in snow!!!"
                        }
                    }
                },
                new TripDto()
                {
                    Id=3,
                    Name="Zion National Park",
                    Description="The one with an Emerald necklace.",
                    PointsOfInterest= new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto() {
                            Id = 1,
                            Name = "Angels Landing",
                            Description = "Was covered in snow!!!"
                        },
                        new PointsOfInterestDto() {
                            Id = 2,
                            Name = "River Walk",
                            Description = "Was covered in snow!!!"
                        }
                    }
                }
            };
        }
    }
}
