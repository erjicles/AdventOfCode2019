using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day9
{
    /// <summary>
    /// Solution to the Day 9 challenge:
    /// https://adventofcode.com/2019/day/9
    /// </summary>
    public static class Day9
    {
        public const string FILE_NAME = "Day9Input.txt";
        public static BigInteger GetDay9Part1Answer()
        {
            // The BOOST program will ask for a single input; run it in test 
            // mode by providing it the value 1. It will perform a series of 
            // checks on each opcode, output any opcodes (and the associated 
            // parameter modes) that seem to be functioning incorrectly, and 
            // finally output a BOOST keycode.
            // Once your Intcode computer is fully functional, the BOOST 
            // program should report no malfunctioning opcodes when run in test 
            // mode; it should only output a single value, the BOOST keycode.
            // What BOOST keycode does it produce?
            // Answer: 2662308295
            var program = GetDay9Input();
            var inputProvider = new StaticValueInputProvider(1);
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            computer.RunProgram();
            return outputListener.Values[0];
        }

        public static BigInteger GetDay9Part2Answer()
        {
            // The program runs in sensor boost mode by providing the input 
            // instruction the value 2. Once run, it will boost the sensors 
            // automatically, but it might take a few seconds to complete the 
            // operation on slower hardware. In sensor boost mode, the program 
            // will output a single value: the coordinates of the distress signal.
            // Run the BOOST program in sensor boost mode.What are the 
            // coordinates of the distress signal?
            // Answer: 63441
            var program = GetDay9Input();
            var inputProvider = new StaticValueInputProvider(2);
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            computer.RunProgram();
            return outputListener.Values[0];
        }

        public static BigInteger[] GetDay9Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
