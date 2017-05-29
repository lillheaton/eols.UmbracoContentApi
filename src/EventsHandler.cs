using Umbraco.Core;

namespace EOls.UmbracoContentApi
{
    public class EventsHandler : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationInitialized(umbracoApplication, applicationContext);
        }
    }
}