using Data.Access.Model;
using System.Data.Entity;

namespace Data.Access.Contexts
{
    public class RequestContext : DbContext
    {
        public RequestContext() : base("XmlTest")
        {

        }

        public DbSet<Request> Requests { get; set; }
    }
}
