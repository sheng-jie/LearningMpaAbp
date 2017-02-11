using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using LearningMpaAbp.Extensions;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Users;
using Microsoft.AspNet.Identity;

namespace LearningMpaAbp
{
    /// <summary>
    ///     Derive your application services from this class.
    /// </summary>
    public abstract class LearningMpaAbpAppServiceBase : ApplicationService
    {
        protected LearningMpaAbpAppServiceBase()
        {
            LocalizationSourceName = LearningMpaAbpConsts.LocalizationSourceName;
        }

        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        //隐藏父类的AbpSession
        //public new IAbpSessionExtension AbpSession { get; set; }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
                throw new ApplicationException("There is no current user!");

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}