using System.Data.Entity;

namespace MinjustInvent.Models
{
    public class ComputerContext : DbContext
    {
        public ComputerContext() : base("minjustDB")
        {

        }
        public DbSet<Computers> computers { get; set; }
    }
}
