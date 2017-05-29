using System;
using System.Linq;
using System.Reflection;

namespace EOls.UmbracoContentApi.Util
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Gets all classes that inherit T in all currentDomainAssemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type[] GetAssemblyClassesInheritInterface<T>()
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(s => typeof(T).IsAssignableFrom(s) && s.IsClass)
                .ToArray();
        }

        /// <summary>
        /// Get all classes that inherit T in assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetAssemblyClassesInheritInterface<T>(Assembly assembly)
        {
            return 
                assembly
                .GetTypes()
                .Where(s => typeof(T)
                .IsAssignableFrom(s) && s.IsClass)
                .ToArray();
        }

        /// <summary>
        /// Get all classes that inherit T in assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Type[] GetAssemblyClassesInheritInterface<T>(Assembly[] assemblies)
        {
            return
                assemblies
                .SelectMany(s => s.GetTypes())
                .Where(s => typeof(T).IsAssignableFrom(s) && s.IsClass)
                .ToArray();
        }
    }
}