using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public static class IntcodeComputer
    {
        public static int[] RunProgram(int[] program)
        {
            var result = new int[program.Length];
            Array.Copy(program, result, program.Length);
            int position = 0;
            while (result[position] != 99)
            {
                var opcode = result[position];
                if (opcode == 1)
                {
                    var val1 = result[result[position + 1]];
                    var val2 = result[result[position + 2]];
                    result[result[position + 3]] = val1 + val2;
                    position += 4;
                }
                else if (opcode == 2)
                {
                    var val1 = result[result[position + 1]];
                    var val2 = result[result[position + 2]];
                    result[result[position + 3]] = val1 * val2;
                    position += 4;
                }
                else if (opcode != 99)
                {
                    throw new Exception($"Invalid opcode {result[position]} at position {position}");
                }
            }
            return result;
        }
    }
}
