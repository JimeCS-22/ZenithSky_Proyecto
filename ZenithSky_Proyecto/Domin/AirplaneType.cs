namespace ZenithSky_Proyecto.Domin
{
    public class AirplaneType
    {
        public int TypeId { get; set; }
        public string Identifier { get; set; } = string.Empty;   // VARCHAR(50)
        public string? Description { get; set; }                 // VARCHAR(250)

        public ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();
    }
}
