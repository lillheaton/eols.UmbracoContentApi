using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EOls.UmbracoContentApi.Util
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get all classes that inherit T in assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritInterface<T>(Assembly assembly)
        {
            return
                assembly
                .GetTypes()
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x));
        }

        /// <summary>
        /// Get all classes that inherit T in assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritInterface<T>(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(GetAssemblyClassesInheritInterface<T>);
        }

        /// <summary>
        /// Gets all classes that inherit T in all currentDomainAssemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritInterface<T>()
        {
            return GetAssemblyClassesInheritInterface<T>(AppDomain.CurrentDomain.GetAssemblies());
        }

        
        /// <summary>
        /// Get all classes that inherit attribute T in assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritAttribute<T>(Assembly assembly)
        {
            return 
                assembly
                .GetTypes()
                .Where(x => x.IsClass && x.GetCustomAttributes(typeof(T)).Any());
        }
        /// <summary>
        /// Get all classes that inherit attribute T in all currentDomainAssemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritAttribute<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(GetAssemblyClassesInheritAttribute<T>);
        }
    }
}