using System.Security.Claims;
using Backend.Application.Features.Authentication.Commands;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _sender.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }


    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _sender.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    [HttpPost("refresh")]
    [Produces("application/json")]
    public  Task<string> Refresh([FromBody] string command)
    {
        // This indicates the method is not yet implemented
        throw new NotImplementedException("Refresh endpoint is not implemented yet.");
    }
    [HttpPost("change-password")]
    [Authorize]
    public  Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var result = await _userManager.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);

        //if (!result)
        //    return BadRequest(new { Message = "Password change failed" });

        throw new NotImplementedException("Change password endpoint is not implemented yet.");

    }
    [HttpGet("current-user")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        // This works because "nameid" is ClaimTypes.NameIdentifier
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var result = await _sender.Send(new GetCurrentUserQuery(userId));
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}
