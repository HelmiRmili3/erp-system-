using Backend.Application.Features.Employees.Commands;
using Backend.Application.Features.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ISender sender, ILogger<EmployeesController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var result = await _sender.Send(new GetEmployeesQuery());
        _logger.LogInformation("Retrieved all employees successfully.");
        return Ok(result);
    }

    // GET: api/employees/{id}
    [HttpGet("{id}", Name = "GetEmployee")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var result = await _sender.Send(new GetEmployeeQuery(id));
        _logger.LogInformation("Retrieved employee with id {EmployeeId}.", id);
        return Ok(result);
    }

    // POST: api/employees

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
    {
        var response = await _sender.Send(command);
        _logger.LogInformation("Created employee with response: {@Response}", response);
        return CreatedAtRoute("GetEmployee", new { id = response }, response);
    }

    // DELETE: api/employees/{id}

    [HttpDelete("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(String id)
    {
        var result = await _sender.Send(new DeleteEmployeeCommand(int.Parse(id)));
        _logger.LogInformation("Deleted employee with id {EmployeeId}. Result: {@Result}", id, result);
        return Ok(result);
    }

}
