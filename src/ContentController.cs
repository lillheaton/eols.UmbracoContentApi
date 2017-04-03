using Newtonsoft.Json;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace EOls.UmbracoContentApi
{
    public class ContentController : UmbracoApiController
    {
        private ContentSerializer _serializer;
        private JsonSerializerSettings _jsonSettings;

        public ContentController()
        {
            this._serializer = new ContentSerializer(this.Umbraco);

            this._jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            if (SerializerManager.ContractResolver != null)
                this._jsonSettings.ContractResolver = SerializerManager.ContractResolver;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var content = Umbraco.TypedContent(id);
            if (content == null)
            {
                return BadRequest("No content with that ID");
            }

            return Json(this._serializer.Serialize(content), this._jsonSettings);
        }
    }
}