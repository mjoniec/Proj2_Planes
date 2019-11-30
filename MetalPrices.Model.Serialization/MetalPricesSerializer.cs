using Newtonsoft.Json;

namespace MetalPrices.Model.Serialization
{
    public class MetalPricesSerializer : ISerializer<MetalPrices>
    {
        public string Serialize(MetalPrices metalPrices)
        {
            return JsonConvert.SerializeObject(metalPrices);
        }
    }
}
