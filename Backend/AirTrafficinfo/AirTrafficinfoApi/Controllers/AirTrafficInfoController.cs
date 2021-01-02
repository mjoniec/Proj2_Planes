using AirTrafficInfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Mvc;

namespace AirTrafficInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _AirTrafficInfoService;

        public AirTrafficInfoController(AirTrafficInfoService AirTrafficInfoService)
        {
            _AirTrafficInfoService = AirTrafficInfoService;
        }

        [HttpGet]
        public string Get()
        {
            return _AirTrafficInfoService.GetAirTrafficInfo();
        }

        [HttpPost]
        public void Post([FromBody] PlaneContract planeContract)
        {
            _AirTrafficInfoService.UpdatePlaneInfo(planeContract);
        }
    }
}
