using Microsoft.AspNetCore.Mvc;
using ZenithSky_Proyecto.Dtos;
using ZenithSky_Proyecto.Services;

namespace ZenithSky_Proyecto.Controllers;

[ApiController]
[Route("api/airports")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AirportsController(IAirportService service) : ControllerBase
{
    // GET /api/airports?search=juan  -> busca por nombre, IATA o ICAO
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirportResponseDto>>> GetAll(
        [FromQuery] string? search, CancellationToken ct) =>
        Ok(await service.GetAllAsync(search, ct));

    // GET /api/airports/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportResponseDto>> GetById(int id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, ct));

    // POST /api/airports
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AirportResponseDto>> Create(
        [FromBody] AirportRequestDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.AirportId }, created);
    }

    // PUT /api/airports/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AirportResponseDto>> Update(
        int id, [FromBody] AirportRequestDto dto, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, dto, ct));

    // DELETE /api/airports/{id}  -> 409 si es base_airport de alguna aerolínea
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
