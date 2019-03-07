using System;
using System.Collections.Generic;

namespace Data.Services
{
    public class GoldService : IGoldService
    {
        //TODO
        //initialize GOLD SERVICE MQTT client here

        public IEnumerable<string> GetAll()
        {
            return new List<string> { "1", "2" };
        }

        public DateTime GetFirstDate()
        {
            return new DateTime(1971, 05, 24);
        }

        public string GetForToday()
        {
            return "2";
        }
    }
}
