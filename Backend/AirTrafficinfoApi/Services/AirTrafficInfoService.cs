namespace AirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private string _airTrafficInfo;

        public string GetAirTrafficInfo()
        {
            return _airTrafficInfo;
        }

        public void UpdatePlaneInfo(string plane, double latitude, double longitude)
        {
            _airTrafficInfo = plane + " " + latitude + " " + longitude;
        }
    }
}
