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
            var server = new Server();
            server.Url = "http://127.0.0.1";
            server.Port = GetPort();
            server.StartServer();
        }

        static int GetPort()
        {
            return int.Parse(Console.ReadLine());
        }
    }
}


