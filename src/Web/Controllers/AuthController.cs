using System.Security.Claims;
using Backend.Application.Features.Authentication.Commands;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


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
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
    {
        var result = await _sender.Send(command);

        if (!result.Succeeded)
            return BadRequest(new { result.Message, result.Errors });

        return Ok(result);
    }
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var dataWithId = new ChangePasswordDataDto
        {
            UserId = userId,
            CurrentPassword = dto.CurrentPassword,
            NewPassword = dto.NewPassword,
            ConfirmNewPassword = dto.ConfirmNewPassword
        };

        var command = new ChangePasswordCommand(dataWithId);
        var result = await _sender.Send(command);
        Console.WriteLine($"Succeeded: {result.Succeeded}, Message: {result.Message}");

        if (!result.Succeeded)
            return BadRequest(new { Message = result.Message, Errors = result.Errors });

        return Ok(result);

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
