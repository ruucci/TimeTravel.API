using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTravel.API.Entities
{
    public class PointsOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        public int TripId { get; set; }
    }
}