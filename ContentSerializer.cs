using System;
using System.Collections.Generic;
using System.Linq;

using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.PropertyConverters;
using EOls.UmbracoContentApi.Util;

using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace EOls.UmbracoContentApi
{
    public sealed class ContentSerializer
    {
        private static object _lock = new object();
        private static ContentSerializer _instance;
        public static ContentSerializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ContentSerializer();
                        }
                    }
                }

                return _instance;
            }
        }

        private Type[] eolsConverters = { typeof(ContentPickerConverter), typeof(RichTextEditorConverter) };

        private Dictionary<string, IApiPropertyConverter> PropertyConverters;
        private Dictionary<string, IDocumentTypeExtender> Extenders;
        
        private ContentSerializer()
        {
            Setup();
        }

        private void Setup()
        {
            // Get all property converters
            PropertyConverters = ReflectionHelper.GetAssemblyClassesInheritInterface<IApiPropertyConverter>()
                .Where(s => !eolsConverters.Contains(s) && s.GetCustomAttributes(false).Any(a => a is PropertyTypeAttribute))
                .Select(
                    s =>
                    {
                        var attribute = s.GetCustomAttributes(false).First(a => a is PropertyTypeAttribute) as PropertyTypeAttribute;
                        return new KeyValuePair<string, IApiPropertyConverter>(attribute.Type, Activator.CreateInstance(s) as IApiPropertyConverter);
                    }).ToDictionary(s => s.Key, s => s.Value);

            // Allow users to override the default converters
            foreach (var eolsConverter in eolsConverters)
            {
                var attribute = eolsConverter.GetCustomAttributes(false).First(a => a is PropertyTypeAttribute) as PropertyTypeAttribute;
                if (!PropertyConverters.ContainsKey(attribute.Type))
                {
                    PropertyConverters.Add(attribute.Type, Activator.CreateInstance(eolsConverter) as IApiPropertyConverter);
                }
            }

            // Get all document extenders
            Extenders =
                ReflectionHelper.GetAssemblyClassesInheritInterface<IDocumentTypeExtender>()
                    .Select(
                        s =>
                        new KeyValuePair<string, IDocumentTypeExtender>(s.Name, Activator.CreateInstance(s) as IDocumentTypeExtender))
                    .ToDictionary(s => s.Key, s => s.Value);
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

        private Dictionary<string, object> ConvertContent(IPublishedContent content)
        {
            Dictionary<string, object> extendedContent = new Dictionary<string, object>();
            
            // Document extenders located and used to extend the model
            if (Extenders.Any(s => s.Key.StartsWith(content.DocumentTypeAlias, StringComparison.InvariantCultureIgnoreCase)))
            {
                extendedContent =
                    Extenders.First(
                        s => s.Key.StartsWith(content.DocumentTypeAlias, StringComparison.InvariantCultureIgnoreCase))
                        .Value.Extend(content);
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
            string editorAlias = contentType.GetPropertyType(property.PropertyTypeAlias).PropertyEditorAlias;
            
            // Property converter exist
            if (PropertyConverters.ContainsKey(editorAlias))
            {
                return PropertyConverters[editorAlias].Convert(property, owner);
            }

            return property.Value;
        }
    }
}