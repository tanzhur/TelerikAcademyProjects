namespace ManagementSystem.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using ManagementSystem.Models;
    using ManagementSystem.Data;
    using Microsoft.AspNet.Identity;
    using ManagementSystem.ViewModels;

    [Authorize]
    public class PlansController : Controller
    {
        private readonly IUowData db = new UowData();

        // GET: /Plans/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Plans.All().ToList());
        }

        // GET: /Plans/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var plan = db.Plans.Find((int)id);

            if (plan == null)
            {
                return HttpNotFound();
            }

            return View(plan);
        }

        public ActionResult GetParticipants(string text )
        {
            string textToLower = text.ToLower();
            var usernameToLower = User.Identity.GetUserName().ToLower();
            var result = this.db.AppUsers
                .All()
                .Where(u=> u.UserName.ToLower().Contains(textToLower) && u.UserName.ToLower() != usernameToLower)
                .Select(ParticipantCreateViewModel.FromAppUser);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: /Plans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Plans/Create
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] 
        public ActionResult Create(Plan plan, string[] participants)
        {
            ModelState.Remove("Participants");
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                plan.Owner = db.AppUsers.All().SingleOrDefault(au => au.Id == userId);

                if (participants != null)
                {
                    foreach (var pGuid in participants)
                    {
                        var participant = db.Participants.All().FirstOrDefault(p => p.Id == pGuid);
                        if (participant != null)
                        {
                            plan.Participants.Add(participant);
                        }
                    }
                }

                db.Plans.Add(plan);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = plan.Id });
            }

            return View(plan);
        }

        // GET: /Plans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var plan = db.Plans.Find((int)id);

            if (plan == null)
            {
                return HttpNotFound();
            }

            if (plan.Owner.Id != User.Identity.GetUserId())
            {
                return RedirectToAction("Index");
            }

            return View(plan);
        }

        // POST: /Plans/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Plan plan)
        {
            if (ModelState.IsValid)
            {
                db.Plans.Update(plan);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = plan.Id });
            }
            return View(plan);
        }

        // GET: /Plans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find((int)id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // POST: /Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plan plan = db.Plans.Find(id);
            db.Plans.Delete(plan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
