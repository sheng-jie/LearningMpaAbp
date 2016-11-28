using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace LearningMpaAbp.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : LearningMpaAbpControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}