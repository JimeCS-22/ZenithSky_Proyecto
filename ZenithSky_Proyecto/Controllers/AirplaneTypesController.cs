using Microsoft.AspNetCore.Mvc;
using ZenithSky_Proyecto.Dtos;
using ZenithSky_Proyecto.Services;

namespace ZenithSky_Proyecto.Controllers;

[ApiController]
[Route("api/airplane-types")]
[Produces("application/json")]
public class AirplaneTypesController(IAirplaneTypeService service) : ControllerBase
{
    // GET /api/airplane-types  -> catálogo para los selectores de la app móvil
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AirplaneTypeDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));
}
