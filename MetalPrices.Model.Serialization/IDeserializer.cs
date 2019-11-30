namespace MetalPrices.Model.Serialization
{
    public interface IDeserializer<T>
    {
        T Deserialize(string json);
    }
}
