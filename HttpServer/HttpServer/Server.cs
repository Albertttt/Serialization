using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Http
{
    class Server : ServerBase
    {        
        private Input input = null;
        private readonly Converter converter = new Converter();
        private readonly JsonSerializer serializer = new JsonSerializer();

        private readonly string[] methods = {"Ping", "PostInputData", "GetAnswer", "Stop", "GetInputData", "WriteAnswer"};
        private readonly Dictionary<string, MethodInfo> serverMethods = new Dictionary<string, MethodInfo>();

        public Server()
        {
            converter.SetFormat("Json");
            foreach (var method in methods)
            {
                serverMethods[method] = typeof(Server).GetMethod(method);
            }
        }

        public void StartServer()
        {
            Start(methods,serverMethods);
        }

        public void WriteAnswer()
        {
            var serializedInput = GetData();
            WriteMessage("");
            Console.WriteLine("Get answer:" + serializedInput);
        }

        public void GetInputData()
        {
            var input = new Input() {K = 10, Muls = new[] {1, 4}, Sums = new decimal[] {1.01m, 2.02m}};
            WriteMessage(serializer.Serialize(input));
            Console.WriteLine("Input object sended");
        }

        public void Stop()
        {
            WriteMessage("");
            StopServer();
        }

        public void Ping()
        {
             WriteMessage("");
        }

        public void GetAnswer()
        {
            if (input == null)
            {
                WriteMessage("");
                return;
            }
            var output = converter.GetOutputObject(input);
            var serializerOutput = converter.GetSerializedOutput(output);
            WriteMessage(serializerOutput);
        }

        public void PostInputData()
        {
            var serializedInput = GetData();
            input = serializer.Deserialize<Input>(serializedInput);
            WriteMessage("");
        }
    }
}