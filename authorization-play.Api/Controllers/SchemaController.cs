using System.Linq;
using System.Web;
using authorization_play.Core.Models;
using authorization_play.Persistance;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("schema")]
    public class SchemaController : ControllerBase
    {
        private readonly AuthorizationPlayContext context;

        public SchemaController(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var found = this.context.Schemas.ToList().Select(s => CSN.FromValue(s.CanonicalName));
            return Ok(found);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Persistance.Models.Schema schema)
        {
            var toAdd = new Persistance.Models.Schema()
            {
                CanonicalName = schema.CanonicalName,
                Description = schema.Description,
                DisplayName = schema.DisplayName
            };
            this.context.Add(toAdd);
            this.context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse(200, "The grant was removed successfully", typeof(string))]
        public IActionResult Delete(string identifier)
        {
            identifier = HttpUtility.UrlDecode(identifier);
            var found = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == identifier);
            if (found == null) return NotFound();
            this.context.Remove(found);
            this.context.SaveChanges();
            return Ok();
        }
    }
}
