using System;
using System.Collections.Generic;
using System.Linq;

namespace Http
{
    public class Converter
    {
        private string serializerFormat;
        private readonly List<ISerializer> serializers = new List<ISerializer>();

        public string GetFormat()
        {
            serializerFormat = Console.ReadLine();
            return Console.ReadLine();
        }

        public Converter()
        {
            serializers.Add(new JsonSerializer());
        }

        public void SetFormat(string type)
        {
            this.serializerFormat = type;
        }

        public ISerializer GetSerializer(string serializerFormat)
        {
            return serializers.First(x => x.Satisfy(serializerFormat));
        }

        public Input GetInputObject(string serializedString)
        {
            return GetSerializer(serializerFormat).Deserialize<Input>(serializedString);
        }

        public Output GetOutputObject(Input input)
        {
            var output = new Output();

            foreach (var sum in input.Sums)
            {
                output.SumResult += sum;
            }
            output.SumResult *= input.K;

            output.MulResult = 1;
            foreach (var mul in input.Muls)
            {
                output.MulResult *= mul;
            }

            output.SortedInputs = input.Sums.Concat(input.Muls.Select(x => (decimal) x)).ToArray();
            Array.Sort(output.SortedInputs);

            return output;
        }

        public string GetSerializedOutput(Output output)
        {
            return GetSerializer(serializerFormat).Serialize(output)
                .Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }
    }
}