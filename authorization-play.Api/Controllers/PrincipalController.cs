using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Principals;
using authorization_play.Core.Principals.Models;
using authorization_play.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("principal")]
    public class PrincipalController : ControllerBase
    {
        private readonly AuthorizationPlayContext context;
        private readonly IPrincipalStorage storage;

        public PrincipalController(AuthorizationPlayContext context,
            IPrincipalStorage storage)
        {
            this.context = context;
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
            var found = this.context.Principals.FirstOrDefault(p => p.PrincipalId == id);
            if (found == null) return NotFound();
            this.context.Principals.Remove(found);
            this.context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("{primary}/relations")]
        public IActionResult AddRelation([FromRoute] string id, [FromBody] string relation)
        {
            var found = this.context.PrincipalRelations
                .Include(r => r.Parent)
                .Include(r => r.Child)
                .FirstOrDefault(r => r.Parent.CanonicalName == id && r.Child.CanonicalName == relation);

            if (found != null) return NoContent();

            var primary = this.context.Principals.FirstOrDefault(p => p.CanonicalName == id);
            var secondary = this.context.Principals.FirstOrDefault(p => p.CanonicalName == relation);

            var toAdd = new Persistance.Models.PrincipalRelation()
            {
                Parent = primary,
                Child = secondary
            };

            this.context.Add(toAdd);
            this.context.SaveChanges();

            return Created(Url.RouteUrl(new { controller = "Principal", action = "Get", id = primary.PrincipalId }), primary);
        }
    }
}
