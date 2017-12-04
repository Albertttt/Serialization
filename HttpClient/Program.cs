using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

public class HttpClient
{
    string host = "127.0.0.1";
    public int port;
    public bool Ping()
    {
        HttpStatusCode status;
        Http_query("GET", "Ping", out status);
        if (status == HttpStatusCode.OK)
            return true;
        else
            return false;
    }
    public byte[] GetInputData()
    {
        HttpStatusCode status;
        return Http_query("GET", "GetInputData", out status);
    }
    public void WriteAnswer(byte[] Ans)
    {
        HttpStatusCode status;
        Http_query("POST", "WriteAnswer", out status, Ans);
    }
    private byte[] Http_query(string scheme, string method, out HttpStatusCode status, byte[] tmp = null)
    {
        UriBuilder home = new UriBuilder("http", host, port, method);
        WebRequest query = WebRequest.CreateHttp(home.Uri);
        query.Method = scheme;
        if (tmp != null)
        {
            Stream stream_query = query.GetRequestStream();
            stream_query.Write(tmp, 0, tmp.Length);
        }
        HttpWebResponse res = (HttpWebResponse)query.GetResponse();
        status = res.StatusCode;
        Stream stream_res = res.GetResponseStream();
        if (stream_res.Length > 0)
        {
            MemoryStream mem = new MemoryStream();
            stream_res.CopyTo(mem);
            return mem.ToArray();
        }
        return null;
    }
}

public class Output
{
    public decimal SumResult { get; set; }
    public int MulResult { get; set; }
    public decimal[] SortedInputs { get; set; }
}
public class Input
{
    public int K { get; set; }
    public decimal[] Sums { get; set; }
    public int[] Muls { get; set; }
}

class Program
{
    static public Output Result(Input b)
    {
        Output a = new Output();
        a.MulResult = b.Muls.Aggregate((p, x) => p = p * x);
        a.SumResult = b.Sums.Sum() * b.K;
        List<decimal> q = b.Sums.ToList();
        int i = 0;
        while (i < b.Muls.Length)
        {
            q.Add(Convert.ToDecimal(b.Muls[i]));
            i++;
        }
        q.Sort();
        a.SortedInputs = q.ToArray();
        return a;
    }

    static void Main()
    {
        HttpClient Client = new HttpClient();
        Client.port = int.Parse(Console.ReadLine());
        if (Client.Ping())
        {
            String str1 = System.Text.Encoding.UTF8.GetString(Client.GetInputData());
            String str2 = JsonConvert.SerializeObject(Result(JsonConvert.DeserializeObject<Input>(str1)));
            String str3 = str2.Replace(Environment.NewLine, "").Replace(" ", "");
            Client.WriteAnswer(System.Text.Encoding.UTF8.GetBytes(str3));
        }
    }
}