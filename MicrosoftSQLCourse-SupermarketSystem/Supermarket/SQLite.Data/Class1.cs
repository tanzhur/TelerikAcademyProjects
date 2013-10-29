using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonReport;

namespace SQLiteData
{
    public class SQLContext : DbContext
    {
        public SQLContext()
            : base("ProductReport")
        {
        }

        public DbSet<ProductReport> ProductReports { get; set; }
    }
}
