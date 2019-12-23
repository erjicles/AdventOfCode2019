using AdventOfCode2019.Intcode;
using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day7
{
    /// <summary>
    /// Solution to the Day 7 challenge:
    /// https://adventofcode.com/2019/day/7
    /// </summary>
    public class Day7
    {
        public const string FILE_NAME = "Day7Input.txt";
        public static int GetDay7Part1Answer()
        {
            // Try every combination of phase settings on the amplifiers. 
            // What is the highest signal that can be sent to the thrusters?
            // Answer: 255840
            var program = GetDay7Input();
            return GetMaximumAmplifierOutput(
                0,
                5,
                program);
        }

        public static int GetMaximumAmplifierOutput(
            int initialInput,
            int numberOfAmplifiers,
            int[] program)
        {
            int maxOutput = int.MinValue;
            var phaseValues = Enumerable.Range(0, numberOfAmplifiers).ToArray();
            var phaseValuesPermutations = new Permutations<int>(phaseValues);

            foreach (IList<int> amplifierPhaseSettings in phaseValuesPermutations)
            {
                var output = GetAmplifierOutput(
                    initialInput,
                    amplifierPhaseSettings.ToArray(),
                    program);
                if (output > maxOutput)
                    maxOutput = output;
            }
            return maxOutput;
        }

        public static int GetAmplifierOutput(
            int initialInput, 
            int[] phaseSettings, 
            int[] program)
        {
            int inputValue = initialInput;
            for (int i = 0; i < phaseSettings.Length; i++)
            {
                var inputProvider = new StaticValueInputProvider(
                    new int[] { phaseSettings[i], inputValue });
                var outputListener = new ListOutputListener();
                var computer = new IntcodeComputer(inputProvider, outputListener);
                computer.RunProgram(program);
                if (outputListener.Values.Count == 0)
                    throw new Exception("No output received");
                var output = outputListener.Values[outputListener.Values.Count - 1];
                inputValue = output;
            }
            return inputValue;
        }

        public static int[] GetDay7Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
