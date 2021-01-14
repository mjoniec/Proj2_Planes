using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Mvc;
using MockAirTrafficinfoApi.Services;

namespace MockAirTrafficinfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockAirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;

        public MockAirTrafficInfoController(AirTrafficInfoService airTrafficInfoService)
        {
            _airTrafficInfoService = airTrafficInfoService;
        }

        //https://localhost:44389/api/MockAirTrafficInfo
        [HttpGet]
        public AirTrafficInfoContract Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
        }

        //https://localhost:44389/api/MockAirTrafficInfo
        [HttpPost]
        public IActionResult Post()
        {
            _airTrafficInfoService.UpdateAirTrafficInfo();

            return Ok();
        }
    }
}
