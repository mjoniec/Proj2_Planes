using AirTrafficinfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace AirTrafficinfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;

        public AirTrafficInfoController(AirTrafficInfoService airTrafficInfoService)
        {
            _airTrafficInfoService = airTrafficInfoService;
        }

        [HttpGet]
        public string Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
        }

        [HttpGet("[action]")]
        public IActionResult GetMock()
        {
            var airTrafficInfo = _airTrafficInfoService.GetAirTrafficInfoMock();

            return Ok(airTrafficInfo);
        }

        //[HttpPost]
        //public void Post([FromBody] Plane plane)
        //{
        //    _airTrafficInfoService.UpdatePlaneInfo(plane);
        //}

        [HttpPost]
        public void Post([FromBody] PlaneContract planeContract)
        {
            _airTrafficInfoService.UpdatePlaneInfo(planeContract);
        }
    }
}
