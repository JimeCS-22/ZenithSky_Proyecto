using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenithSky_Proyecto.Dtos;

public class AirplaneRequestDto
{
    [JsonPropertyName("capacity")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0.")]
    public int Capacity { get; set; }

    [JsonPropertyName("type_id")]
    [Range(1, int.MaxValue, ErrorMessage = "El tipo de avión es obligatorio.")]
    public int TypeId { get; set; }

    [JsonPropertyName("airline_id")]
    [Range(1, int.MaxValue, ErrorMessage = "La aerolínea es obligatoria.")]
    public int AirlineId { get; set; }
}

public record AirplaneResponseDto(
    [property: JsonPropertyName("airplane_id")] int AirplaneId,
    [property: JsonPropertyName("capacity")] int Capacity,
    [property: JsonPropertyName("type_id")] int TypeId,
    [property: JsonPropertyName("type_identifier")] string TypeIdentifier,
    [property: JsonPropertyName("type_description")] string? TypeDescription,
    [property: JsonPropertyName("airline_id")] int AirlineId,
    [property: JsonPropertyName("airline_name")] string AirlineName);
