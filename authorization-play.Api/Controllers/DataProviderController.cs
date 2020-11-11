using System.Web;
using authorization_play.Core;
using authorization_play.Core.DataProviders;
using authorization_play.Core.DataProviders.Models;
using authorization_play.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("data-provider")]
    public class DataProviderController : ControllerBase
    {
        private readonly IDataProviderStorage storage;

        public DataProviderController(IDataProviderStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.storage.All());
        }

        [Route("{identifier}/sources")]
        public IActionResult GetSources(string identifier)
        {
            identifier = HttpUtility.UrlDecode(identifier);
            return Ok(this.storage.GetSources(identifier));
        }

        [HttpPost]
        [Route("{identifier}/sources")]
        public IActionResult AddSource(string identifier, [FromBody] DataSource source)
        {
            this.storage.AddSource(source);
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] DataProvider schema)
        {
            this.storage.Add(schema);
            return Ok();
        }

        [HttpDelete]
        [Route("{identifier}")]
        [SwaggerResponse(200, "The grant was removed successfully", typeof(string))]
        public IActionResult Delete(string identifier)
        {
            identifier = HttpUtility.UrlDecode(identifier);
            this.storage.Remove(identifier);
            return Ok();
        }
    }
}
