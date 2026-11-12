using System.ComponentModel.DataAnnotations;

namespace ZenithSky_Proyecto.Dtos
{
    public class AirlineDtos
    {
        public class AirlineRequest
        {
            [Required(ErrorMessage = "iata es obligatorio.")]
            [RegularExpression("^[A-Za-z0-9]{2}$", ErrorMessage = "iata debe tener exactamente 2 caracteres.")]
            public string Iata { get; set; } = string.Empty;

            [Required(ErrorMessage = "airlinename es obligatorio.")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "airlinename admite hasta 100 caracteres.")]
            public string Airlinename { get; set; } = string.Empty;

            [Range(1, int.MaxValue, ErrorMessage = "baseAirport es obligatorio.")]
            public int BaseAirport { get; set; }
        }

        // "airport" trae los datos completos del aeropuerto base (nombre, IATA, ICAO)
        public record AirlineDto(int AirlineId, string Iata, string Airlinename, int BaseAirport, AirportDtos.AirportDto? Airport);

    }
}
