using Abp.Web.Mvc.Views;

namespace Call.Web.Views
{
    public abstract class CallWebViewPageBase : CallWebViewPageBase<dynamic>
    {

    }

    public abstract class CallWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CallWebViewPageBase()
        {
            LocalizationSourceName = CallConsts.LocalizationSourceName;
        }
    }
}