using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ListOutputListener : IOutputListener
    {
        public List<BigInteger> Values = new List<BigInteger>();

        public void SendOutput(BigInteger value)
        {
            Values.Add(value);
        }
    }
}
