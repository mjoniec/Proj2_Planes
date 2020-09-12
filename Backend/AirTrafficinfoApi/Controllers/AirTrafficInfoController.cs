using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirTrafficinfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        // GET: api/AirTrafficInfo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AirTrafficInfo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AirTrafficInfo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/AirTrafficInfo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
