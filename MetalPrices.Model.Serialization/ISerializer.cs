namespace MetalPrices.Model.Serialization
{
    public interface ISerializer<T>
    {
        string Serialize(T t);
    }
}
