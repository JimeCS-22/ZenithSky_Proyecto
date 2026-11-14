using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenithSky_Proyecto.Common;
using ZenithSky_Proyecto.Data;
using ZenithSky_Proyecto.Domin;
using ZenithSky_Proyecto.Dtos;

namespace ZenithSky_Proyecto.Services;

public interface IAirplaneService
{
    Task<IReadOnlyList<AirplaneResponseDto>> GetAllAsync(int? typeId, int? airlineId, CancellationToken ct);
    Task<AirplaneResponseDto> GetByIdAsync(int id, CancellationToken ct);
    Task<AirplaneResponseDto> CreateAsync(AirplaneRequestDto dto, CancellationToken ct);
    Task<AirplaneResponseDto> UpdateAsync(int id, AirplaneRequestDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}

public class AirplaneService(ZenithSkyDbContext db) : IAirplaneService
{
    private static readonly Expression<Func<Airplane, AirplaneResponseDto>> ToDto =
        p => new AirplaneResponseDto(
            p.AirplaneId,
            p.Capacity,
            p.TypeId,
            p.AirplaneType.Identifier,
            p.AirplaneType.Description,
            p.AirlineId,
            p.Airline.AirlineName);

    public async Task<IReadOnlyList<AirplaneResponseDto>> GetAllAsync(
        int? typeId, int? airlineId, CancellationToken ct)
    {
        var query = db.Airplanes.AsNoTracking();

        if (typeId.HasValue)
            query = query.Where(p => p.TypeId == typeId.Value);
        if (airlineId.HasValue)
            query = query.Where(p => p.AirlineId == airlineId.Value);

        return await query.OrderBy(p => p.AirplaneId).Select(ToDto).ToListAsync(ct);
    }

    public async Task<AirplaneResponseDto> GetByIdAsync(int id, CancellationToken ct) =>
        await db.Airplanes.AsNoTracking()
            .Where(p => p.AirplaneId == id)
            .Select(ToDto)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException($"No existe una aeronave con id {id}.");

    public async Task<AirplaneResponseDto> CreateAsync(AirplaneRequestDto dto, CancellationToken ct)
    {
        await ValidateReferencesAsync(dto, ct);

        var entity = new Airplane
        {
            Capacity = dto.Capacity,
            TypeId = dto.TypeId,
            AirlineId = dto.AirlineId
        };
        db.Airplanes.Add(entity);
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.AirplaneId, ct);
    }

    public async Task<AirplaneResponseDto> UpdateAsync(int id, AirplaneRequestDto dto, CancellationToken ct)
    {
        var entity = await db.Airplanes.FirstOrDefaultAsync(p => p.AirplaneId == id, ct)
            ?? throw new NotFoundException($"No existe una aeronave con id {id}.");

        await ValidateReferencesAsync(dto, ct);

        entity.Capacity = dto.Capacity;
        entity.TypeId = dto.TypeId;
        entity.AirlineId = dto.AirlineId;
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Airplanes.FirstOrDefaultAsync(p => p.AirplaneId == id, ct)
            ?? throw new NotFoundException($"No existe una aeronave con id {id}.");

        db.Airplanes.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private async Task ValidateReferencesAsync(AirplaneRequestDto dto, CancellationToken ct)
    {
        if (dto.Capacity <= 0)
            throw new BadRequestException("La capacidad debe ser mayor a 0.");

        if (!await db.AirplaneTypes.AnyAsync(t => t.TypeId == dto.TypeId, ct))
            throw new BadRequestException($"El tipo de avión con id {dto.TypeId} no existe.");

        if (!await db.Airlines.AnyAsync(l => l.AirlineId == dto.AirlineId, ct))
            throw new BadRequestException($"La aerolínea con id {dto.AirlineId} no existe.");
    }
}
