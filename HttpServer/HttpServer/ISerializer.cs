namespace Http
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string bytes);
        bool Satisfy(string serializerFormat);
    }
}