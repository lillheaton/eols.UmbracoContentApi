using System.Web;

using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;

using Umbraco.Core.Models;

namespace EOls.UmbracoContentApi.PropertyConverters
{
    [PropertyType(Type = "Umbraco.TinyMCEv3")]
    public class RichTextEditorConverter : IApiPropertyConverter
    {
        public object Convert(IPublishedProperty property, IPublishedContent owner)
        {
            var htmlString = property.Value as HtmlString;
            return htmlString != null ? htmlString.ToHtmlString() : string.Empty;
        }
    }
}