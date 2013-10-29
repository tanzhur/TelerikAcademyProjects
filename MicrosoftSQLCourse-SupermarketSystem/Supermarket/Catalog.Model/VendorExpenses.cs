using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLCatalog.Model;

namespace Catalog.Model
{
    public class VendorExpense
    {
        public int VendorExpenseID { get; set; }
       
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }
        
        public decimal Expense { get; set; }
        
        public DateTime Month { get; set; }
        
    }
}
