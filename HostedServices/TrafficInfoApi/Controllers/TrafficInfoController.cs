using Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficInfoApi.Services;

namespace TrafficInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficInfoController : ControllerBase
    {
        private readonly TrafficInfoService _trafficInfoService;
        private readonly StaticResourcesProvider _staticResourcesProvider;

        public TrafficInfoController(TrafficInfoService trafficInfoService,
            StaticResourcesProvider staticResourcesService)
        {
            _trafficInfoService = trafficInfoService;
            _staticResourcesProvider = staticResourcesService;
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        public TrafficInfoContract Get()
        {
            return _trafficInfoService.GetTrafficInfo();
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        [Route("GetAirport/{airportName}")]
        public AirportContract GetAirport(string airportName)
        {
            return _trafficInfoService.GetAirport(airportName);
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        [Route("GetAirports")]
        public List<AirportContract> GetAirports()
        {
            return _trafficInfoService.GetTrafficInfo().Airports;
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("AddPlane")]
        public IActionResult AddPlane([FromBody] PlaneContract planeContract)
        {
            try
            {
                _trafficInfoService.AddPlane(planeContract);

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("AddAirport")]
        public IActionResult AddAirport([FromBody] AirportContract airportContract)
        {
            try
            {
                _trafficInfoService.AddAirport(airportContract);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdatePlane")]
        public IActionResult UpdatePlane([FromBody] PlaneContract planeContract)
        {
            try
            {
                _trafficInfoService.UpdatePlane(planeContract);

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdateAirport")]
        public IActionResult UpdateAirport([FromBody] AirportContract airportContract)
        {
            try
            {
                _trafficInfoService.UpdateAirport(airportContract);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //remove after fix https://github.com/mjoniec/Proj2_Planes/issues/17
        //https://localhost:44389/api/rTrafficInfo/WorldMap
        [EnableCors("MyAllowedOrigins")]
        [HttpGet("WorldMap")]
        public async Task<string> WorldMap()
        {
            return await _staticResourcesProvider.GetWorldMap();
        }
    }
}
