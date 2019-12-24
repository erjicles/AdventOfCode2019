using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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

        public static ConsoleOutputListener RunDay5Part1()
        {
            // Input 1, output final value
            // Answer: Diagnostic value output: 12896948
            var program = GetDay5Input();
            var outputListener = new ConsoleOutputListener();
            var computer = new IntcodeComputer(new StaticValueInputProvider(1), outputListener);
            computer.LoadProgram(program);
            computer.RunProgram();
            return outputListener;
        }

        public static ConsoleOutputListener RunDay5Part2()
        {
            // Input 5, output final value
            // Answer: Diagnostic value output: 7704130
            var program = GetDay5Input();
            var outputListener = new ConsoleOutputListener();
            var computer = new IntcodeComputer(new StaticValueInputProvider(5), outputListener);
            computer.LoadProgram(program);
            computer.RunProgram();
            return outputListener;
        }

        public static BigInteger[] GetDay5Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
