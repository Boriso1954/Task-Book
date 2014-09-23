using System.Web.Mvc;

namespace TaskBook.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}