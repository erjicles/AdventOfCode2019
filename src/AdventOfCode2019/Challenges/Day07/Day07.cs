using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day07
{
    /// <summary>
    /// Solution to the Day 7 challenge:
    /// https://adventofcode.com/2019/day/7
    /// </summary>
    public class Day07
    {
        public const string FILE_NAME = "Day07Input.txt";

        public static int GetDay7Part1Answer()
        {
            // Try every combination of phase settings on the amplifiers. 
            // What is the highest signal that can be sent to the thrusters?
            // Answer: 255840
            var program = GetDay7Input();
            return GetMaximumAmplifierOutput(
                initialInput:0,
                numberOfAmplifiers:5,
                program:program,
                feedbackMode:FeedbackMode.Normal);
        }

        public static int GetDay7Part2Answer()
        {
            // Try every combination of the new phase settings on the amplifier 
            // feedback loop. What is the highest signal that can be sent to 
            // the thrusters?
            // Clarification on problem instructions found here:
            // https://old.reddit.com/r/adventofcode/comments/e7aqcb/2019_day_7_part_2_confused_with_the_question/
            // Answer: 84088865
            var program = GetDay7Input();
            return GetMaximumAmplifierOutput(
                initialInput: 0,
                numberOfAmplifiers: 5,
                program: program,
                feedbackMode: FeedbackMode.Loop);
        }

        public static int GetMaximumAmplifierOutput(
            int initialInput,
            int numberOfAmplifiers,
            BigInteger[] program,
            FeedbackMode feedbackMode)
        {
            int maxOutput = int.MinValue;
            int[] phaseValues;
            if (FeedbackMode.Normal.Equals(feedbackMode))
            {
                // Phase values between 1 and N
                phaseValues = Enumerable.Range(0, numberOfAmplifiers).ToArray();
            }
            else if (FeedbackMode.Loop.Equals(feedbackMode))
            {
                // Phase values between N and 2N
                phaseValues = Enumerable.Range(numberOfAmplifiers, numberOfAmplifiers).ToArray();
            }
            else
            {
                throw new Exception($"Invalid feedback mode {feedbackMode}");
            }
            var phaseValuesPermutations = new Permutations<int>(phaseValues);

            foreach (IList<int> amplifierPhaseSettings in phaseValuesPermutations)
            {
                var output = GetAmplifierOutput(
                    initialInput,
                    amplifierPhaseSettings.ToArray(),
                    program,
                    feedbackMode);
                if (output > maxOutput)
                    maxOutput = output;
            }
            return maxOutput;
        }

        public static int GetAmplifierOutput(
            int initialInput, 
            int[] phaseSettings, 
            BigInteger[] program,
            FeedbackMode feedbackMode)
        {
            // Initialize the amplifiers
            var numberOfAmplifiers = phaseSettings.Length;
            var amplifiers = new List<Tuple<IntcodeComputer, BufferedInputProvider, ListOutputListener>>(numberOfAmplifiers);
            for (int i = 0; i < numberOfAmplifiers; i++)
            {
                var inputProvider = new BufferedInputProvider();
                inputProvider.AddInputValue(phaseSettings[i]);
                if (i == 0)
                {
                    inputProvider.AddInputValue(initialInput);
                }
                var outputListener = new ListOutputListener();
                var computer = new IntcodeComputer(inputProvider, outputListener);
                computer.LoadProgram(program);
                amplifiers.Add(new Tuple<IntcodeComputer, BufferedInputProvider, ListOutputListener>(
                    computer,
                    inputProvider,
                    outputListener));
            }

            int currentAmplifierIndex = 0;
            int round = 1;
            BigInteger output;
            while (true)
            {
                var currentAmplifier = amplifiers[currentAmplifierIndex];
                var computer = currentAmplifier.Item1;
                var outputListener = currentAmplifier.Item3;
                var status = computer.RunProgram();
                if (!IntcodeProgramStatus.AwaitingInput.Equals(status)
                    && !IntcodeProgramStatus.Completed.Equals(status))
                {
                    throw new Exception($"Program halted with invalid status: {status}");
                }
                if (outputListener.Values.Count == 0)
                    throw new Exception("No output received");
                output = outputListener.Values.Last();

                // The program has completed, break out
                if (currentAmplifierIndex == numberOfAmplifiers - 1
                    && IntcodeProgramStatus.Completed.Equals(status))
                {
                    break;
                }

                // The program hasn't finished...
                // Pass the output from this amplifier to the input of the next
                var nextAmplifierIndex = currentAmplifierIndex + 1;
                if (nextAmplifierIndex >= numberOfAmplifiers)
                    nextAmplifierIndex = 0;
                var nextAmplifier = amplifiers[nextAmplifierIndex];
                var nextAmplifierInputProvider = nextAmplifier.Item2;
                nextAmplifierInputProvider.AddInputValue(output);

                currentAmplifierIndex++;
                if (currentAmplifierIndex == numberOfAmplifiers)
                {
                    if (FeedbackMode.Normal.Equals(feedbackMode))
                        break;
                    else if (FeedbackMode.Loop.Equals(feedbackMode))
                    {
                        currentAmplifierIndex = 0;
                        round++;
                    }
                }
            }

            return (int)output;
        }

        public static BigInteger[] GetDay7Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }

    public enum FeedbackMode
    {
        Normal = 0,
        Loop = 1
    }
}
