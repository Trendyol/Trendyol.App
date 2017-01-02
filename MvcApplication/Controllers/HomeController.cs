using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [HttpGet, Route("")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Sample");
        }
    }
}