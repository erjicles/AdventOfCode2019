using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ConsoleOutputListener : IOutputListener
    {
        public void SendOutput(int value)
        {
            Console.WriteLine($"---->Output value: {value}");
        }
    }
}
