using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.PropertyConverters
{
    [PropertyType(Type = "Umbraco.ContentPickerAlias")]
    public class ContentPickerConverter : IApiPropertyConverter
    {
        public object Convert(IPublishedProperty property, IPublishedContent owner)
        {
            return ContentSerializer.Instance.Serialize(new UmbracoHelper(UmbracoContext.Current).TypedContent(property.Value));
        }
    }
}