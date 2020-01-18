using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public static class IntcodeComputerHelper
    {
        public static void DrawAsciiOutput(IList<BigInteger> outputValues)
        {
            foreach (var value in outputValues)
            {
                Console.Write(char.ConvertFromUtf32((int)value));
            }
        }
    }
}
