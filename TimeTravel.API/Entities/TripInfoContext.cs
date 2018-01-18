using System;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TimeTravel.API.Entities
{
    public class TripInfoContext : DbContext
    {
        public TripInfoContext(DbContextOptions<TripInfoContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Trip> Trips { get; set; }

        public DbSet<PointsOfInterest> PointsOfInterest { get; set; }
    }
}