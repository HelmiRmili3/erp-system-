using Backend.Application.Features.Payrolls.Commands;
using Backend.Application.Features.Payrolls.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// Manages employee payrolls.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<PayrollController> _logger;

    public PayrollController(ISender sender, ILogger<PayrollController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Create a new payroll record.
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> CreatePayroll([FromForm] CreatePayrollCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Created payroll: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }


    /// <summary>
    /// Update an existing payroll.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> UpdatePayroll([FromBody] UpdatePayrollCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Updated payroll: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a payroll by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeletePayroll(int id)
    {
        var result = await _sender.Send(new DeletePayrollCommand(id));
        _logger.LogInformation("Deleted payroll with ID {PayrollId}: {@Result}", id, result);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all payrolls (optionally filter by user or period).
    /// </summary>
    [HttpGet("all")]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> GetAllPayrolls(
       [FromQuery] string? userId,
       [FromQuery] int? month = null,
       [FromQuery] int? year = null)
    {
        var query = new GetAllPayrollsQuery(userId, month, year);
        var result = await _sender.Send(query);

        _logger.LogInformation("Fetched payrolls - UserId: {UserId}, Month: {Month}, Year: {Year}",
            userId, month, year);

        return Ok(result);
    }

    /// <summary>
    /// Get a payroll by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]

    public async Task<IActionResult> GetPayrollById(int id)
    {
        var result = await _sender.Send(new GetPayrollQuery(id));
        _logger.LogInformation("Fetched payroll with ID {PayrollId}", id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get payrolls for the currently authenticated user.
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "Employee")]

    public async Task<IActionResult> GetMyPayrolls(
     [FromQuery] int? month = null,
     [FromQuery] int? year = null)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var query = new GetAllPayrollsQuery(userId,month, year);

        var result = await _sender.Send(query);

        _logger.LogInformation("Fetched payrolls for current user {UserId} with filters Month: {Month}, Year: {Year}",
            userId, month, year);

        return Ok(result);
    }


   
}
