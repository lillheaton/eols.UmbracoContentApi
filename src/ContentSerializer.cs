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

        private static bool KeyStartsWith<T>(KeyValuePair<string, T> pair, string s) => pair.Key.StartsWith(s, StringComparison.InvariantCultureIgnoreCase);
        
        private static bool ShouldHideDocument(IPublishedContent content)
        {
            // Check if there is any settings for the specific document type
            var docTypeSettings = SerializerManager.DocumentSettings.GetValueOrDefault(x => KeyStartsWith(x, content.DocumentTypeAlias));
            return docTypeSettings != null && docTypeSettings.Hide;
        }

        private static bool ShouldHideProperty(IPublishedContent content, IPublishedProperty property)
        {
            var docTypeSettings = SerializerManager.DocumentSettings.GetValueOrDefault(x => KeyStartsWith(x, content.DocumentTypeAlias));
            return docTypeSettings != null && docTypeSettings.HideProperties.Contains(property.PropertyTypeAlias);
        }

        private Dictionary<string, object> ExtendedContent(IPublishedContent content)
        {
            IDocumentTypeExtender docExtender = SerializerManager.Extenders.GetValueOrDefault(x => KeyStartsWith(x, content.DocumentTypeAlias));

            if (docExtender != null)
                return docExtender.Extend(content, this, this._umbraco);

            return new Dictionary<string, object>();
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


        public object Serialize(IPublishedContent content)
        {
            if (ShouldHideDocument(content))
                return null;

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
            if (ShouldHideDocument(content))
                return null;

            // Continue and convert all the properties and Union the extended dictionary
            return
                content
                    .Properties
                    .Where(p => !ShouldHideProperty(content, p))
                    .OrderBy(p => p.PropertyTypeAlias)
                    .ToDictionary(
                        s => s.PropertyTypeAlias,
                        s => ConvertProperty(s, content, content.ContentType))
                    .Union(ExtendedContent(content))
                    .ToDictionary(s => s.Key, s => s.Value);
        }
    }
}