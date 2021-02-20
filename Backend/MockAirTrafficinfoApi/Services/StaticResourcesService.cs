using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MockAirTrafficinfoApi.Services
{
    public class StaticResourcesService
    {
        public async Task<string> GetWorldMap()
        {
            string json;

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                + "//worldMap.json";

            json = await File.ReadAllTextAsync(path);

            return json;
        }
    }
}
