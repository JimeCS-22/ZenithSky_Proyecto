namespace ZenithSky_Proyecto.Domin
{
    public class Airplane
    {
        public int AirplaneId { get; set; }
        public int Capacity { get; set; }
        public int TypeId { get; set; }      // FK -> airplane_type
        public int AirlineId { get; set; }   // FK -> airline

        public AirplaneType? Type { get; set; }
        public Airline? Airline { get; set; }
    }
}
