using Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext()
            : base("TantalumStore")
        { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Product> Products { get; set; }
    
        public DbSet<Order> Orders { get; set; }

        public DbSet<SingleOrder> SingleOrders { get; set; }
    }
}
