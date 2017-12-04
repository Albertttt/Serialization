using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ConsoleApp
{
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


        //todo: реализуйте интерфейс двумя классами - для json и для xml и используйте в коде ниже
        interface IISerializer()
        {
            string Serialze(object obj);
            object Deserialze(Type objType, string source);
        }

        static void Main(string[] args)
        {
            String str1 = Console.ReadLine();
            String str2 = Console.ReadLine();
            if (str1 == "Json")
            {
                String str3 = JsonConvert.SerializeObject(Result(JsonConvert.DeserializeObject<Input>(str2)));
                Console.WriteLine(str3.Replace(Environment.NewLine, "").Replace(" ", ""));
            }
            if (str1 == "Xml")
            {
                XmlSerializer res = new XmlSerializer(typeof(Input));
                Output x = Result((Input)res.Deserialize(new StringReader(str2)));
                MemoryStream temp = new MemoryStream();
                res = new XmlSerializer(typeof(Output));
                res.Serialize(temp, x);
                String str3 = Encoding.UTF8.GetString(temp.ToArray());
                str3 = str3.Remove(0, str3.IndexOf('>') + 1);
                str3 = str3.Remove(str3.IndexOf(' '), str3.IndexOf('>', str3.IndexOf(' ')) - str3.IndexOf(' ')).Replace(Environment.NewLine, string.Empty).Replace(" ", string.Empty);
                Console.WriteLine(str3);
            }
        }
    }
}