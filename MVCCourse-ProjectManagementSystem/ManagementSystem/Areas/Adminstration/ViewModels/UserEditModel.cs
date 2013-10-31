using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ManagementSystem.Models;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class UserEditModel
    {
        public static Expression<Func<ApplicationUser, UserEditModel>> FromApplicationUser
        {
            get
            {
                return u => new UserEditModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                    AboutMe = u.AboutMe
                };
            }
        }

        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string AboutMe { get; set; }
    }
}