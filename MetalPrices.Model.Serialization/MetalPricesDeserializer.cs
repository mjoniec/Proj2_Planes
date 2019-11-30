using Newtonsoft.Json;

namespace MetalPrices.Model.Serialization
{
    public class MetalPricesDeserializer : IDeserializer<MetalPrices>
    {
        public MetalPrices Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            var metalPrice = JsonConvert.DeserializeObject<MetalPrices>(json);

            return metalPrice;
        }
    }
}
