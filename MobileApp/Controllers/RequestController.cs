using Data.Services;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MobileApp.Controllers
{
    [MobileAppController]
    public class RequestController : ApiController
    {
        IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        //TODO routing in azure mobile web api https://github.com/mjoniec/MarJonDemo/issues/1
        //[HttpGet("jobs/saveFiles")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            string XML = _requestService.GetRequests();

            return ResponseMessage(new HttpResponseMessage()
            {
                Content = new StringContent(XML, Encoding.UTF8, "application/xml")
            });
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] object requests)
        {
            try
            {
                var message = _requestService.SaveRequests(requests);

                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest("Incorrect requests in body parameter" + e);
            }
        }
    }
}