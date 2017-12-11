using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Http
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new Converter();
            converter.SetFormat("Json");
            var client = new MyClient();

            client.Port = GetPort();

            while (!client.Ping());

            var input = client.GetInpuData();
            var output = converter.GetOutputObject(input);

            client.PostAnswer(output);
        }

        static int GetPort()
        {
            return int.Parse(Console.ReadLine());
        }
    }
}


