using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Model
{
    public class Shop
    {
        public int ShopID { get; set; }
        public string Name { get; set; }
        private ICollection<Sale> sales;

        public Shop()
        {
            this.sales = new HashSet<Sale>();
        }

        public ICollection<Sale> Sales
        {
            get { return sales; }
            set { sales = value; }
        }
    }
}