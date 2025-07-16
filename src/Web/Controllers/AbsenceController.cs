using Backend.Application.Features.Absences.Commands;
using Backend.Application.Features.Absences.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class AbsenceController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<AbsenceController> _logger;

    public AbsenceController(ISender sender, ILogger<AbsenceController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    // GET: api/absence
    [HttpGet]
    [Authorize(Policy = "Absences.View")]
    public async Task<IActionResult> GetAbsences()
    {
        var result = await _sender.Send(new GetAbsencesQuery());
        _logger.LogInformation("Retrieved all absences successfully.");
        return Ok(result);
    }

    // GET: api/absence/{id}
    [HttpGet("{id}", Name = "GetAbsence")]
    [Authorize(Policy = "Absences.View")]
    public async Task<IActionResult> GetAbsence(int id)
    {
        var result = await _sender.Send(new GetAbsenceQuery(id));
        _logger.LogInformation("Retrieved absence with id {AbsenceId}.", id);
        return Ok(result);
    }

    // POST: api/absence
    [Authorize(Policy = "Absences.Create")]
    [HttpPost]
    public async Task<IActionResult> CreateAbsence([FromBody] CreateAbsenceCommand command)
    {
        var response = await _sender.Send(command);
        _logger.LogInformation("Created absence with response: {@Response}", response);
        return CreatedAtRoute("GetAbsence", new { id = response }, response);
    }
    // PUT: api/absence/{id}
    [Authorize(Policy = "Absences.Edit")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAbsence(int id, [FromBody] UpdateAbsenceCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _sender.Send(command);
        _logger.LogInformation("Updated absence with id {AbsenceId}. Result: {@Result}", id, result);
        return Ok(result);
    }

    // DELETE: api/absence/{id}
    [Authorize(Policy = "Absences.Delete")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAbsence(int id)
    {
        var result = await _sender.Send(new DeleteAbsenceCommand(id));
        _logger.LogInformation("Deleted absence with id {AbsenceId}. Result: {@Result}", id, result);
        return Ok(result);
    }
    // GET: api/absence/my
    [Authorize(Roles = "Employee", Policy = "Absences.View")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyAbsences([FromQuery] int? month, [FromQuery] int? year)
    {
        var query = new GetEmployeeAbsencesByIdQuery(month, year);
        var result = await _sender.Send(query);
        _logger.LogInformation("Retrieved absences for current user with filters - Month: {Month}, Year: {Year}", month, year);
        return Ok(result);
    }

}
