using EOls.UmbracoContentApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace EOls.UmbracoContentApi
{
    public sealed class ContentSerializer
    {
        private UmbracoHelper _umbraco;

        public ContentSerializer(UmbracoHelper umbraco)
        {
            this._umbraco = umbraco;
        }

        public object Serialize(IPublishedContent content)
        {
            return new
            {
                ContentId = content.Id,
                Name = content.Name,
                Url = content.Url,
                ContentTypeId = content.DocumentTypeId,
                ContentTypeAlias = content.DocumentTypeAlias,
                Content = ConvertContent(content)
            };
        }

        public Dictionary<string, object> ConvertContent(IPublishedContent content)
        {
            Dictionary<string, object> extendedContent = new Dictionary<string, object>();

            // Document extenders located and used to extend the model
            var extender = 
                    SerializerManager.Extenders
                    .Where(s => s.Key.StartsWith(content.DocumentTypeAlias, StringComparison.InvariantCultureIgnoreCase))
                    .Select(s => s as KeyValuePair<string, IDocumentTypeExtender>?)
                    .FirstOrDefault();

            if (extender != null)
            {
                extendedContent = extender.Value.Value.Extend(content, this, this._umbraco);
            }

            // Continue and convert all the properties and Union the extended dictionary
            return
                content.Properties.ToDictionary(
                    s => s.PropertyTypeAlias,
                    s => ConvertProperty(s, content, content.ContentType))
                    .Union(extendedContent)
                    .ToDictionary(s => s.Key, s => s.Value);
        }

        private object ConvertProperty(IPublishedProperty property, IPublishedContent owner, PublishedContentType contentType)
        {
            string editorAlias = contentType.GetPropertyType(property.PropertyTypeAlias).PropertyEditorAlias.ToLowerInvariant();

            // Property converter exist
            if (SerializerManager.PropertyConverters.ContainsKey(editorAlias))
            {
                return SerializerManager.PropertyConverters[editorAlias].Convert(property, owner, this, this._umbraco);
            }

            return property.Value;
        }
    }
}