using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MockAirTrafficinfoApi.Services;
using System.Threading.Tasks;

namespace MockAirTrafficinfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockAirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;
        private readonly StaticResourcesService _staticResourcesService;

        public MockAirTrafficInfoController(AirTrafficInfoService airTrafficInfoService, StaticResourcesService staticResourcesService)
        {
            _airTrafficInfoService = airTrafficInfoService;
            _staticResourcesService = staticResourcesService;
        }

        //https://localhost:44389/api/MockAirTrafficInfo
        [EnableCors("MyAllowedOrigins")]
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

        //https://localhost:44389/api/MockAirTrafficInfo/WorldMap
        [EnableCors("MyAllowedOrigins")]
        [HttpGet("WorldMap")]
        public async Task<string> WorldMap()
        {
            return await _staticResourcesService.GetWorldMap();
        }
    }
}
