using EOls.UmbracoContentApi.Interfaces;
using EOls.UmbracoContentApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOls.UmbracoContentApi.Setup
{
    public static class DocumentTypeExtenders
    {
        private static KeyValuePair<string, IDocumentTypeExtender> Initiate(Type type) =>
            new KeyValuePair<string, IDocumentTypeExtender>(type.Name.ToLowerInvariant(), Activator.CreateInstance(type) as IDocumentTypeExtender);

        public static Dictionary<string, IDocumentTypeExtender> Setup()
        {
            return
                ReflectionHelper.GetAssemblyClassesInheritInterface<IDocumentTypeExtender>()
                .Select(Initiate)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}