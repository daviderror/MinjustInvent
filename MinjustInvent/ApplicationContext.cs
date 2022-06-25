using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;

namespace MinjustInvent
{
    class ApplicationContext:DbContext
    {
        public DbSet<Computer> Computers { get; set; }
        public ApplicationContext() : base("minjustDB") { }
    }
}
