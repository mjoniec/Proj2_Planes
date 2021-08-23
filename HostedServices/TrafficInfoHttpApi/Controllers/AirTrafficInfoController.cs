﻿using Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficInfoHttpApi.Services;

namespace TrafficInfoHttpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;
        private readonly StaticResourcesProvider _staticResourcesProvider;

        public AirTrafficInfoController(AirTrafficInfoService airTrafficInfoService,
            StaticResourcesProvider staticResourcesService)
        {
            _airTrafficInfoService = airTrafficInfoService;
            _staticResourcesProvider = staticResourcesService;
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        public AirTrafficInfoContract Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        [Route("GetAirports")]
        public List<AirportContract> GetAirports()
        {
            return _airTrafficInfoService.GetAirTrafficInfo().Airports;
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdatePlaneInfo")]
        public void UpdatePlaneInfo([FromBody] PlaneContract planeContract)
        {
            _airTrafficInfoService.UpdatePlane(planeContract);
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdateAirportInfo")]
        public void UpdateAirportInfo([FromBody] AirportContract airportContract)
        {
            _airTrafficInfoService.UpdateAirport(airportContract);
        }

        //remove after fix https://github.com/mjoniec/Proj2_Planes/issues/17
        //https://localhost:44389/api/AirTrafficInfo/WorldMap
        [EnableCors("MyAllowedOrigins")]
        [HttpGet("WorldMap")]
        public async Task<string> WorldMap()
        {
            return await _staticResourcesProvider.GetWorldMap();
        }
    }
}