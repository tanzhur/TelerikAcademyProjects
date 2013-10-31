using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ManagementSystem.Models;

namespace ManagementSystem.ViewModels
{
    public class ParticipantCreateViewModel
    {
        public static Expression<Func<ApplicationUser, ParticipantCreateViewModel>> FromAppUser
        {
            get
            {
                return a => new ParticipantCreateViewModel()
                {
                    Id = a.Id,
                    UserName = a.UserName
                };
            }
        }

        public string Id { get; set; }

        public string UserName { get; set; }
    }
}