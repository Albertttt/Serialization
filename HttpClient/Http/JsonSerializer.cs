using Newtonsoft.Json;

namespace Http
{
    public class JsonSerializer : ISerializer
    {

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string bytes)
        {
            return JsonConvert.DeserializeObject<T>(bytes);
        }

        public bool Satisfy(string serializerFormat)
        {
            return serializerFormat == "Json";
        }
    }
}