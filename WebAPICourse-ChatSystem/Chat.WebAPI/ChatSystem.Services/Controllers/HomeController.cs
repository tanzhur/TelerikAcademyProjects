using System.Web.Mvc;

namespace ChatSystem.Services.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}