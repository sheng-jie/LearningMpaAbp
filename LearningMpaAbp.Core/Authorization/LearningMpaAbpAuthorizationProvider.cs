using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace LearningMpaAbp.Authorization
{
    public class LearningMpaAbpAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages);
            if (pages == null)
            {
                pages = context.CreatePermission(PermissionNames.Pages, L("Pages"));
            }

            var users = pages.CreateChildPermission(PermissionNames.Pages_Users, L("Users"));

            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            //Tasks
            var tasks = pages.CreateChildPermission(PermissionNames.Pages_Tasks, L("Tasks"));
            tasks.CreateChildPermission(PermissionNames.Pages_Tasks_AssignPerson, L("AssignTaskToPerson"));
            tasks.CreateChildPermission(PermissionNames.Pages_Tasks_Delete, L("DeleteTask"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LearningMpaAbpConsts.LocalizationSourceName);
        }
    }
}
