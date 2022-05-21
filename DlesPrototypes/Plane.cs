namespace DlesPrototypes
{
    class Plane
    {
        //what is needed in order for yellow to work?
        string airports; // could be internal in method 
        string planePosition;
        string destinationPosition;

        object gpsHelper; //for calculations of coordinates
        object airportNotificationSource; //listener for global events (rabbit)
        object planeNotificationSource; //listener for local events (NET or rabbit?)
        object trafficInfo; //for sending info update & requesting airports (http client)

        //Commands - 4 blue
        void SendInfoUpdateToTrafficApi()
        {
            //if plane reached destination - trigger event - yellow 1/3
        }

        void ChangeDestination()
        {
            //pick airport only with good weather  - yellow 2/3
        }

        void ReSubscribeToNewAirport()
        {

        }

        void RequestAirportsInfo()
        {

        }

        //Events - 3 orange
        void OnBadWeatherEventHandler(BadWeatherEvent e) { } //global domain event - outside BC
                                                             //- we need to listen to destination as we go - yellow 3/3
        void OnPlaneLandedEventHandler(PlaneLandedEvent e) { } //internal domain event - inside BC
        void OnPlaneUpdate(PlaneUpdateEvent e) { } //cyclic internal domain event - inside BC
    }
}
