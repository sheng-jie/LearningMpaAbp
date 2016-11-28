using Abp.Authorization;
using LearningMpaAbp.Authorization.Roles;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
