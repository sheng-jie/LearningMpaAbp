using System.Web.Mvc;

namespace LearningMpaAbp.Web.Controllers
{
    public class AboutController : LearningMpaAbpControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}