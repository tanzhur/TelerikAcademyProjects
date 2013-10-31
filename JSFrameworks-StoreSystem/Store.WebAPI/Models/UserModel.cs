using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebAPI.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string AuthCode { get; set; }

        public bool? IsAdmin { get; set; }

        public string ImageSource { get; set; }

    }
}