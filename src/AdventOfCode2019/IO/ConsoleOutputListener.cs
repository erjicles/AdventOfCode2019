using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ConsoleOutputListener : IOutputListener
    {
        public IList<BigInteger> OutputValues { get; set; } = new List<BigInteger>();
        public void SendOutput(BigInteger value)
        {
            OutputValues.Add(value);
            Console.WriteLine($"---->Output value: {value}");
        }
    }
}
