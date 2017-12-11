using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

interface ISerializer
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string str);
}

class JsonSerializer : ISerializer
{
    public T Deserialize<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}

class XmlSerializer : ISerializer
{
    public T Deserialize<T>(string str)
    {
        var res = new System.Xml.Serialization.XmlSerializer(typeof(T));
        StringReader x = new StringReader(str);
        return (T)res.Deserialize(x);
    }         
    public string Serialize<T>(T obj)
    {
        var res = new System.Xml.Serialization.XmlSerializer(typeof(T));
        MemoryStream temp = new MemoryStream();
        res.Serialize(temp, obj);
        String str3 = Encoding.UTF8.GetString(temp.ToArray());
        str3 = str3.Remove(0, str3.IndexOf('>') + 1);
        str3 = str3.Remove(str3.IndexOf(' '), str3.IndexOf('>', str3.IndexOf(' ')) - str3.IndexOf(' '));
        return str3;
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
        ISerializer ser;
        String str1 = Console.ReadLine();
        String str2 = Console.ReadLine();
        if (str1 == "Json")
            ser = new JsonSerializer();
        else
            ser = new XmlSerializer();
        Console.WriteLine(ser.Serialize(Result(ser.Deserialize<Input>(str2))).Replace(Environment.NewLine, "").Replace(" ", string.Empty));
    }
}