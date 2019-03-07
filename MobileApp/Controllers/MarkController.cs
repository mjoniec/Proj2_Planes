using Data.Services;
using Microsoft.Azure.Mobile.Server.Config;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MobileApp.Controllers
{
    [MobileAppController]
    public class MarkController : ApiController
    {
        IMarkService _markService;

        public MarkController(IMarkService markService)
        {
            _markService = markService;
        }

        //http://localhost:57158/api/mark
        [HttpGet]
        public IHttpActionResult Get()
        {
            var list = _markService.GetAllDemo().Select(l => l.X).ToList();
            var list2 = new List<string>();

            foreach(var l in list)
            {
                list2.Add(l.ToString());
            }

            return Ok(list2);
        }

        //http://localhost:57158/api/mark/3
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var value = _markService.GetById(id.ToString());

            return Ok(value);
        }
    }
}