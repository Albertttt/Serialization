using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

//todo: методы элтого класса должны работать с нормальными объектами Inpt, Output, а не сбайтовыми массивами
public class HttpClient
{
    //todo: сделай host, port параметром класса, а не методов
    public byte Ping(string host, int port)
    {
        HttpStatusCode status;
        Http_query("GET", host, port, "Ping", out status);
        if (status == HttpStatusCode.OK)
            //todo: может лучше возвращать true/false?
            return 1;
        else
            return 0;
    }
    public byte[] GetInputData(string host, int port)
    {
        HttpStatusCode status;
        return Http_query("GET", host, port, "GetInputData", out status);
    }
    public void WriteAnswer(string host, int port, byte[] Ans)
    {
        HttpStatusCode status;
        Http_query("POST", host, port, "WriteAnswer", out status, Ans);
    }
    private byte[] Http_query(string scheme, string host, int port, string method, out HttpStatusCode status, byte[] tmp = null)
    {
        UriBuilder home = new UriBuilder("http", "127.0.0.1", port, method);
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

    static void Main(string[] args)
    {
        int k = int.Parse(Console.ReadLine());
        HttpClient Client = new HttpClient();
        if (Client.Ping("127.0.0.1", k) == 1)
        {
            String str1 = System.Text.Encoding.UTF8.GetString(Client.GetInputData("127.0.0.1", k));
            String str2 = JsonConvert.SerializeObject(Result(JsonConvert.DeserializeObject<Input>(str1)));
            String str3 = str2.Replace(Environment.NewLine, "").Replace(" ", "");
            Client.WriteAnswer("127.0.0.1", k, System.Text.Encoding.UTF8.GetBytes(str3));
        }
    }
}