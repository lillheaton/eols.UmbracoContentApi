using System.Collections.Generic;
using Umbraco.Core.Models;

namespace EOls.UmbracoContentApi.Interfaces
{
    public interface IDocumentTypeExtender
    {
        Dictionary<string, object> Extend(IPublishedContent content);
    }
}