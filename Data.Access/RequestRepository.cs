using Data.Access.Contexts;
using Data.Access.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Access.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        RequestContext _requestContext;

        public RequestRepository(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public IEnumerable<Request> GetRequests()
        {
            return _requestContext.Requests.ToList();
        }

        public string SaveRequests(IEnumerable<Request> requests)
        {
            var stringBuilder = new StringBuilder("save requests table log");

            stringBuilder.AppendLine();

            var ids = new List<int>();

            foreach (var row in _requestContext.Requests)
            {
                ids.Add(row.Index);
            }

            foreach (var request in requests)
            {
                try
                {
                    if (!ids.Any(id => id == request.Index))
                    {
                        _requestContext.Requests.Add(request);
                        ids.Add(request.Index);

                        stringBuilder.AppendLine("added request with id=" + request.Index);
                    }
                    else
                    {
                        //TODO modify existing request and write info as edited

                        stringBuilder.AppendLine("request already in use with id=" + request.Index);
                    }
                }
                catch(Exception e)
                {
                    stringBuilder.AppendLine("not added request with id=" + request.Index);
                    stringBuilder.AppendLine(e.Message);
                }
            }

            _requestContext.SaveChanges();

            return stringBuilder.ToString();
        }
    }
}
