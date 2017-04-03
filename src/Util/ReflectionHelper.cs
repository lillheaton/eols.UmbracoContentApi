using System;
using System.Linq;

namespace EOls.UmbracoContentApi.Util
{
    public class ReflectionHelper
    {
        public static Type[] GetAssemblyClassesInheritInterface<T>()
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(s => typeof(T).IsAssignableFrom(s) && s.IsClass)
                    .ToArray();
        }
    }
}