namespace ZenithSky_Proyecto.Domin
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string Iata { get; set; } = string.Empty;   // CHAR(3)
        public string Icao { get; set; } = string.Empty;   // CHAR(4)
        public string Name { get; set; } = string.Empty;   // VARCHAR(100)

        public ICollection<Airline> Airlines { get; set; } = new List<Airline>();
    }
}
