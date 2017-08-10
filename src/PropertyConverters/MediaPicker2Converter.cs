using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.PropertyConverters
{
    [PropertyType(Type = "Umbraco.MediaPicker2")]
    public class MediaPicker2Converter : IApiPropertyConverter
    {
        public object Convert(IPublishedProperty property, IPublishedContent owner, ContentSerializer serializer, UmbracoHelper umbraco)
        {
            if (!property.HasValue)
                return null;

            if (property.Value is IPublishedContent)
            {
                return ((IPublishedContent)property.Value).Url;
            }
            else if (property.Value is IEnumerable<IPublishedContent>)
            {
                return ((IEnumerable<IPublishedContent>)property.Value).Select(s => s.Url).ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
