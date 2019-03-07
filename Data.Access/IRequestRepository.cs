using System.Collections.Generic;

namespace Data.Access
{
    public interface IRequestRepository
    {
        string SaveRequests(IEnumerable<Request> requests);
        IEnumerable<Request> GetRequests();
    }
}
