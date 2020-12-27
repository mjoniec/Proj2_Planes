using AirTrafficinfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public void Post([FromBody] PlaneContract planeContract)
        {
            _airTrafficInfoService.UpdatePlaneInfo(planeContract);
        }
    }
}
