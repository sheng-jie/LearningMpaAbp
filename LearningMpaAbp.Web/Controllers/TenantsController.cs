using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LearningMpaAbp.Authorization;
using LearningMpaAbp.MultiTenancy;

namespace LearningMpaAbp.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : LearningMpaAbpControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}