using System.Linq;
using System.Web;
using authorization_play.Core.Models;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceStorage storage;

        public ResourceController(IResourceStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        [SwaggerResponse(200, "Any resources that are registered", typeof(Resource[]))]
        public IActionResult Get() => Ok(this.storage.All());

        [HttpPost]
        [SwaggerResponse(200, "The Resource was added successfully", typeof(Resource))]
        public IActionResult Post([FromBody] Resource resource)
        {
            this.storage.Add(resource);
            return Ok(resource);
        }

        [HttpDelete]
        [Route("{resource}")]
        [SwaggerResponse(200, "The Resource was removed successfully", typeof(Resource))]
        [SwaggerResponse(404, "The Resource could not be found", typeof(string))]
        public IActionResult Delete(string resource)
        {
            var resourceName = CRN.FromValue(HttpUtility.UrlDecode(resource));

            var found = this.storage.FindByIdentifier(resourceName).FirstOrDefault();

            if (found == null) return NotFound();

            this.storage.Remove(found);

            return Ok(resource);
        }
    }
}
