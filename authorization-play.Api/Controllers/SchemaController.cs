using System.Web;
using authorization_play.Core;
using authorization_play.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("schema")]
    public class SchemaController : ControllerBase
    {
        private readonly IDataSchemaStorage storage;

        public SchemaController(IDataSchemaStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.storage.All());
        }

        [HttpPost]
        public IActionResult Post([FromBody] DataSchema schema)
        {
            this.storage.Add(schema);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse(200, "The grant was removed successfully", typeof(string))]
        public IActionResult Delete(string identifier)
        {
            identifier = HttpUtility.UrlDecode(identifier);
            this.storage.Remove(CSN.FromValue(identifier));
            return Ok();
        }
    }
}
