using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenithSky_Proyecto.Dtos;

public class AirportRequestDto
{
    [JsonPropertyName("iata")]
    [Required(ErrorMessage = "El código IATA es obligatorio.")]
    [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "El código IATA debe tener exactamente 3 letras.")]
    public string Iata { get; set; } = string.Empty;

    [JsonPropertyName("icao")]
    [Required(ErrorMessage = "El código ICAO es obligatorio.")]
    [RegularExpression("^[A-Za-z]{4}$", ErrorMessage = "El código ICAO debe tener exactamente 4 letras.")]
    public string Icao { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;
}

public record AirportResponseDto(
    [property: JsonPropertyName("airport_id")] int AirportId,
    [property: JsonPropertyName("iata")] string Iata,
    [property: JsonPropertyName("icao")] string Icao,
    [property: JsonPropertyName("name")] string Name);
