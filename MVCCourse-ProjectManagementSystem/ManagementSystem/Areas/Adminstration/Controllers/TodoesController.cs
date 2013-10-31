using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ManagementSystem.Areas.Adminstration.ViewModels;
using ManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystem.Models;

namespace ManagementSystem.Areas.Adminstration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TodoesController : Controller
    {
        private readonly IUowData db;
        public TodoesController()
        {
            this.db = new UowData();
        }
        //
        // GET: /Adminstration/Todoes/
        public ActionResult Index()
        {
            ViewData["plans"] = this.db.Plans.All()
                                    .Select(p => new SamplePlanViewModel()
                                        {
                                            PlanName = p.Title,
                                            PlanId = p.Id
                                        });

            //tva sa nekvi probi...
            // koi action izpolzvash
            //za kvo mi e action az mu podavam property :?
            //probvai taka
            return View();
        }

        [ValidateAntiForgeryToken]
        public JsonResult CreateTodo([DataSourceRequest] DataSourceRequest request, TodoViewModel todo)
        {
            if (todo != null && ModelState.IsValid)
            {
                Plan currentPlan = this.db.Plans.All().FirstOrDefault(p=>p.Title == todo.PlanName);
                if (currentPlan != null)
                {
                    Todo newTodo = new Todo()
                        {
                            Title = todo.Title,
                            Description = todo.Description,
                            DateCreated = DateTime.Now,
                            Plan = currentPlan,
                            State = todo.State,
                            Priority = todo.Priority
                        };

                    this.db.Todos.Add(newTodo);
                    this.db.SaveChanges();
                    todo.Id = newTodo.Id;
                    todo.PlanName = currentPlan.Title;
                }
            }

            return Json(new[] { todo }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadTodos([DataSourceRequest] DataSourceRequest request)
        {
            var result = this.db.Todos.All().Select(TodoViewModel.FromTodo);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTodo([DataSourceRequest] DataSourceRequest request, TodoViewModel todo)
        {
            var existingTodo = this.db.Todos.Find(todo.Id);
            var existingPlan = this.db.Plans.All().FirstOrDefault(p => p.Title == todo.PlanName);

            if (existingTodo != null && existingPlan!=null && ModelState.IsValid)
            {
                existingTodo.Title = todo.Title;
                existingTodo.Description = todo.Description;
                existingTodo.DateCreated = DateTime.Now;
                existingTodo.Plan = existingPlan;
                existingTodo.State = todo.State;
                existingTodo.Priority = todo.Priority;

                this.db.SaveChanges();
                todo.Id = existingTodo.Id;
                todo.PlanName = todo.PlanName;
            }

            return Json(new[] {todo}.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteTodo([DataSourceRequest] DataSourceRequest request, TodoViewModel todo)
        {
            var existingTodo = this.db.Todos.Find(todo.Id);

            if (existingTodo != null && ModelState.IsValid)
            {
                this.db.Todos.Delete(existingTodo);
                this.db.SaveChanges();
            }

            return Json(new[] {todo}, JsonRequestBehavior.AllowGet);
        }
    }
}