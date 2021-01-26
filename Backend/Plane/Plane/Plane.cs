using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Plane
{
    public class Plane : AirTrafficBackgroundService
    {
        private readonly IConfiguration _configuration;
        
        public Plane(IConfiguration configuration, IHostEnvironment hostEnvironment) 
            : base(new PlaneService(hostEnvironment))
        {
            _configuration = configuration;

            //works on premises ps launch - dotnet run --color=888111
            //var color = _configuration.GetValue<string>("color");
            //Console.WriteLine(color + " test");
        }
    }
}
