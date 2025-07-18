using System.Security.Claims;
using Backend.Application.Common.Parameters;
using Backend.Application.Features.Expenses.Commands;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Application.Features.Expenses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Web.Controllers;

/// <summary>
/// Handles operations related to employee expenses.
/// </summary>
[ApiController]
[Route("api/[controller]s")]
public class ExpenseController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ExpenseController> _logger;

    public ExpenseController(ISender sender, ILogger<ExpenseController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Create a new expense.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Expenses.Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateExpense([FromForm] CreateExpenseCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Created expense: {@Result}", result);

        return result.Succeeded ? Ok(result) : BadRequest(result);
    }


    /// <summary>
    /// Update an existing expense.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Expenses.Edit")]

    public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Updated expense: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete an expense by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Expenses.Delete")]


    public async Task<IActionResult> DeleteExpense(int id)
    {
        var result = await _sender.Send(new DeleteExpenseCommand(id));
        _logger.LogInformation("Deleted expense with ID {ExpenseId}: {@Result}", id, result);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all expenses with optional filters.
    /// </summary>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Expenses.View")]


    public async Task<IActionResult> GetAllExpenses([FromQuery] PagingParameter paging, [FromQuery] string? userId, [FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var result = await _sender.Send(new GetAllExpensesQuery(paging,userId, day, month, year));
        _logger.LogInformation("Fetched expenses with filters - UserId: {UserId}, Day: {Day}, Month: {Month}, Year: {Year}", userId, day, month, year);
        return Ok(result);
    }

    /// <summary>
    /// Get expenses for the currently authenticated user.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Employee")]

    public async Task<IActionResult> GetMyExpenses([FromQuery] PagingParameter paging, [FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var result = await _sender.Send(new GetAllExpensesQuery(paging, userId, day, month, year));
        _logger.LogInformation("Fetched expenses for current user {UserId}", userId);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific expense by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Policy = "Expenses.View")]


    public async Task<IActionResult> GetExpenseById(int id)
    {
        var result = await _sender.Send(new GetExpenseByIdQuery(id));
        _logger.LogInformation("Fetched expense with ID {ExpenseId}", id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }
}
