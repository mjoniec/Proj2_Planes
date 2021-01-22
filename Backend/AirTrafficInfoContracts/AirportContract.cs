namespace AirTrafficInfoContracts
{
    public class AirportContract
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Color { get; set; }
        public double SymbolRotate { get; set; }
        public string Symbol => "triangle";
        public bool IsGoodWeather { get; set; }
        public Type Type => Type.Airport;
    }
}
