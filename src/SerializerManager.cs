using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.PropertyConverters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOls.UmbracoContentApi
{
    public static class SerializerManager
    {
        public static IContractResolver ContractResolver { get; set; }
        public static Dictionary<string, IApiPropertyConverter> PropertyConverters { get; private set; }
        public static Dictionary<string, IDocumentTypeExtender> Extenders { get; private set; }        

        private static void AddPrebuiltConverters()
        {
            AddConverter(new ContentPickerConverter());
            AddConverter(new RichTextEditorConverter());
        }

        public static void Initialize()
        {
            // Set Default to camelcase
            ContractResolver = new CamelCasePropertyNamesContractResolver();

            PropertyConverters = PropertyConverters ?? new Dictionary<string, IApiPropertyConverter>();
            Extenders = Extenders ?? new Dictionary<string, IDocumentTypeExtender>();

            AddPrebuiltConverters();
        }

        public static void AddConverter(IApiPropertyConverter converter)
        {
            if (PropertyConverters == null)
                Initialize();

            var attribute = converter.GetType().GetCustomAttributes(false).First(a => a is PropertyTypeAttribute) as PropertyTypeAttribute;
            if (attribute == null)
                throw new Exception("Converter needs to have the PropertyTypeAttribute");

            if (PropertyConverters.Any(s => s.Key == attribute.Type))
                return;

            PropertyConverters.Add(attribute.Type, converter);
        }

        public static void AddExtender(IDocumentTypeExtender extender)
        {
            if (Extenders == null)
                Initialize();

            if (Extenders.Any(s => s.Key == extender.GetType().Name))
                return;

            Extenders.Add(extender.GetType().Name, extender);
        }
    }
}
