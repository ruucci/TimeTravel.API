using System;
using Microsoft.AspNetCore.Mvc;
using TimeTravel.API.Entities;

namespace TimeTravel.API.Controllers
{
    public class DummyController : Controller
    {
        private TripInfoContext _ttx;
        public DummyController(TripInfoContext ttx)
        {
            _ttx = ttx;
        }

        [HttpGet]
        [Route("api/testdb")]
        public IActionResult TestDb()
        {
            return Ok();
        }
    }
}