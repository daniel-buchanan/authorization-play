using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Principals;
using authorization_play.Core.Principals.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("principal")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalStorage storage;

        public PrincipalController(IPrincipalStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get() => Ok(this.storage.All());

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id) => Ok(this.storage.GetById(id));

        [HttpPost]
        [SwaggerResponse(200, "The grant was added successfully", typeof(PermissionGrant))]
        [SwaggerResponse(400, "The grant could not be added or was invalid", typeof(string))]
        public IActionResult Post([FromBody] Principal principal)
        {
            if (principal == null) return BadRequest();
            this.storage.Add(principal);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse(200, "The grant was removed successfully", typeof(string))]
        public IActionResult Delete(int id)
        {
           this.storage.Remove(id);
            return Ok();
        }

        [HttpPost]
        [Route("{primary}/relations")]
        public IActionResult AddRelation([FromRoute] string id, [FromBody] string relation)
        {
            var parent = CPN.FromValue(id);
            var found = this.storage.Find(parent).FirstOrDefault();

            if (found == null) return NotFound();
            
            var child = CPN.FromValue(relation);
            if (!this.storage.AddRelation(parent, child))
                return NoContent();

            return Created(Url.RouteUrl(new { controller = "Principal", action = "Get", id = found.Id }), found);
        }
    }
}
