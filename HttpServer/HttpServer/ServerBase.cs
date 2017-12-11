using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Http
{
    class ServerBase
    {
        private readonly HttpListener server = new HttpListener();
        private bool isNeedFinish = false;
        private HttpListenerContext serverContext;

        public string Url { get; set; }
        public int Port { get; set; }

        public void Start(string[] methodNames, Dictionary<string, MethodInfo> methods)
        {
            foreach (var method in methodNames)
            {
                server.Prefixes.Add($"{Url}:{Port}/{method}/");
            }

            server.Start();
            while (true)
            {
                try
                {
                    if (isNeedFinish) return;
                    serverContext = server.GetContext();
                    serverContext.Response.StatusCode = (int) HttpStatusCode.OK;

                    foreach (var method in methodNames)
                    {
                        if (serverContext.Request.RawUrl.Contains(method))
                        {
                            methods[method].Invoke(this, null);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public string GetData()
        {
            var reader = new StreamReader(serverContext.Request.InputStream);
            return reader.ReadToEnd();
        }

        public void WriteMessage(string message)
        {
            serverContext.Response.ContentLength64 = Encoding.UTF8.GetByteCount(message);
            serverContext.Response.KeepAlive = false;
            using (Stream stream = serverContext.Response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(message);
                }
            }
        }

        public void StopServer()
        {
            isNeedFinish = true;
            server.Stop();
        }
    }
}