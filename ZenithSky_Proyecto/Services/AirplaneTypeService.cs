using Microsoft.EntityFrameworkCore;
using ZenithSky_Proyecto.Data;
using ZenithSky_Proyecto.Dtos;

namespace ZenithSky_Proyecto.Services;

public interface IAirplaneTypeService
{
    Task<IReadOnlyList<AirplaneTypeDto>> GetAllAsync(CancellationToken ct);
}

public class AirplaneTypeService(ZenithSkyDbContext db) : IAirplaneTypeService
{
    public async Task<IReadOnlyList<AirplaneTypeDto>> GetAllAsync(CancellationToken ct) =>
        await db.AirplaneTypes.AsNoTracking()
            .OrderBy(t => t.Identifier)
            .Select(t => new AirplaneTypeDto(t.TypeId, t.Identifier, t.Description))
            .ToListAsync(ct);
}
