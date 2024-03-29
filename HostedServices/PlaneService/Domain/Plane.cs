﻿using Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Utils;

namespace PlaneService.Domain
{
    /// <summary>
    /// Comment for PR:
    /// - Domain object Plane should not have http client directly and handle connection logic - it should have airports data query or sth ...
    /// - Domain object Plane should not have stopping token, that logic belongs in host service or worker, plane should expose how to start update and stop it, business domain behavior
    /// - navigation (shared kernel?) class have move plane method, that is leaking language, it should have move point x by speed y on time t etc ...
    /// - navigation in plane or domain specific assumption (like accuracy when we consider destination as reached) should be in plane
    /// - plane and its navigation should be abstract to its host service, usable from simulated traffic and actual background worker 
    /// </summary>

    public class Plane
    {
        private PlaneContract _planeContract;

        public PlaneContract PlaneContract => _planeContract; //TODO should I expose this according to DDD ? it uses Name from PlaneContract as an entity Id
        public bool PlaneReachedItsDestination { get; private set; }//I think this should be an event, refactor away from procedural state machine
        private readonly ILogger<Plane> _logger;

        public Plane(string name)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<Plane>();
            PlaneReachedItsDestination = false;
            _planeContract = new PlaneContract
            {
                Name = name,
                SpeedInMetersPerSecond = 1000000 //business constant - speed is as such for updates to be notable - export to some config ?
            };
        }

        /// <summary>
        /// Setup Destination and Departure Airports for the Plane
        /// </summary>
        /// <returns></returns>
        public void StartPlane(List<AirportContract> airports)
        {
            _logger.LogInformation(_planeContract.Name + "Plane start at: " + DateTime.Now.ToString("G"));

            if (!AreEnoughAirportsToSelectNewDestination(airports))
            {
                EmptyDestinationAndDepartureAirports();

                return;
            }

            var randomDepartureAirport = SelectRandomAirport(airports);

            _planeContract.SetDepartureAirportData(randomDepartureAirport);
            _planeContract.Latitude = randomDepartureAirport.Latitude;
            _planeContract.Longitude = randomDepartureAirport.Longitude;
            _planeContract.LastPositionUpdate = DateTime.Now;

            SetDestinationAirport(airports, randomDepartureAirport.Name);
        }

        public void UpdatePlane()
        {
            _logger.LogInformation(_planeContract.Name + "Plane update at: " + DateTime.Now.ToString("G"));

            var currentTime = DateTime.Now;

            Navigation.MovePlane(ref _planeContract, currentTime);

            _planeContract.LastPositionUpdate = currentTime;
            PlaneReachedItsDestination = HasPlaneReachedItsDestination();
        }

        //see task #33
        //domain logic on how to select an airport does not belong in plane - should be in a separate service - moreover its a public method, just screams refactor me ...
        public void SelectNewDestinationAirport(List<AirportContract> airports)
        {
            _logger.LogInformation("SelectNewDestinationAirport");

            if (!AreEnoughAirportsToSelectNewDestination(airports))
            {
                _logger.LogWarning("NOT AreEnoughAirportsToSelectNewDestination - should not happen !!!");

                EmptyDestinationAndDepartureAirports();

                return;
            }

            SetDestinationAirport(airports, _planeContract.DestinationAirportName);
        }

        private void SetDestinationAirport(List<AirportContract> airports, string exceptThisAirportName)
        {
            var randomDestinationAirport = SelectRandomAirportExceptTheOneProvided(airports, exceptThisAirportName);

            _planeContract.SetNewDestinationAndDepartureAirports(randomDestinationAirport);
            _planeContract.DepartureTime = DateTime.Now;
        }

        private void EmptyDestinationAndDepartureAirports()
        {
            _planeContract.DestinationAirportName = string.Empty;
            _planeContract.DepartureAirportName = string.Empty;
            _planeContract.Color = "#000000";
        }

        private bool AreEnoughAirportsToSelectNewDestination(List<AirportContract> airports)
        {
            return airports.Count >= 2;
        }

        private AirportContract SelectRandomAirport(List<AirportContract> airports)
        {
            return airports[new Random().Next(0, airports.Count)];
        }

        private AirportContract SelectRandomAirportExceptTheOneProvided(List<AirportContract> allAirports, string exceptThisAirportName)
        {
            var airportsExcludingGivenException = new List<AirportContract>(allAirports);

            airportsExcludingGivenException.RemoveAll(a => a.Name == exceptThisAirportName);

            return airportsExcludingGivenException[new Random().Next(0, airportsExcludingGivenException.Count)];
        }

        private bool HasPlaneReachedItsDestination()
        {
            return Navigation.HasPlaneReachedItsDestination(
                _planeContract.DestinationAirportLatitude,
                _planeContract.DestinationAirportLongitude,
                _planeContract.Latitude,
                _planeContract.Longitude);
        }
    }
}
