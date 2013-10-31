namespace ManagementSystem.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ManagementSystem.Models;
    using ManagementSystem.Data;
    using ManagementSystem.ViewModels;

    public class TodosController : Controller
    {
        private readonly IUowData db;

        public TodosController()
        {
            this.db = new UowData();
        }

        // GET: /Todos/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PlanId = id;
            var priorities = from Priority p in Enum.GetValues(typeof(Priority))
                             select new { Id = p, Name = p.ToString() };
            ViewBag.Priority = new SelectList(priorities, "Id", "Name");
            var states = from State s in Enum.GetValues(typeof(State))
                             select new { Id = s, Name = s.ToString() };
            ViewBag.State = new SelectList(states, "Id", "Name");

            return View();
        }

        // POST: /Todos/Create
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TodoViewModel todo)
        {
            if (ModelState.IsValid)
            {
                db.Todos.Add(new Todo
                {
                    DateCreated = DateTime.Now,
                    Description = todo.Description,
                    Plan = this.db.Plans.Find(todo.PlanId),
                    Priority = todo.Priority,
                    State = todo.State,
                    Title = todo.Title
                });
                db.SaveChanges();
                return RedirectToAction("Details", "Plans", new { area = "", id = todo.PlanId });
            }

            return View(todo);
        }

        // GET: /Todos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find((int)id);
            if (todo == null)
            {
                return HttpNotFound();
            }

            var priorities = from Priority p in Enum.GetValues(typeof(Priority))
                             select new { Id = p, Name = p.ToString() };
            ViewBag.Priority = new SelectList(priorities, "Id", "Name");
            var states = from State s in Enum.GetValues(typeof(State))
                         select new { Id = s, Name = s.ToString() };
            ViewBag.State = new SelectList(states, "Id", "Name");

            return View(new TodoViewModel
            {
                Description = todo.Description,
                PlanId = todo.Plan.Id,
                State = todo.State,
                Priority = todo.Priority,
                Title = todo.Title
            });
        }

        // POST: /Todos/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TodoViewModel todo)
        {
            if (ModelState.IsValid)
            {
                var target = this.db.Todos.Find(todo.Id);
                target.Description = todo.Description;
                target.Priority = todo.Priority;
                target.State = todo.State;
                target.Title = todo.Title;
                db.SaveChanges();
                return RedirectToAction("Details", "Plans", new { area = "", id = todo.PlanId });
            }
            return View(todo);
        }

        // GET: /Todos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find((int)id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: /Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Todo todo = db.Todos.Find(id);
            var planId = todo.Plan.Id;
            db.Todos.Delete(todo);
            db.SaveChanges();
            return RedirectToAction("Details", "Plans", new { area = "", id = planId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
