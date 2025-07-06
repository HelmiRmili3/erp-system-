using System.Security.Claims;
using Backend.Application.Features.Contracts.Commands;
using Backend.Application.Features.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// Handles operations related to employee contracts.
/// </summary>
[ApiController]
[Route("api/[controller]s")]
public class ContractController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ContractController> _logger;

    public ContractController(ISender sender, ILogger<ContractController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Create a new contract for a user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Authorize(Roles = "Administrator")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateContract([FromForm] CreateContractCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Created contract: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update an existing contract.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> UpdateContract([FromBody] UpdateContractCommand command)
    {
        var result = await _sender.Send(command);
        _logger.LogInformation("Updated contract: {@Result}", result);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a contract by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> DeleteContract(int id)
    {
        var result = await _sender.Send(new DeleteContractCommand(id));
        _logger.LogInformation("Deleted contract with ID {ContractId}: {@Result}", id, result);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get all contracts (optionally filter by user and date).
    /// </summary>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> GetAllContracts([FromQuery] string? userId, [FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var result = await _sender.Send(new GetAllContractsQuery(userId, day, month, year));
        _logger.LogInformation("Fetched contracts with filters - UserId: {UserId}, Day: {Day}, Month: {Month}, Year: {Year}", userId, day, month, year);
        return Ok(result);
    }

    /// <summary>
    /// Get contracts for the currently logged-in user.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize(Roles = "Employee")]

    public async Task<IActionResult> GetMyContracts([FromQuery] int? day, [FromQuery] int? month, [FromQuery] int? year)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "User ID not found in token." });

        var result = await _sender.Send(new GetAllContractsQuery(userId, day, month, year));
        _logger.LogInformation("Fetched contracts for current user {UserId}", userId);
        return Ok(result);
    }

    ///// <summary>
    ///// Get all contracts for a specific user.
    ///// </summary>
    //[HttpGet("user/{userId}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[Produces("application/json")]
    //public async Task<IActionResult> GetUserContracts(string userId)
    //{
    //    var result = await _sender.Send(new GetContractsByUserIdQuery(userId));
    //    _logger.LogInformation("Fetched contracts for user {UserId}", userId);
    //    return Ok(result);
    //}

    /// <summary>
    /// Get a contract by its ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Authorize]

    public async Task<IActionResult> GetContractById(int id)
    {
        var result = await _sender.Send(new GetContractByIdQuery(id));
        _logger.LogInformation("Fetched contract with ID {ContractId}", id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }
}
