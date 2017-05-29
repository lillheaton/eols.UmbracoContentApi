using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.PropertyConverters
{
    [PropertyType(Type = "Umbraco.ContentPickerAlias")]
    public class ContentPickerConverter : IApiPropertyConverter
    {
        public object Convert(IPublishedProperty property, IPublishedContent owner, ContentSerializer serializer, UmbracoHelper umbraco)
        {
            if (property.Value is IPublishedContent)
                return serializer.Serialize(property.Value as IPublishedContent);

            IPublishedContent content = umbraco.TypedContent(property.Value);
            if (content == null)
                return null;

            return serializer.Serialize(content);
        }
    }
}