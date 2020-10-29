using System.Collections.Generic;
using System.Linq;
using System.Web;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("grant")]
    public class GrantController : ControllerBase
    {
        private readonly IPermissionGrantFinder grantFinder;
        private readonly IPermissionGrantManager manager;

        public GrantController(IPermissionGrantFinder grantFinder,
            IPermissionGrantManager manager)
        {
            this.grantFinder = grantFinder;
            this.manager = manager;
        }

        [HttpGet]
        [Route("{principal}")]
        [SwaggerResponse(200, "All found grants for the given Principal and or Schema", typeof(PermissionGrant[]))]
        [SwaggerResponse(404, "No grants were found for the given Principal or Schema", typeof(string))]
        public IActionResult Get([FromRoute] string principal, [FromQuery] string schema = null)
        {
            IEnumerable<PermissionGrant> results;
            var principalRn = CRN.FromValue(HttpUtility.UrlDecode(principal));
            if (schema == null) results = this.grantFinder.Find(principalRn);
            else
            {
                var schemaRn = DataSchema.FromValue(HttpUtility.UrlDecode(schema));
                results = this.grantFinder.Find(principalRn, schemaRn);
            }

            if (!results.Any()) return NotFound();
            return Ok(results);
        }

        [HttpPost]
        [SwaggerResponse(200, "The grant was added successfully", typeof(PermissionGrant))]
        [SwaggerResponse(400, "The grant could not be added or was invalid", typeof(string))]
        public IActionResult Post(PermissionGrant grant)
        {
            var valid = this.manager.Add(grant);
            if(!valid) return BadRequest("Provided grant is invalid");
            return Ok(grant);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse(204, "The grant was removed successfully")]
        public IActionResult Delete(int id)
        {
            this.manager.Remove(id);
            return NoContent();
        }
    }
}
