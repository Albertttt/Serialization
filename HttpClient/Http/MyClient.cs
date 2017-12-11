using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Http
{
    class MyClient
    { 
        private readonly JsonSerializer serializer = new JsonSerializer();
        private static string url = "http://127.0.0.1";
        public int Port { get; set; }

        public bool Ping()
        {
            string answer;
            while (!SendRequest("Ping", MethodType.GET, null, out answer));
            return true;
        }

        public Input GetInpuData()
        {
            string answer;
            while (!SendRequest("GetInputData", MethodType.GET, null, out answer) || string.IsNullOrEmpty(answer));
            return serializer.Deserialize<Input>(answer);
        }

        public void PostAnswer<T>(T data) where T : class
        {
            string answer;
            var serialized = serializer.Serialize(data);
            while (!SendRequest("WriteAnswer", MethodType.POST, serialized, out answer));
        }


        bool SendRequest(string method, MethodType type, string body, out string answer)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(BuildUrl(method));
            webRequest.Timeout = 1000;
            webRequest.Method = type.ToString();

            answer = string.Empty;

            try
            {
                if (body != null)
                {
                    var data = Encoding.UTF8.GetBytes(body);
                    webRequest.ContentLength = data.Length;
                    var requestStream = webRequest.GetRequestStream();
                    using (var writer = new StreamWriter(requestStream))
                    {
                        writer.Write(body);
                    }
                }

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                var responseStream = webResponse.GetResponseStream();
                if (responseStream != null && responseStream.CanRead)
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        answer = streamReader.ReadToEnd();
                        return true;
                    }
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout || ex.Status == WebExceptionStatus.ReceiveFailure ||
                                ex.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    answer = string.Empty;
                    return false;
                }
                throw;
            }
        }

        private string BuildUrl(string method)
        {
            return $"{url}:{Port}/{method}";
        }
    }
}