using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementSystem.Controllers
{
    using ManagementSystem.Data;
    using ManagementSystem.Models;
    using ManagementSystem.ViewModels;
    using Microsoft.AspNet.Identity;

    public class UsersController : Controller
    {
        private readonly IUowData db = new UowData();

        //
        // GET: /Users/
        [Authorize]
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    id = User.Identity.GetUserId();
                }
                else
                {
                    Response.Redirect("~/");
                }
            }

            var user = this.db.AppUsers.All().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                Response.Redirect("~/");
            }

            return View(this.AppUserToUserViewModel(user));
        }

        //
        // GET: /Users/Profile
        public PartialViewResult Profile(string id)
        {
            var user = this.db.AppUsers.All().FirstOrDefault(u => u.Id == id);
            return this.PartialView("_Profile", this.AppUserToProfileViewModel(user));
        }

        private UserViewModel AppUserToUserViewModel(ApplicationUser au)
        {
            return new UserViewModel()
            {
                Id = au.Id,
                Profile = this.AppUserToProfileViewModel(au),
                MyPlans = this.db.Plans.All().Where(p => p.Owner.Id == au.Id).ToList(),
                AllPlans = this.db.Plans.All().Where(pl => pl.Participants.Any(p => p.Id == au.Id)).ToList()
            };
        }

        private ProfileViewModel AppUserToProfileViewModel(ApplicationUser au)
        {
            return new ProfileViewModel()
            {
                Id = au.Id,
                AboutMe = au.AboutMe,
                Email = au.Email,
                FullName = au.FullName,
                Username = au.UserName
            };
        }
	}
}