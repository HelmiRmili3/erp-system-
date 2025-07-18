using Backend.Application.Common.Parameters;
using Backend.Application.Features.Admin.Commands;
using Backend.Application.Features.Admin.Queries;
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

        [HttpPost("assign-permissions-to-role")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionsToRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-roles-to-user")]
        public async Task<IActionResult> AssignRolesToUser([FromBody] AssignRolesToUserCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove-permissions-from-role")]
        public async Task<IActionResult> RemovePermissionsFromRole([FromBody] DeletePermissionsFromRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("role")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand command)
        {
            var result = await _sender.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        // ----- Queries -----

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions([FromQuery] PagingParameter paging)
        {
            var result = await _sender.Send(new GetPermissionsQuery(paging));
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles([FromQuery] PagingParameter paging)
        {
            var result = await _sender.Send(new GetRolesQuery(paging));
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpGet("rolesPermissions")]
        public async Task<IActionResult> GetRolesPermissions([FromQuery] PagingParameter paging)
        {
            var result = await _sender.Send(new GetRolesWithPermissionsQuery(paging));
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] PagingParameter paging)
        {
            var result = await _sender.Send(new GetUsersQuery(paging));
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


    }

}
