using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
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

        public PrincipalController(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var found = this.context.Principals
                .Include(p => p.ChildRelations)
                .ThenInclude(r => r.Child)
                .ToList()
                .Select(p => new Principal()
                {
                    Identifier = CRN.FromValue(p.CanonicalName),
                    Children = p.ChildRelations.Select(r => CRN.FromValue(r.Child.CanonicalName)).ToList()
                });
            return Ok(found);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var found = this.context.Principals
                .Include(p => p.ChildRelations)
                .ThenInclude(r => r.Child)
                .FirstOrDefault(p => p.PrincipalId == id);

            if (found == null) return NotFound();

            var toReturn = new Principal()
            {
                Identifier = CRN.FromValue(found.CanonicalName),
                Children = found.ChildRelations.Select(r => CRN.FromValue(r.Child.CanonicalName)).ToList()
            };

            return Ok(toReturn);
        }

        [HttpPost]
        [SwaggerResponse(200, "The grant was added successfully", typeof(PermissionGrant))]
        [SwaggerResponse(400, "The grant could not be added or was invalid", typeof(string))]
        public IActionResult Post([FromBody] Principal principal)
        {
            if (principal == null) return BadRequest();

            var toAdd = new Persistance.Models.Principal()
            {
                CanonicalName = principal.Identifier
            };

            this.context.Add(toAdd);

            foreach (var child in principal.Children)
            {
                var found = this.context.Principals.FirstOrDefault(p => p.CanonicalName == child.Value);
                if (found == null) continue;
                this.context.Add(new Persistance.Models.PrincipalRelation()
                {
                    Parent = toAdd,
                    Child = found
                });
            }

            this.context.SaveChanges();

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
