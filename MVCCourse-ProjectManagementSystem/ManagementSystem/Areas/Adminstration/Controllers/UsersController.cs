using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ManagementSystem.Areas.Adminstration.ViewModels;
using ManagementSystem.Data;

namespace ManagementSystem.Areas.Adminstration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private IUowData db;
        public UsersController()
        {
            this.db = new UowData();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = db.AppUsers.All().Select(UserEditModel.FromApplicationUser);

            return Json(users.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest] DataSourceRequest request, UserEditModel user)
        {
            var existingUser = this.db.AppUsers.All().FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null && ModelState.IsValid)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.AboutMe = user.AboutMe;
                this.db.SaveChanges();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, UserEditModel user) 
        {
            var existingUser = this.db.AppUsers.All().FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                this.db.AppUsers.Delete(existingUser);
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }
	}
}