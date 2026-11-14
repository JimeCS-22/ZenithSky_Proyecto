using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenithSky_Proyecto.Dtos;

public class AirlineRequestDto
{
    [JsonPropertyName("iata")]
    [Required(ErrorMessage = "El código IATA es obligatorio.")]
    [RegularExpression("^[A-Za-z0-9]{2}$", ErrorMessage = "El código IATA de la aerolínea debe tener exactamente 2 caracteres.")]
    public string Iata { get; set; } = string.Empty;

    [JsonPropertyName("airlinename")]
    [Required(ErrorMessage = "El nombre de la aerolínea es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string AirlineName { get; set; } = string.Empty;

    [JsonPropertyName("base_airport")]
    [Range(1, int.MaxValue, ErrorMessage = "El aeropuerto base es obligatorio.")]
    public int BaseAirport { get; set; }
}

public record AirlineResponseDto(
    [property: JsonPropertyName("airline_id")] int AirlineId,
    [property: JsonPropertyName("iata")] string Iata,
    [property: JsonPropertyName("airlinename")] string AirlineName,
    [property: JsonPropertyName("base_airport")] int BaseAirport,
    [property: JsonPropertyName("base_airport_info")] AirportResponseDto BaseAirportInfo);
