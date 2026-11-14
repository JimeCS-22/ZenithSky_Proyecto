using Microsoft.AspNetCore.Mvc;
using ZenithSky_Proyecto.Dtos;
using ZenithSky_Proyecto.Services;

namespace ZenithSky_Proyecto.Controllers;

[ApiController]
[Route("api/airplanes")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AirplanesController(IAirplaneService service) : ControllerBase
{
    // GET /api/airplanes?typeId=1&airlineId=2
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirplaneResponseDto>>> GetAll(
        [FromQuery] int? typeId, [FromQuery] int? airlineId, CancellationToken ct) =>
        Ok(await service.GetAllAsync(typeId, airlineId, ct));

    // GET /api/airplanes/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirplaneResponseDto>> GetById(int id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, ct));

    // POST /api/airplanes
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirplaneResponseDto>> Create(
        [FromBody] AirplaneRequestDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.AirplaneId }, created);
    }

    // PUT /api/airplanes/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirplaneResponseDto>> Update(
        int id, [FromBody] AirplaneRequestDto dto, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, dto, ct));

    // DELETE /api/airplanes/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
