using System.Linq;
using System.Web;
using authorization_play.Core.Resources.Models;
using authorization_play.Persistance;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("resource/actions")]
    public class ResourceActionController : ControllerBase
    {
        private readonly AuthorizationPlayContext context;

        public ResourceActionController(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [SwaggerResponse(200, "Any resources that are registered", typeof(Resource[]))]
        public IActionResult Get() => Ok(this.context.Actions.ToList().Select(a => ResourceAction.FromValue(a.CanonicalName)));

        [HttpPost]
        [SwaggerResponse(200, "The Resource was added successfully", typeof(Resource))]
        public IActionResult Post([FromBody] ResourceAction resource)
        {
            var toAdd = new Persistance.Models.Action()
            {
                Category = resource.Category,
                Name = resource.Action,
                CanonicalName = resource.ToString()
            };
            this.context.Actions.Add(toAdd);
            this.context.SaveChanges();
            return Ok(resource);
        }

        [HttpDelete]
        [Route("{resource}")]
        [SwaggerResponse(200, "The Resource was removed successfully", typeof(Resource))]
        [SwaggerResponse(404, "The Resource could not be found", typeof(string))]
        public IActionResult Delete(string resource)
        {
            var actionName = ResourceAction.FromValue(HttpUtility.UrlDecode(resource));

            var found = this.context.Actions.FirstOrDefault(a => a.CanonicalName == actionName);

            if (found == null) return NotFound();

            this.context.Actions.Remove(found);

            return Ok(resource);
        }
    }
}
