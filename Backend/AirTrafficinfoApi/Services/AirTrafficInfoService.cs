namespace AirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private string _plane;
        private double _latitude;
        private double _longitude;

        public string GetAirTrafficInfo()
        {
            return _plane + " " + _latitude.ToString() + " " + _longitude.ToString();
        }

        public void UpdatePlaneInfo(string plane, double latitude, double longitude)
        {
            _plane = plane;
            _latitude = latitude;
            _longitude = longitude;
        }
    }
}
