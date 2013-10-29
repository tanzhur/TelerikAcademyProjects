using System;
using System.Linq;
using MySQLCatalog.Model;

namespace Catalog.Model
{
    public class Sale
    {
        public int SaleID { get; set; }

        public DateTime Date { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Sum { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int ShopID { get; set; }
        public virtual Shop Shop { get; set; }
    }
}