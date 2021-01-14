namespace AirTrafficInfoContracts
{
    public class AirportContract
    {
        public string Name { get; set; }
        public double PositionX { get; set; }
        public double Positiony { get; set; }
        public bool IsGoodWeather { get; set; }
        public Type Type => Type.Airport;
    }
}
