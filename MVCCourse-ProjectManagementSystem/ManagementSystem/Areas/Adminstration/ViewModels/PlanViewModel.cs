using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;
using ManagementSystem.Data;
using ManagementSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class PlanViewModel
    {
        public static Expression<Func<Plan, PlanViewModel>> FromPlan
        {
            get
            {
                return p => new PlanViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    OwnerId = p.Owner.Id,
                    OwnerName = p.Owner.UserName,
                    //Participants = p.Participants
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Required]
        public string Description { get; set; }

        [UIHint("UsersDropDown")]
        [DisplayName("Owner Name")]
        public string OwnerId { get; set; }

        //[UIHint("ParticipantsList")]
        //public ICollection<Participant> Participants { get; set; }

        [ScaffoldColumn(false)]
        public string OwnerName { get; set; }

        [UIHint("UsersDropDown")]
        public virtual ApplicationUser Owner { get; set; }
    }
}