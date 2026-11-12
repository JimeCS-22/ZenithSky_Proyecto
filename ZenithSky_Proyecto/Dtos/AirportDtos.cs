using System.ComponentModel.DataAnnotations;

namespace ZenithSky_Proyecto.Dtos
{
    public class AirportDtos
    {
        public class AirportRequest
        {
            [Required(ErrorMessage = "iata es obligatorio.")]
            [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "iata debe tener exactamente 3 letras.")]
            public string Iata { get; set; } = string.Empty;

            [Required(ErrorMessage = "icao es obligatorio.")]
            [RegularExpression("^[A-Za-z]{4}$", ErrorMessage = "icao debe tener exactamente 4 letras.")]
            public string Icao { get; set; } = string.Empty;

            [Required(ErrorMessage = "name es obligatorio.")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "name admite hasta 100 caracteres.")]
            public string Name { get; set; } = string.Empty;
        }

        public record AirportDto(int AirportId, string Iata, string Icao, string Name);
    }
}
