using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class UserViewModel
    {
        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        public string Username { get; set; }  
    }
}