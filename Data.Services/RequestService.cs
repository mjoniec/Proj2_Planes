using Data.Model;
using Data.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Data.Services
{
    /// <summary>
    /// This service converts JSON into db model and returns XML based on db model
    /// TODO: Refactor and export parsing functionality
    /// https://github.com/mjoniec/MarJonDemo/issues/3
    /// </summary>
    public class RequestService : IRequestService
    {
        IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Stores requests into database
        /// </summary>
        /// <param name="requests">JSON for deserialization</param>
        /// <returns></returns>
        public string SaveRequests(object requests)
        {
            //TODO: improve deserialization
            //var requestsParsed = JsonConvert.DeserializeObject<List<Request>>(requests.ToString());

            var requestsParsed = AllChildren(JObject.Parse(requests.ToString()))
                .First(c => c.Type == JTokenType.Array && c.Path.Contains("requests"))
                .Children<JObject>();

            var requestsModels = new List<Request>();

            foreach (var requestParsed in requestsParsed)
            {
                var request = new Request
                {
                    Index = int.Parse(requestParsed.GetValue("ix").ToString()),
                    Name = requestParsed.GetValue("name").ToString(),
                    Visits = null
                };

                var visits = requestParsed.GetValue("visits").ToString();

                if (!string.IsNullOrEmpty(visits)) request.Visits = int.Parse(visits);

                var date = requestParsed.GetValue("date").ToString();
                var dateTime = new DateTime();

                if (DateTime.TryParse(date, out dateTime)) request.Date = dateTime;

                requestsModels.Add(request);
            }

            return _requestRepository.SaveRequests(requestsModels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Serialized XML with requests</returns>
        public string GetRequests()
        {
            //var requests = _requestRepository.GetRequests();
            //var xmlSerializer = new XmlSerializer(typeof(Request));
            //string s = string.Empty;

            //using (StringWriter stringWriter = new StringWriter())
            //{
            //    foreach (var r in requests)
            //    {
            //        xmlSerializer.Serialize(stringWriter, r);
            //    }

            //    s = stringWriter.ToString();
            //}

            //return s;

            var requests = _requestRepository.GetRequests();
            var stringBuilder = new StringBuilder();

            foreach (var r in requests)
            {
                stringBuilder.Append(r.ToXML());
            }

            return stringBuilder.ToString();
        }

        //TODO: extension method or sth...
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;

                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}
