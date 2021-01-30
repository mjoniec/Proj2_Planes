using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Airport
{
    class AirportBackgroundService : AirTrafficBackgroundService
    {
        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(new AirportService(configuration, hostEnvironment))
        {
            //works on premises ps launch - dotnet run --color=888111
            //var color = configuration.GetValue<string>("color");
            //Console.WriteLine(color + " test");
        }
    }
}
