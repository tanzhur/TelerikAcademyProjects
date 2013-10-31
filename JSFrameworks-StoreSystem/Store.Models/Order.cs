using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Order
    {
        public int Id { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<SingleOrder> SingleOrders { get; set; }

        public virtual string Status { get; set; }
    }
}
