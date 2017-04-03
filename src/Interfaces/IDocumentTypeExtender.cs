using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace EOls.UmbracoContentApi.Interfaces
{
    public interface IDocumentTypeExtender
    {
        Dictionary<string, object> Extend(IPublishedContent content, ContentSerializer serializer, UmbracoHelper umbraco);
    }
}