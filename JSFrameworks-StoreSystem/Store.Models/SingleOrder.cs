using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class SingleOrder
    {
        public int Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}