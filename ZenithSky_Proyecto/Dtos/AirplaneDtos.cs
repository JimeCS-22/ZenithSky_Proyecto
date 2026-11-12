using System.ComponentModel.DataAnnotations;

namespace ZenithSky_Proyecto.Dtos
{
    public class AirplaneDtos
    {
        public class AirplaneRequest
        {
            [Range(1, int.MaxValue, ErrorMessage = "capacity debe ser mayor que 0.")]
            public int Capacity { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "typeId es obligatorio.")]
            public int TypeId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "airlineId es obligatorio.")]
            public int AirlineId { get; set; }
        }

        public record AirplaneDto(
            int AirplaneId,
            int Capacity,
            int TypeId,
            string TypeIdentifier,
            string? TypeDescription,
            int AirlineId,
            string AirlineName);
    }
}
