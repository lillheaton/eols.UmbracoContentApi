using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.Interfaces
{
    public interface IApiPropertyConverter
    {
        object Convert(IPublishedProperty property, IPublishedContent owner, ContentSerializer serializer, UmbracoHelper umbraco);
    }
}