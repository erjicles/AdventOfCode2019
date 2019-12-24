using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode2019.Intcode;

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
            var computer = new IntcodeComputer();
            computer.LoadProgram(program);
            computer.RunProgram();
            return computer.GetProgramCopy()[0];
        }

        public static int GetDay2Part2Answer()
        {
            // The inputs should still be provided to the program by replacing 
            // the values at addresses 1 and 2, just like before. In this program, 
            // the value placed in address 1 is called the noun, and the value 
            // placed in address 2 is called the verb. Each of the two input 
            // values will be between 0 and 99, inclusive.

            // Once the program has halted, its output is available at address 0, 
            // also just like before. Each time you try a pair of inputs, make 
            // sure you first reset the computer's memory to the values in the 
            // program (your puzzle input) - in other words, don't reuse memory 
            // from a previous attempt.
            //
            // Find the input noun and verb that cause the program to produce 
            // the output 19690720. What is 100 * noun + verb? 
            // (For example, if noun=12 and verb=2, the answer would be 1202.)
            // Answer: 2254
            var initialProgram = GetDay2Input();
            int verb = 0;
            bool foundResult = false;
            int noun;
            for (noun = 0; noun <= 99; noun++)
            {
                for (verb = 0; verb <= 99; verb++)
                {
                    initialProgram[1] = noun;
                    initialProgram[2] = verb;
                    var computer = new IntcodeComputer();
                    computer.LoadProgram(initialProgram);
                    computer.RunProgram();
                    var result = computer.GetProgramCopy();
                    if (result[0] == 19690720)
                    {
                        foundResult = true;
                        break;
                    }
                }
                if (foundResult)
                    break;
            }
            return (100 * noun) + verb;
        }  

        public static int[] GetDay2Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
