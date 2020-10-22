using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.Api.Controllers
{
    [ApiController]
    [Route("ticket")]
    public class TicketsController : ControllerBase
    {
        private const string Secret = "secret";
        private readonly IPermissionTicketManager manager;

        public TicketsController(IPermissionTicketStorage storage,
            IPermissionTicketManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("request")]
        [SwaggerResponse(200, "A JWT Permissions Ticket", typeof(string))]
        [SwaggerResponse(400, "A ticket was not able to be issued.", typeof(string))]
        public IActionResult RequestTicket(PermissionRequest[] request)
        {
            var ticket = this.manager.Request(request);
            if(!ticket.IsValid)
                return BadRequest("Invalid Request");

            return Content(ticket.ToJwt(Secret));
        }

        [HttpPost]
        [Route("validate")]
        [SwaggerResponse(200, "The validated Ticket", typeof(PermissionTicket))]
        [SwaggerResponse(400, "The provided ticket is invalid", typeof(string))]
        public IActionResult Validate(PermissionTicket ticket)
        {
            var valid = this.manager.Validate(ticket);
            if (valid) return Ok(ticket);
            return BadRequest("Ticket is Invalid");
        }

        [HttpGet]
        [Route("validate")]
        [SwaggerResponse(200, "The validated Ticket", typeof(PermissionTicket))]
        [SwaggerResponse(400, "The provided ticket is invalid", typeof(string))]
        public IActionResult Validate([FromHeader(Name = "X-PemTicket")] string ticket)
        {
            var valid = this.manager.Validate(ticket, Secret);
            if (valid) return Ok(PermissionTicket.FromJwt(ticket, Secret));
            return BadRequest("Ticket is Invalid");
        }

        [HttpDelete]
        [Route("{hash}")]
        [SwaggerResponse(200, "The provided ticket was revoked", typeof(string))]
        public IActionResult Revoke(string hash)
        {
            this.manager.Revoke(hash);
            return Ok();
        }
    }
}
