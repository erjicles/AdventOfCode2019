using AdventOfCode2019.Intcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day5
{
    /// <summary>
    /// Solution to the Day 5 challenge:
    /// https://adventofcode.com/2019/day/5
    /// </summary>
    public static class Day5
    {
        public const string FILE_NAME = "Day5Input.txt";

        public static void RunDay5Part1()
        {
            // Input 1, output final value
            // Answer: Diagnostic value output: 12896948
            var program = GetDay5Input();
            Console.WriteLine("----> When prompted, enter 1");
            var computer = new IntcodeComputer();
            computer.RunProgram(program);
        }

        public static void RunDay5Part2()
        {
            // Input 5, output final value
            // Answer: Diagnostic value output: 7704130
            var program = GetDay5Input();
            Console.WriteLine("----> When prompted, enter 5");
            var computer = new IntcodeComputer();
            computer.RunProgram(program);
        }

        public static int[] GetDay5Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
