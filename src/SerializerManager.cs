using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.Util;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace EOls.UmbracoContentApi
{
    public static class SerializerManager
    {
        private static Dictionary<string, IApiPropertyConverter> _propertyConverters;
        private static Dictionary<string, IDocumentTypeExtender> _extenders;
        private static Dictionary<string, DocumentTypeSettingsAttribute> _documentSettings;

        private static IContractResolver GetDefaultContractResolver() => new CamelCasePropertyNamesContractResolver();

        private static Dictionary<string, T> GetAssemblyClasses<T>() =>
            ReflectionHelper.GetAssemblyClassesInheritAttribute<T>()
            .Select(x => new KeyValuePair<string, T>(x.Name.ToLowerInvariant(), (T)x.GetCustomAttributes(typeof(T), false).First()))
            .ToDictionary(x => x.Key, x => x.Value);

        public static IContractResolver ContractResolver { get; set; }

        public static Dictionary<string, IApiPropertyConverter> PropertyConverters
        {
            get { return _propertyConverters = (_propertyConverters == null ? Setup.PropertyConverters.Setup() : _propertyConverters); }
        }

        public static Dictionary<string, IDocumentTypeExtender> Extenders
        {
            get { return _extenders = (_extenders == null ? Setup.DocumentTypeExtenders.Setup() : _extenders); }
        }

        public static Dictionary<string, DocumentTypeSettingsAttribute> DocumentSettings
        {
            get { return _documentSettings = (_documentSettings == null ? GetAssemblyClasses<DocumentTypeSettingsAttribute>() : _documentSettings); }
        }

        static SerializerManager()
        {
            ContractResolver = GetDefaultContractResolver();
            _propertyConverters = Setup.PropertyConverters.Setup();
            _extenders = Setup.DocumentTypeExtenders.Setup();
            _documentSettings = GetAssemblyClasses<DocumentTypeSettingsAttribute>();
        }
    }
}
