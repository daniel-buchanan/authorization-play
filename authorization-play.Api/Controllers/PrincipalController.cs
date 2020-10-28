using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using authorization_play.Persistance;
using Microsoft.AspNetCore.Mvc;
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
        [SwaggerResponse(200, "All found grants for the given Principal and or Schema", typeof(PermissionGrant[]))]
        [SwaggerResponse(404, "No grants were found for the given Principal or Schema", typeof(string))]
        public IActionResult Get()
        {
            var found = this.context.Principals.ToList().Select(p => CRN.FromValue(p.CanonicalName));
            return Ok(found);
        }

        [HttpPost]
        [SwaggerResponse(200, "The grant was added successfully", typeof(PermissionGrant))]
        [SwaggerResponse(400, "The grant could not be added or was invalid", typeof(string))]
        public IActionResult Post([FromBody] string identifier)
        {
            var principalIdentifier = CRN.FromValue(HttpUtility.UrlDecode(identifier));
            var toAdd = new Persistance.Models.Principal()
            {
                CanonicalName = principalIdentifier.ToString()
            };
            this.context.Add(toAdd);
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
    }
}
