using Microsoft.AspNetCore.Mvc;
using ZenithSky_Proyecto.Dtos;
using ZenithSky_Proyecto.Services;

namespace ZenithSky_Proyecto.Controllers;

[ApiController]
[Route("api/airlines")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AirlinesController(IAirlineService service) : ControllerBase
{
    // GET /api/airlines  -> incluye el aeropuerto base (base_airport_info)
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirlineResponseDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    // GET /api/airlines/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineResponseDto>> GetById(int id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, ct));

    // POST /api/airlines
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirlineResponseDto>> Create(
        [FromBody] AirlineRequestDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.AirlineId }, created);
    }

    // PUT /api/airlines/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineResponseDto>> Update(
        int id, [FromBody] AirlineRequestDto dto, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, dto, ct));

    // DELETE /api/airlines/{id}  -> 409 si tiene aeronaves asignadas
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
