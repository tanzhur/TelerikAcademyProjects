using System;
using System.Data.Entity;
using System.Linq;
using MySQLCatalog.Model;
using Catalog.Model;

namespace Catalog.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext()
            : base("CatalogContextDb")
        {

        }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<VendorExpense> VendorExpenses { get; set; }
    }
}