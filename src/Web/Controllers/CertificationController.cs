using System.Security.Claims;
using Backend.Application.Common.Parameters;
using Backend.Application.Features.Certifications.Commands;
using Backend.Application.Features.Certifications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// Handles operations related to employee certifications.
/// </summary>
[ApiController]
[Route("api/[controller]s")]
public class CertificationController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<CertificationController> _logger;

    public CertificationController(ISender sender, ILogger<CertificationController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Create a new certification for a user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Authorize(Policy = "Certifications.Create")]
    [Consumes("multipart/form-data")]

    public async Task<IActionResult> CreateCertification([FromForm] CreateCertificationCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Created certification: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update an existing certification.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Certifications.Edit")]

    public async Task<IActionResult> UpdateCertification([FromBody] UpdateCertificationCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Updated certification: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a certification by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Certifications.Delete")]

    public async Task<IActionResult> DeleteCertification(int id)
    {
        var result = await _sender.Send(new DeleteCertificationCommand(id));
        _logger.LogInformation("Deleted certification with ID {CertificationId}: {@Result}", id, result);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all certifications (optionally filter by user and date).
    /// </summary>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Certifications.View")]

    public async Task<IActionResult> GetAllCertifications([FromQuery] PagingParameter paging,[FromQuery] string? userId, [FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var result = await _sender.Send(new GetAllCertificationsQuery(paging,userId, day, month, year));
        _logger.LogInformation("Fetched certifications with filters - UserId: {UserId}, Day: {Day}, Month: {Month}, Year: {Year}", userId, day, month, year);
        return Ok(result);
    }

    /// <summary>
    /// Get certifications for the currently logged-in user.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Employee")]

    public async Task<IActionResult> GetMyCertifications([FromQuery] PagingParameter paging,[FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var result = await _sender.Send(new GetAllCertificationsQuery(paging, userId, day, month, year));
        _logger.LogInformation("Fetched certifications for current user {UserId}", userId);
        return  Ok(result);
    }
    /// <summary>
    /// Get a certification by its ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize]
    [Authorize(Policy = "Certifications.View")]
    public async Task<IActionResult> GetCertificationById(int id)
    {
        var result = await _sender.Send(new GetCertificationByIdQuery(id));
        _logger.LogInformation("Fetched certification with ID {CertificationId}", id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }
}
