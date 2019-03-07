using Data.Access.Model;
using System.Data.Entity;

namespace Data.Access.Contexts
{
    public class MarkContext : DbContext
    {
        //public MarkContext() : base("connection")
        //{

        //}

        public DbSet<MarkModel> Marks { get; set; }
    }
}
