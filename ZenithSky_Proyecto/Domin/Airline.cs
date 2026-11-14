namespace ZenithSky_Proyecto.Domin
{

    public class Airline
    {
        public int AirlineId { get; set; }
        public string Iata { get; set; } = string.Empty;          // CHAR(2)
        public string AirlineName { get; set; } = string.Empty;   // VARCHAR(100)
        public int BaseAirportId{ get; set; }                      // FK -> airport

        public Airport? Airport { get; set; }
        public ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();
    }
}

