using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
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
            var basePhasePattern = new int[] { 0, 1, 0, -1 };
            var input = GetDay16Input();
            var result = GetFFT(input, 100, basePhasePattern);
            return result.Substring(0, 8);
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
            
            // Get the number of times each phase entry should be repeated
            int numberOfTimesToRepeatEachEntry = outputIndex + 1;

            // For the first repetition of the phase pattern, shift right to
            // exclude the first entry
            // For all subsequent repetitions, include the full pattern
            int normalPhasePatternLength = basePhasePattern.Count * numberOfTimesToRepeatEachEntry;
            int firstPhasePatternLength = normalPhasePatternLength - 1;

            // Get the modded index of the phase pattern to use
            int indexToUse;
            if (inputIndex < firstPhasePatternLength)
            {
                // For the first repetition, shift right by one
                indexToUse = inputIndex + 1;
            }
            else
            {
                // For subsequen repetitions, do not shift right
                inputIndex -= firstPhasePatternLength;
                indexToUse = inputIndex % normalPhasePatternLength;
            }
            // Convert the phase pattern index to the entry index of the base
            // pattern
            int basePatternIndex = indexToUse / numberOfTimesToRepeatEachEntry;
            int phaseValue = basePhasePattern[basePatternIndex];
            return phaseValue;
        }

        public static string GetDay16Input()
        {
            string input = FileHelper.ReadInputFileAsString(FILE_NAME);
            return input;
        }
    }
}
