using System.Text.Json.Serialization;

namespace ZenithSky_Proyecto.Dtos;

public record AirplaneTypeDto(
    [property: JsonPropertyName("type_id")] int TypeId,
    [property: JsonPropertyName("identifier")] string Identifier,
    [property: JsonPropertyName("description")] string? Description);
