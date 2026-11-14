using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenithSky_Proyecto.Common;
using ZenithSky_Proyecto.Data;
using ZenithSky_Proyecto.Domin;
using ZenithSky_Proyecto.Dtos;

namespace ZenithSky_Proyecto.Services;

public interface IAirportService
{
    Task<IReadOnlyList<AirportResponseDto>> GetAllAsync(string? search, CancellationToken ct);
    Task<AirportResponseDto> GetByIdAsync(int id, CancellationToken ct);
    Task<AirportResponseDto> CreateAsync(AirportRequestDto dto, CancellationToken ct);
    Task<AirportResponseDto> UpdateAsync(int id, AirportRequestDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}

public class AirportService(ZenithSkyDbContext db) : IAirportService
{
    private static readonly Expression<Func<Airport, AirportResponseDto>> ToDto =
        a => new AirportResponseDto(a.AirportId, a.Iata, a.Icao, a.Name);

    public async Task<IReadOnlyList<AirportResponseDto>> GetAllAsync(string? search, CancellationToken ct)
    {
        var query = db.Airports.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(a => a.Name.Contains(term) || a.Iata.Contains(term) || a.Icao.Contains(term));
        }

        return await query.OrderBy(a => a.Name).Select(ToDto).ToListAsync(ct);
    }

    public async Task<AirportResponseDto> GetByIdAsync(int id, CancellationToken ct) =>
        await db.Airports.AsNoTracking()
            .Where(a => a.AirportId == id)
            .Select(ToDto)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException($"No existe un aeropuerto con id {id}.");

    public async Task<AirportResponseDto> CreateAsync(AirportRequestDto dto, CancellationToken ct)
    {
        var iata = dto.Iata.Trim().ToUpperInvariant();
        var icao = dto.Icao.Trim().ToUpperInvariant();
        await EnsureUniqueAsync(iata, icao, 0, ct);

        var entity = new Airport { Iata = iata, Icao = icao, Name = dto.Name.Trim() };
        db.Airports.Add(entity);
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.AirportId, ct);
    }

    public async Task<AirportResponseDto> UpdateAsync(int id, AirportRequestDto dto, CancellationToken ct)
    {
        var entity = await db.Airports.FirstOrDefaultAsync(a => a.AirportId == id, ct)
            ?? throw new NotFoundException($"No existe un aeropuerto con id {id}.");

        var iata = dto.Iata.Trim().ToUpperInvariant();
        var icao = dto.Icao.Trim().ToUpperInvariant();
        await EnsureUniqueAsync(iata, icao, id, ct);

        entity.Iata = iata;
        entity.Icao = icao;
        entity.Name = dto.Name.Trim();
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Airports.FirstOrDefaultAsync(a => a.AirportId == id, ct)
            ?? throw new NotFoundException($"No existe un aeropuerto con id {id}.");

        if (await db.Airlines.AnyAsync(l => l.BaseAirportId == id, ct))
            throw new ConflictException(
                "No se puede eliminar el aeropuerto porque es el aeropuerto base de una o más aerolíneas.");

        db.Airports.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    // excludeId = 0 al crear; al actualizar se excluye el propio registro.
    private async Task EnsureUniqueAsync(string iata, string icao, int excludeId, CancellationToken ct)
    {
        if (await db.Airports.AnyAsync(a => a.Iata == iata && a.AirportId != excludeId, ct))
            throw new ConflictException($"Ya existe un aeropuerto con el código IATA '{iata}'.");

        if (await db.Airports.AnyAsync(a => a.Icao == icao && a.AirportId != excludeId, ct))
            throw new ConflictException($"Ya existe un aeropuerto con el código ICAO '{icao}'.");
    }
}
