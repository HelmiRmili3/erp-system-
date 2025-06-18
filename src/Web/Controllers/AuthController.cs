using Backend.Application.Features.Authentication.Commands;
using MediatR;
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
        var response = await _sender.Send(command);

        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }


    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var response = await _sender.Send(command);

        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }
}
