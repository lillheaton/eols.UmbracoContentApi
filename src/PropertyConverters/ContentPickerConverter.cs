﻿using EOls.UmbracoContentApi.Attributes;
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
            return serializer.Serialize(umbraco.TypedContent(property.Value));
        }
    }
}