using System.Collections;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ManagementSystem.Areas.Adminstration.ViewModels;
using ManagementSystem.Data;
using ManagementSystem.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace ManagementSystem.Areas.Adminstration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlansController : Controller
    {
        private readonly IUowData db;
        public PlansController()
        {
            this.db = new UowData();
        }

        //
        // GET: /Adminstration/Plans/
        public ActionResult Index()
        {
            ViewData["users"] = this.db.AppUsers.All().Select(u => new UserViewModel
                    {
                        UserId = u.Id,
                        Username = u.UserName
                    }
                );

            return View();
        }

        //[ValidateAntiForgeryToken]
        public JsonResult CreatePlan([DataSourceRequest] DataSourceRequest request, PlanViewModel plan)
        {
            if (plan != null && ModelState.IsValid)
            {
                ApplicationUser currentUser = this.db.AppUsers.All().FirstOrDefault(x => x.Id == plan.OwnerId);
                Plan newPlan = new Plan()
                    {
                        Title =  plan.Title,
                        Description = plan.Description,
                        Owner = currentUser,
                        //Participants = plan.Participants
                    };

                plan.Id = newPlan.Id;
                plan.OwnerName = currentUser.UserName;

                this.db.Plans.Add(newPlan);
                this.db.SaveChanges();
            }

            return Json(new[] {plan}.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadPlans([DataSourceRequest] DataSourceRequest request)
        {
            var result = this.db.Plans.All().Select(PlanViewModel.FromPlan);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public JsonResult UpdatePlan([DataSourceRequest] DataSourceRequest request, PlanViewModel plan)
        {
            var existingPlan = this.db.Plans.Find(plan.Id);

            if (existingPlan != null && ModelState.IsValid)
            {
                var owner = this.db.AppUsers.All().FirstOrDefault(u => u.Id == plan.OwnerId);
                if (owner != null)
                {
                    existingPlan.Title = plan.Title;
                    existingPlan.Description = plan.Description;
                    existingPlan.Owner = owner;

                    this.db.SaveChanges();

                    plan.OwnerName = owner.UserName;
                }
            }

            return Json(new[] { plan }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePlan([DataSourceRequest] DataSourceRequest request, PlanViewModel plan)
        {
            var existingPlan = this.db.Plans.Find(plan.Id);

            if (existingPlan != null)
            {
                if (existingPlan.Todos.Count > 0) 
                {
                   existingPlan.Todos.ToList().ForEach(t=>this.db.Todos.Delete(t));
                }

                this.db.Plans.Delete(existingPlan);
                this.db.SaveChanges();
            }

            return Json(new[] {plan}, JsonRequestBehavior.AllowGet);
        }
    }
}