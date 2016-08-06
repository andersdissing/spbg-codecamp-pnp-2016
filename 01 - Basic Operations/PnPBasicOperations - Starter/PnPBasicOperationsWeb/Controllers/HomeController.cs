using System.Web.Mvc;

namespace PnPBasicOperationsWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}
