using System.Web.Http;
using Newtonsoft.Json;
using Umbraco.Web.WebApi;

namespace EOls.UmbracoContentApi
{
    public class ContentController : UmbracoApiController
    {
        public IHttpActionResult Get(int id)
        {
            var content = Umbraco.TypedContent(id);
            if (content == null)
            {
                return BadRequest("No content with that ID");
            }

            return Json(
                ContentSerializer.Instance.Serialize(content),
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}