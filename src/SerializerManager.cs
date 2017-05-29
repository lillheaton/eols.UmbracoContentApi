using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.PropertyConverters;
using EOls.UmbracoContentApi.Util;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOls.UmbracoContentApi
{
    public static class SerializerManager
    {
        private static Dictionary<string, IApiPropertyConverter> _propertyConverters;
        private static Dictionary<string, IDocumentTypeExtender> _extenders;

        public static IContractResolver ContractResolver { get; set; }
        public static Dictionary<string, IApiPropertyConverter> PropertyConverters
        {
            get { return _propertyConverters = (_propertyConverters == null ? SetupPropertyConverters() : _propertyConverters); }
        }
        public static Dictionary<string, IDocumentTypeExtender> Extenders
        {
            get { return _extenders = (_extenders == null ? SetupDocumentExtenders() : _extenders); }
        }

        static SerializerManager()
        {
            ContractResolver = GetDefaultContractResolver();
            _propertyConverters = SetupPropertyConverters();
            _extenders = SetupDocumentExtenders();
        }

        private static IContractResolver GetDefaultContractResolver()
        {
            return new CamelCasePropertyNamesContractResolver();
        }

        private static Dictionary<string, IApiPropertyConverter> InstantiateConverter(Type[] ConverterTypes)
        {
            return ConverterTypes.Select(s =>
            {
                var attr = s.GetCustomAttributes(typeof(PropertyTypeAttribute), false).FirstOrDefault() as PropertyTypeAttribute;
                if(attr == null)
                    throw new Exception("Converter needs to have the PropertyTypeAttribute");

                return new KeyValuePair<string, IApiPropertyConverter>(attr.Type.ToLowerInvariant(), Activator.CreateInstance(s) as IApiPropertyConverter);
            }).ToDictionary(s => s.Key, s => s.Value);
        }

        private static Dictionary<string, IApiPropertyConverter> SetupPropertyConverters()
        {
            // EOls.UmbracoContentApi IApiPropertyConverter types
            Type[] localTypes =
                ReflectionHelper.GetAssemblyClassesInheritInterface<IApiPropertyConverter>(typeof(SerializerManager).Assembly);

            // All assemblies except EOls.UmbracoContentApi
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(s => s != typeof(SerializerManager).Assembly).ToArray(); 

            // All namespace IApiPropertyConverter types
            Type[] currentDomainTypes = ReflectionHelper.GetAssemblyClassesInheritInterface<IApiPropertyConverter>(assemblies);

            Dictionary<string, IApiPropertyConverter> localConverters = InstantiateConverter(localTypes);
            Dictionary<string, IApiPropertyConverter> currentDomainConverters = InstantiateConverter(currentDomainTypes);

            // Allow users to override EOls.UmbracoContentApi converters
            return 
                currentDomainConverters
                .Concat(
                    localConverters
                    .Where(s => !currentDomainConverters.ContainsKey(s.Key)))
                .ToDictionary(s => s.Key, s => s.Value);
        }

        private static Dictionary<string, IDocumentTypeExtender> SetupDocumentExtenders()
        {
            Type[] types = ReflectionHelper.GetAssemblyClassesInheritInterface<IDocumentTypeExtender>();

            return 
                types
                .Select(s => new KeyValuePair<string, IDocumentTypeExtender>(s.Name.ToLowerInvariant(), Activator.CreateInstance(s) as IDocumentTypeExtender))
                .ToDictionary(s => s.Key, s => s.Value);
        }
    }
}
