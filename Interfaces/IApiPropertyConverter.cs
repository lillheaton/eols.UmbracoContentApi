using Umbraco.Core.Models;

namespace EOls.UmbracoContentApi.Interfaces
{
    public interface IApiPropertyConverter
    {
        object Convert(IPublishedProperty property, IPublishedContent owner);
    }
}