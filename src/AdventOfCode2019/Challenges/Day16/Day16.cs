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
            var result = GetFFT(input, 100, new int[] { 0, 1, 0, -1 });
            return result.Substring(0, 8);
        }

        public static string GetDay16Part2Answer()
        {
            // After repeating your input signal 10000 times and running 100 
            // phases of FFT, what is the eight-digit message embedded in the 
            // final output list?
            // Answer: 12482168
            var input = GetDay16Input();
            var result = GetAdvancedFFT(input, 100);
            return result;
        }

        public static string GetAdvancedFFT(string input, int numberOfPhases)
        {
            var inputBuilder = new StringBuilder(input.Length * 10000);
            for (int i = 0; i < 10000; i++)
                inputBuilder.Append(input);
            input = inputBuilder.ToString();
            int offset = int.Parse(input.Substring(0, 7));
            var currentOutput = input.Select(d => int.Parse(d.ToString())).ToArray();
            for (int i = 0; i < numberOfPhases; i++)
            {
                currentOutput = GetAdvancedPhaseOutput(currentOutput, offset);
            }
            var resultString = String.Join("", currentOutput);
            return resultString.Substring(offset, 8);
        }

        public static string GetFFT(string input, int numberOfPhases, IList<int> basePhasePattern)
        {
            var currentOutput = input;
            for (int i = 0; i < numberOfPhases; i++)
            {
                currentOutput = GetPhaseOutput(currentOutput, basePhasePattern);
            }
            return currentOutput;
        }

        public static int[] GetAdvancedPhaseOutput(int[] phaseInput, int offset)
        {
            if (offset > phaseInput.Length / 2)
            {
                var outputTotals = new int[phaseInput.Length];
                int cumulativeTotal = 0;
                for (int outputIndex = outputTotals.Length - 1; outputIndex >= offset; outputIndex--)
                {
                    cumulativeTotal += phaseInput[outputIndex];
                    outputTotals[outputIndex] = cumulativeTotal;
                }
                for (int i = offset; i < phaseInput.Length; i++)
                {
                    outputTotals[i] = int.Parse(outputTotals[i].ToString().Last().ToString());
                }
                return outputTotals;
            }

            throw new Exception("This only works for large offsets");
        }

        public static string GetPhaseOutput(
            string phaseInput,
            IList<int> basePhasePattern)
        {
            var result = new List<string>();
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
                // Get the last digit of the output total
                var outputString = outputTotal.ToString();
                result.Add(outputString.Substring(outputString.Length - 1, 1));
            }
            return String.Join("", result);
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
