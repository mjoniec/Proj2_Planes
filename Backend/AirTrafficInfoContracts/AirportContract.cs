namespace AirTrafficInfoContracts
{
    public class AirportContract
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Color { get; set; }
        public bool IsGoodWeather { get; set; }

        //const
        public Type Type => Type.Airport;
        
        //UI related
        public string Symbol => "triangle";
    }
}
