using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using LearningMpaAbp.Extensions;
using Microsoft.AspNet.Identity;

namespace LearningMpaAbp.Web.Controllers
{
    /// <summary>
    ///     Derive all Controllers from this class.
    /// </summary>
    public abstract class LearningMpaAbpControllerBase : AbpController
    {
        protected LearningMpaAbpControllerBase()
        {
            LocalizationSourceName = LearningMpaAbpConsts.LocalizationSourceName;
        }

        //隐藏父类的AbpSession
        //public new IAbpSessionExtension AbpSession { get; set; }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}