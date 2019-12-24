using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ListOutputListener : IOutputListener
    {
        public List<int> Values = new List<int>();

        public void SendOutput(int value)
        {
            Values.Add(value);
        }
    }
}
