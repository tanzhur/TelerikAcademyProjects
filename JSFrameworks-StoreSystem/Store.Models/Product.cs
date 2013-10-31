using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        [Column(TypeName="money")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<SingleOrder> SingleOrders { get; set; }
    }
}
