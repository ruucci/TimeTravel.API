using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTravel.API.Models
{
    public class PointsOfInterestUpdaterDto
    {
        [Required(ErrorMessage = "Name field is Required")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
