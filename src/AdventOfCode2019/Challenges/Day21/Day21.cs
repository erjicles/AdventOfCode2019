using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day21
{
    /// <summary>
    /// Solution to the Day 21 challenge:
    /// https://adventofcode.com/2019/day/21
    /// </summary>
    public class Day21
    {
        public const string FILE_NAME = "Day21Input.txt";

        public static BigInteger GetDay21Part1Answer()
        {
            // Program the springdroid with logic that allows it to survey the 
            // hull without falling into space. What amount of hull damage 
            // does it report?
            // Answer: 19360724
            BigInteger result = 0;
            Console.WriteLine("Starting Day 21 - Part 1...");
            var program = GetDay21Input();
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);

            // Define the robot instructions
            // Note - a jump will land 4 spaces ahead
            // Jump only if any of A, B, or C are holes
            // AND D is not a hole
            // ->
            // NOT A J <- JUMP is true if A is a hole
            // NOT B T <- TEMP is true if B is a hole
            // OR T J  <- JUMP is true if A or B is a hole
            // NOT C T <- TEMP is true if C is a hole
            // OR T J  <- JUMP is true if A or B or C is a hole
            // NOT D T <- TEMP is true if D is a hole
            // NOT T T <- TEMP is true if D is *not* a hole
            // AND T J <- JUMP is true if (A or B or C is a hole) AND (D is not a hole)
            var robotInstructions = new List<string>()
            {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "NOT D T",
                "NOT T T",
                "AND T J",
                "WALK"
            };
            var robotInstructionAsciiInput = GetRobotInstructionAsciiInputValues(robotInstructions);
            inputProvider.Values.AddRange(robotInstructionAsciiInput);

            computer.RunProgram();
            // If the robot successfully made it across, then the last output
            // value will be a large non-ascii character
            if (outputListener.Values.Count > 0 && outputListener.Values.Last() > 255)
            {
                result = outputListener.Values.Last();
                outputListener.Values.RemoveAt(outputListener.Values.Count - 1);
            }
            IntcodeComputerHelper.DrawAsciiOutput(outputListener.Values);
            return result;
        }

        public static BigInteger GetDay21Part2Answer()
        {
            // Successfully survey the rest of the hull by ending your program 
            // with RUN. What amount of hull damage does the springdroid now 
            // report?
            // Answer: 1140450681
            BigInteger result = 0;
            Console.WriteLine("Starting Day 21 - Part 2...");
            var program = GetDay21Input();
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);

            // Define the robot instructions
            // Note - a jump will still land 4 spaces ahead
            // Jump if:
            // There is a hole in A, B, or C
            // And D is not a hole
            // And (H OR (E AND (I OR F)))
            // OR A J  |\
            // AND B J | | A, B, or C is a hole: !(A AND B AND C)
            // AND C J | |
            // NOT J J |/
            // AND D J | AND D is a block
            // OR F T  |\ (I OR F)
            // OR I T  |/
            // AND E T | E AND (I OR F)
            // OR H T  | H OR (E AND (I OR F))
            // AND T J | final
            var robotInstructions = new List<string>()
            {
                "OR A J ",
                "AND B J",
                "AND C J",
                "NOT J J",
                "AND D J",
                "OR F T ",
                "OR I T ",
                "AND E T",
                "OR H T ",
                "AND T J",
                "RUN"
            };
            var robotInstructionAsciiInput = GetRobotInstructionAsciiInputValues(robotInstructions);
            inputProvider.Values.AddRange(robotInstructionAsciiInput);

            computer.RunProgram();
            // If the robot successfully made it across, then the last output
            // value will be a large non-ascii character
            if (outputListener.Values.Count > 0 && outputListener.Values.Last() > 255)
            {
                result = outputListener.Values.Last();
                outputListener.Values.RemoveAt(outputListener.Values.Count - 1);
            }
            IntcodeComputerHelper.DrawAsciiOutput(outputListener.Values);
            return result;
        }

        public static IList<BigInteger> GetRobotInstructionAsciiInputValues(IList<string> robotInstructions)
        {
            var result = new List<BigInteger>();
            foreach (var instruction in robotInstructions)
            {
                var trimmedInstruction = instruction.Trim();
                for (int i = 0; i < trimmedInstruction.Length; i++)
                {
                    result.Add(char.ConvertToUtf32(trimmedInstruction, i));
                }
                result.Add(10);
            }
            return result;
        }

        public static BigInteger[] GetDay21Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
