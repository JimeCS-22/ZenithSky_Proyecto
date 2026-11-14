using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenithSky_Proyecto.Common;
using ZenithSky_Proyecto.Data;
using ZenithSky_Proyecto.Domin;
using ZenithSky_Proyecto.Dtos;

namespace ZenithSky_Proyecto.Services;

public interface IAirlineService
{
    Task<IReadOnlyList<AirlineResponseDto>> GetAllAsync(CancellationToken ct);
    Task<AirlineResponseDto> GetByIdAsync(int id, CancellationToken ct);
    Task<AirlineResponseDto> CreateAsync(AirlineRequestDto dto, CancellationToken ct);
    Task<AirlineResponseDto> UpdateAsync(int id, AirlineRequestDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}

public class AirlineService(ZenithSkyDbContext db) : IAirlineService
{
    private static readonly Expression<Func<Airline, AirlineResponseDto>> ToDto =
        l => new AirlineResponseDto(
            l.AirlineId,
            l.Iata,
            l.AirlineName,
            l.BaseAirportId,
            new AirportResponseDto(
                l.Airport.AirportId,
                l.Airport.Iata,
                l.Airport.Icao,
                l.Airport.Name));

    public async Task<IReadOnlyList<AirlineResponseDto>> GetAllAsync(CancellationToken ct) =>
        await db.Airlines.AsNoTracking().OrderBy(l => l.AirlineName).Select(ToDto).ToListAsync(ct);

    public async Task<AirlineResponseDto> GetByIdAsync(int id, CancellationToken ct) =>
        await db.Airlines.AsNoTracking()
            .Where(l => l.AirlineId == id)
            .Select(ToDto)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException($"No existe una aerolínea con id {id}.");

    public async Task<AirlineResponseDto> CreateAsync(AirlineRequestDto dto, CancellationToken ct)
    {
        await EnsureBaseAirportExistsAsync(dto.BaseAirport, ct);

        var entity = new Airline
        {
            Iata = dto.Iata.Trim().ToUpperInvariant(),
            AirlineName = dto.AirlineName.Trim(),
            BaseAirportId = dto.BaseAirport
        };
        db.Airlines.Add(entity);
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.AirlineId, ct);
    }

    public async Task<AirlineResponseDto> UpdateAsync(int id, AirlineRequestDto dto, CancellationToken ct)
    {
        var entity = await db.Airlines.FirstOrDefaultAsync(l => l.AirlineId == id, ct)
            ?? throw new NotFoundException($"No existe una aerolínea con id {id}.");

        await EnsureBaseAirportExistsAsync(dto.BaseAirport, ct);

        entity.Iata = dto.Iata.Trim().ToUpperInvariant();
        entity.AirlineName = dto.AirlineName.Trim();
        entity.BaseAirportId = dto.BaseAirport;
        await db.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Airlines.FirstOrDefaultAsync(l => l.AirlineId == id, ct)
            ?? throw new NotFoundException($"No existe una aerolínea con id {id}.");

        if (await db.Airplanes.AnyAsync(p => p.AirlineId == id, ct))
            throw new ConflictException("No se puede eliminar la aerolínea porque tiene aeronaves asignadas.");

        db.Airlines.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private async Task EnsureBaseAirportExistsAsync(int airportId, CancellationToken ct)
    {
        if (!await db.Airports.AnyAsync(a => a.AirportId == airportId, ct))
            throw new BadRequestException($"El aeropuerto base con id {airportId} no existe.");
    }
}
