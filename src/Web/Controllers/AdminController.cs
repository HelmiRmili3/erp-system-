using Backend.Application.Features.Admin.Commands;
using Backend.Application.Features.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ISender sender, ILogger<AdminController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        // ----- Commands -----

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-claim-to-role")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignClaimToRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-claim")]
        public async Task<IActionResult> DeleteClaim([FromBody] DeleteRoleClaimCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // ----- Queries -----

        [HttpGet("claims")]
        public async Task<IActionResult> GetClaims()
        {
            var result = await _sender.Send(new GetClaimsQuery());
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("claims-with-roles")]
        public async Task<IActionResult> GetClaimsWithRoles()
        {
            var result = await _sender.Send(new GetClaimsWithRolesQuery());
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("claim/{id}")]
        public async Task<IActionResult> GetClaimById(int id)
        {
            var result = await _sender.Send(new GetClaimByIdQuery(id));
            return result.Succeeded ? Ok(result) : NotFound(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _sender.Send(new GetRolesQuery());
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("role/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await _sender.Send(new GetRoleByIdQuery(id));
            return result.Succeeded ? Ok(result) : NotFound(result);
        }

        [HttpGet("roles-with-claims")]
        public async Task<IActionResult> GetRolesWithClaims()
        {
            var result = await _sender.Send(new GetRolesWithClaimsQuery());
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
