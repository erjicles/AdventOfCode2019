using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ConsoleOutputListener : IOutputListener
    {
        public IList<int> OutputValues { get; set; } = new List<int>();
        public void SendOutput(int value)
        {
            OutputValues.Add(value);
            Console.WriteLine($"---->Output value: {value}");
        }
    }
}
