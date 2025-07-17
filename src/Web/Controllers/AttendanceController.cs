using System.Security.Claims;
using Backend.Application.Features.Attendances.Commands;
using Backend.Application.Features.Attendances.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// Handles attendance operations such as check-in, check-out, and retrieval.
/// </summary>
[ApiController]
[Route("api/[controller]s")]
public class AttendanceController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(ISender sender, ILogger<AttendanceController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Mark attendance for a user (Check-In or Check-Out).
    /// </summary>
    /// <param name="command">The attendance data including user ID, date, and check-in or check-out time.</param>
    /// <returns>A success or error response depending on the state of the attendance record.</returns>
    [Authorize(Policy = "Attendances.Create")]
    [HttpPost("mark")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceCommand command)
    {
        var response = await _sender.Send(command);
        _logger.LogInformation("Attendance marked: {@Response}", response);
        return response.Succeeded ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Delete a specific attendance record by ID.
    /// </summary>
    /// <param name="id">The ID of the attendance record.</param>
    /// <returns>Confirmation of deletion.</returns>
    [Authorize(Policy = "Attendances.Delete")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var result = await _sender.Send(new DeleteAttendanceCommand(id));
        _logger.LogInformation("Deleted attendance with ID {AttendanceId}: {@Result}", id, result);
        return Ok(result);
    }

    /// <summary>
    /// Get all attendance records for all users with optional filters.
    /// </summary>
    /// <param name="day">Optional day filter.</param>
    /// <param name="month">Optional month filter.</param>
    /// <param name="year">Optional year filter.</param>
    /// <returns>List of all attendance records.</returns>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Policy = "Attendances.View")]
    public async Task<IActionResult> GetAllAttendances([FromQuery] string? userId, [FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var result = await _sender.Send(new GetAllAttendancesQuery(userId,day, month, year));
        _logger.LogInformation("Fetched all attendances with filters - UserId: {UserId},Day: {Day}, Month: {Month}, Year: {Year}",userId, day, month, year);
        return Ok(result);
    }

    /// <summary>
    /// Get attendance for the currently authenticated user (check-in and check-out history).
    /// </summary>
    /// <param name="month">Optional month filter.</param>
    /// <param name="year">Optional year filter.</param>
    /// <returns>List of the user's attendance records.</returns>
    [Authorize]
    [HttpGet("me")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetMyAttendances([FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var result = await _sender.Send(new GetUserAttendancesQuery(userId, day, month, year));
        _logger.LogInformation("Fetched attendance for current user {UserId} with Day:{Day} Month: {Month}, Year: {Year}", userId, day, month, year);
        return Ok(result);
    }
}
