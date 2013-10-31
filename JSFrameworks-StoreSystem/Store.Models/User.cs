using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string AuthCode { get; set; }
        
        public string Username { get; set; }
        
        public string DisplayName { get; set; }
        
        public string ImageSource { get; set; }
        
        public bool? IsAdmin { get; set; }

        public string SessionKey { get; set; }
    }
}
