using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebAPI.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public IEnumerable<SingleOrderModel> SingleOrders { get; set; }
    }
}