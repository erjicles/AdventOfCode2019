using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day2
{
    /// <summary>
    /// Solution to the Day 2 challenge:
    /// https://adventofcode.com/2019/day/2
    /// </summary>
    public static class Day2
    {
        public const string FILE_NAME = "Day2Input.txt";

        public static int GetDay2Part1Answer()
        {
            // Once you have a working computer, the first step is to restore 
            // the gravity assist program (your puzzle input) to the "1202 
            // program alarm" state it had just before the last computer caught
            // fire. 
            // To do this, before running the program, replace position 1 with 
            // the value 12 and replace position 2 with the value 2. 
            // What value is left at position 0 after the program halts?
            // Answer: 11590668
            var program = GetDay2Input();
            program[1] = 12;
            program[2] = 2;
            var result = RunProgram(program);
            return result[0];
        }

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

        public static int[] GetDay2Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Cannot locate file {filePath}");
            }
            var inputText = File.ReadAllText(filePath);
            return inputText.Split(",").Select(v => int.Parse(v)).ToArray();
        }
    }
}
