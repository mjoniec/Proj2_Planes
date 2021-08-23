using System;

namespace Utils
{
    public static class HostServiceNameSelector
    {

        /// <summary>
        /// we do not want the name on production to have anything other than pilot name 
        /// we also want to see more info easily on non production environments
        /// </summary>
        /// <param name="hostServiceType">eg. Plane, Airport</param>
        /// <param name="hostEnvironmentName">should come from config, Development, Docker</param>
        /// <param name="name">Any name identifier</param>
        /// <returns></returns>
        public static string AssignName(string hostServiceType, string hostEnvironmentName, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (hostEnvironmentName == "Development")//manual on premises launch from visual studio
                {
                    name = hostServiceType + "_" + hostEnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
                else if (hostEnvironmentName == "Docker")
                {
                    name = "Error - " + hostServiceType + "NameShouldHaveBeenGivenFor_" + hostEnvironmentName + "_Environment_" + new Random().Next(1001, 9999).ToString();
                }
                else
                {
                    name = "Warning - Unpredicted Environment - " + hostServiceType + "_" + hostEnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }
            else
            {
                if (hostEnvironmentName == "Development")//on premises launch from ps script
                {
                    
                }
                else if (hostEnvironmentName == "Docker")
                {
                    //production name - expected to be displayed as given from docker compose
                }
                else
                {
                    name += "Warning - Unpredicted Environment - " + hostServiceType + "_" + hostEnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }

            return name;
        }
    }
}
