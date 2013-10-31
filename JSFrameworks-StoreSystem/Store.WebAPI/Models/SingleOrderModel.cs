using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebAPI.Models
{
    public class SingleOrderModel
    {
        public int Id { get; set; }

        public ProductModel Product { get; set; }

        public int Quantity { get; set; }
    }
}