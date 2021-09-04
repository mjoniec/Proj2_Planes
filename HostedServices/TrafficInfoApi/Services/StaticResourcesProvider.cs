using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace TrafficInfoApi.Services
{
    //remove after fix https://github.com/mjoniec/Proj2_Planes/issues/17
    public class StaticResourcesProvider
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
