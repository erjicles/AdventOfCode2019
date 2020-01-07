using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day16
{
    /// <summary>
    /// Solution to the Day 16 challenge:
    /// https://adventofcode.com/2019/day/16
    /// </summary>
    public class Day16
    {
        public const string FILE_NAME = "Day16Input.txt";

        public static string GetDay16Part1Answer()
        {
            // After 100 phases of FFT, what are the first eight digits in 
            // the final output list?
            // Answer: 44098263
            var input = GetDay16Input();
            var result = GetFFT(
                input: input, 
                numberOfPhases: 100, 
                basePhasePattern: new int[] { 0, 1, 0, -1 },
                inputRepeatCount: 1,
                skip: 0);
            return result.Substring(0, 8);
        }

        public static string GetDay16Part2Answer()
        {
            // After repeating your input signal 10000 times and running 100 
            // phases of FFT, what is the eight-digit message embedded in the 
            // final output list?
            // Answer: 12482168
            var input = GetDay16Input();
            int numberToSkip = int.Parse(input.Substring(0, 7));
            var result = GetFFT(
                input: input,
                numberOfPhases: 100,
                basePhasePattern: new int[] { 0, 1, 0, -1 },
                inputRepeatCount: 10000,
                skip: numberToSkip);
            return result.Substring(0, 8);
        }

        public static string GetFFT(
            string input, 
            int numberOfPhases, 
            IList<int> basePhasePattern,
            int inputRepeatCount,
            int skip)
        {
            // In general, computing each pass requires O(n^2) operations
            // because each output digit is dependent on all of the input
            // digits.
            // However, in the special case when we are skipping at least half
            // of the output digits, and the first entry in the base phase
            // pattern is zero, then we observe that each output digit 
            // (after skipping the requested number of digits) is only a
            // cumulative sum of the corresponding input digits following it.
            // So for each phase, we only need to calculate the sum of the
            // input digits at the skip index and after, which is O(n).

            int totalInputLength = input.Length * inputRepeatCount;
            if (skip >= totalInputLength)
                throw new Exception("Skipping more digits than exist");

            // Construct the input
            var inputBuilder = new StringBuilder();
            for (int i = 0; i < inputRepeatCount; i++)
                inputBuilder.Append(input);

            // Determine if we can use the fast algorithm
            bool canUseFastAlgorithm = (skip >= inputBuilder.Length / 2)
                && (basePhasePattern.Count >= 1 && basePhasePattern[0] == 0);
            if (canUseFastAlgorithm)
            {
                var currentOutput = inputBuilder.ToString(skip, totalInputLength - skip)
                    .Select(d => int.Parse(d.ToString()))
                    .ToArray();
                int multiplier = basePhasePattern.Count > 1 ? basePhasePattern[1] : 0;
                for (int phaseIndex = 0; phaseIndex < numberOfPhases; phaseIndex++)
                {
                    currentOutput = GetFastPhaseOutput(currentOutput, multiplier);
                }
                return String.Join("", currentOutput);
            }
            else
            {
                var currentOutput = inputBuilder.ToString()
                    .Select(d => int.Parse(d.ToString()))
                    .ToArray();
                for (int phaseIndex = 0; phaseIndex < numberOfPhases; phaseIndex++)
                {
                    currentOutput = GetPhaseOutput(currentOutput, basePhasePattern);
                }
                return String.Join("", currentOutput);
            }
        }

        public static int[] GetFastPhaseOutput(int[] phaseInput, int multiplier)
        {
            var outputTotals = new int[phaseInput.Length];
            int cumulativeTotal = 0;
            for (int outputIndex = outputTotals.Length - 1; outputIndex >= 0; outputIndex--)
            {
                cumulativeTotal += phaseInput[outputIndex] * multiplier;
                outputTotals[outputIndex] = GetOnesDigit(cumulativeTotal);
            }
            return outputTotals;
        }

        public static int[] GetPhaseOutput(
            int[] phaseInput,
            IList<int> basePhasePattern)
        {
            var result = new int[phaseInput.Length];
            for (int outputIndex = 0; outputIndex < phaseInput.Length; outputIndex++)
            {
                int outputTotal = 0;
                for (int inputIndex = 0; inputIndex < phaseInput.Length; inputIndex++)
                {
                    int phaseEntry = GetPhaseEntry(basePhasePattern, inputIndex, outputIndex);
                    int inputValue = int.Parse(phaseInput[inputIndex].ToString());
                    int outputContribution = inputValue * phaseEntry;
                    outputTotal += outputContribution;
                }
                result[outputIndex] = GetOnesDigit(outputTotal);
            }
            return result;
        }

        public static int GetOnesDigit(int value)
        {
            int result = Math.Abs(value) % 10;
            return result;
        }

        public static int GetPhaseEntry(
            IList<int> basePhasePattern,
            int inputIndex,
            int outputIndex)
        {
            // While each element in the output array uses all of the same 
            // input array elements, the actual repeating pattern to use 
            // depends on which output element is being calculated. The base 
            // pattern is 0, 1, 0, -1. Then, repeat each value in the pattern 
            // a number of times equal to the position in the output list 
            // being considered. Repeat once for the first element, twice for 
            // the second element, three times for the third element, and so 
            // on. So, if the third element of the output list is being 
            // calculated, repeating the values would produce: 
            // 0, 0, 0, 1, 1, 1, 0, 0, 0, -1, -1, -1.
            // When applying the pattern, skip the very first value exactly 
            // once. (In other words, offset the whole pattern left by one.) 
            // So, for the second element of the output list, the actual 
            // pattern used would be: 
            // 0, 1, 1, 0, 0, -1, -1, 0, 0, 1, 1, 0, 0, -1, -1, ....
            int numberOfTimesToRepeatEachEntry = outputIndex + 1;
            int normalPhasePatternLength = basePhasePattern.Count * numberOfTimesToRepeatEachEntry;
            int basePatternIndex = ((inputIndex + 1) % normalPhasePatternLength) / numberOfTimesToRepeatEachEntry;
            int phaseValue = basePhasePattern[basePatternIndex];
            return phaseValue;
        }

        public static int GetBasePhasePatternIndex(int inputIndex, int outputIndex)
        {
            int numberOfTimesToRepeatEachEntry = outputIndex + 1;
            int phasePatternLength = 4 * numberOfTimesToRepeatEachEntry;
            int basePatternIndex = ((inputIndex + 1) % phasePatternLength) 
                / numberOfTimesToRepeatEachEntry;
            return basePatternIndex;
        }

        public static string GetDay16Input()
        {
            string input = FileHelper.ReadInputFileAsString(FILE_NAME);
            return input;
        }
    }
}
