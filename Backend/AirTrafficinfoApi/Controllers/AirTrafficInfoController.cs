using AirTrafficinfoApi.Services;
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

        // GET: api/AirTrafficInfo
        [HttpGet]
        public string Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
        }

        // POST: api/AirTrafficInfo
        [HttpPost("{plane}/{latitude}/{longitude}")]
        public void Post(string plane, double latitude, double longitude)
        {
            _airTrafficInfoService.UpdatePlaneInfo(plane, latitude, longitude);
        }
    }
}
