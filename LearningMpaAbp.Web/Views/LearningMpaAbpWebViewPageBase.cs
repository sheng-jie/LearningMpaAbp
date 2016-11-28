using Abp.Web.Mvc.Views;

namespace LearningMpaAbp.Web.Views
{
    public abstract class LearningMpaAbpWebViewPageBase : LearningMpaAbpWebViewPageBase<dynamic>
    {

    }

    public abstract class LearningMpaAbpWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected LearningMpaAbpWebViewPageBase()
        {
            LocalizationSourceName = LearningMpaAbpConsts.LocalizationSourceName;
        }
    }
}