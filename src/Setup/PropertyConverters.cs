using EOls.UmbracoContentApi.Attributes;
using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EOls.UmbracoContentApi.Setup
{
    public static class PropertyConverters
    {
        private static IEnumerable<Type> AssemblyPropertyConverters(Func<Assembly, bool> isAssemblies)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(isAssemblies);
            return ReflectionHelper.GetAssemblyClassesInheritInterface<IApiPropertyConverter>(assemblies);
        }

        private static KeyValuePair<string, IApiPropertyConverter> Instantiate(Type converter)
        {
            var attr = converter.GetCustomAttributes(typeof(PropertyTypeAttribute), false).FirstOrDefault() as PropertyTypeAttribute;
            if (attr == null) throw new Exception("Converter needs to have the PropertyTypeAttribute");
            return new KeyValuePair<string, IApiPropertyConverter>(attr.Type.ToLowerInvariant(), Activator.CreateInstance(converter) as IApiPropertyConverter);
        }


        public static Dictionary<string, IApiPropertyConverter> Setup()
        {
            // EOls.UmbracoContentApi IApiPropertyConverter 
            IEnumerable<KeyValuePair<string, IApiPropertyConverter>> localAssemblyTypes = 
                AssemblyPropertyConverters(s => s == typeof(PropertyConverters).Assembly)
                .Select(Instantiate);

            // All namespace IApiPropertyConverter except EOls.UmbracoContentApi
            IEnumerable<KeyValuePair<string, IApiPropertyConverter>> assembliesTypes = 
                AssemblyPropertyConverters(s => s != typeof(PropertyConverters).Assembly)
                .Select(Instantiate);

            Func<KeyValuePair<string, IApiPropertyConverter>, bool> isNotPartOfAssembliesTypes = x => assembliesTypes.Any(y => y.Key != x.Key);

            return 
                assembliesTypes
                .Union(
                    localAssemblyTypes.Where(isNotPartOfAssembliesTypes)
                ).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
