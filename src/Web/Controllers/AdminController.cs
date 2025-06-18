using Backend.Application.Features.Admin.Commands;
using Backend.Application.Features.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
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

        // Create a new role
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }



        // Assign permission to role
        [HttpPost("assign-permission-to-role")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignClaimToRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // Assign role to user
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // Delete a role
        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // Delete a claim
        [HttpDelete("delete-claim")]
        public async Task<IActionResult> DeleteClaim([FromBody] DeleteRoleClaimCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

    }
}
