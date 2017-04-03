using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.PropertyConverters
{
    [PropertyType(Type = "Umbraco.TinyMCEv3")]
    public class RichTextEditorConverter : IApiPropertyConverter
    {
        public object Convert(IPublishedProperty property, IPublishedContent owner, ContentSerializer serializer, UmbracoHelper umbraco)
        {
            var htmlString = property.Value as HtmlString;
            return htmlString != null ? htmlString.ToHtmlString() : string.Empty;
        }
    }
}