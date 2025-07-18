using Backend.Application.Common.Parameters;
using Backend.Application.Features.Payrolls.Commands;
using Backend.Application.Features.Payrolls.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// Manages employee payrolls.
/// </summary>
[ApiController]
[Route("api/[controller]s")]
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
    [Authorize(Policy = "Payrolls.View")]

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
    [Authorize(Policy = "Payrolls.Edit")]
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
    [Authorize(Policy = "Payrolls.Delete")]

    public async Task<IActionResult> DeletePayroll(int id)
    {
        var result = await _sender.Send(new DeletePayrollCommand(id));
        _logger.LogInformation("Deleted payroll with ID {PayrollId}: {@Result}", id, result);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all payrolls (optionally filter by user or period).
    /// </summary>
    [HttpGet()]
    [Authorize(Policy = "Payrolls.View")]


    public async Task<IActionResult> GetAllPayrolls(
       [FromQuery] PagingParameter paging,
       [FromQuery] string? userId,
       [FromQuery] int? month = null,
       [FromQuery] int? year = null)
    {
        var query = new GetAllPayrollsQuery(paging,userId, month, year);
        var result = await _sender.Send(query);

        _logger.LogInformation("Fetched payrolls - UserId: {UserId}, Month: {Month}, Year: {Year}",
            userId, month, year);

        return Ok(result);
    }

    /// <summary>
    /// Get a payroll by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "Payrolls.View")]

    public async Task<IActionResult> GetPayrollById(int id)
    {
        var result = await _sender.Send(new GetPayrollQuery(id));
        _logger.LogInformation("Fetched payroll with ID {PayrollId}", id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get payrolls for the currently authenticated user.
    /// </summary>
    [HttpGet("me")]
    [Authorize(Roles = "Employee")]

    public async Task<IActionResult> GetMyPayrolls(
    [FromQuery] PagingParameter paging,
     [FromQuery] int? month = null,
     [FromQuery] int? year = null)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var query = new GetAllPayrollsQuery(paging,userId, month, year);

        var result = await _sender.Send(query);

        _logger.LogInformation("Fetched payrolls for current user {UserId} with filters Month: {Month}, Year: {Year}",
            userId, month, year);

        return Ok(result);
    }


   
}
