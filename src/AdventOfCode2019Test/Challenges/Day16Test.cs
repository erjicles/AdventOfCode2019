using AdventOfCode2019.Challenges.Day16;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day16Test
    {
        [Fact]
        public static void GetPhaseEntryTest()
        {
            // Test exampels taken from here:
            // https://adventofcode.com/2019/day/16
            // Input signal: 12345678
            // 1 * 1 + 2 * 0 + 3 * -1 + 4 * 0 + 5 * 1 + 6 * 0 + 7 * -1 + 8 * 0 = 4
            // 1 * 0 + 2 * 1 + 3 * 1 + 4 * 0 + 5 * 0 + 6 * -1 + 7 * -1 + 8 * 0 = 8
            // 1 * 0 + 2 * 0 + 3 * 1 + 4 * 1 + 5 * 1 + 6 * 0 + 7 * 0 + 8 * 0 = 2
            // 1 * 0 + 2 * 0 + 3 * 0 + 4 * 1 + 5 * 1 + 6 * 1 + 7 * 1 + 8 * 0 = 2
            // 1 * 0 + 2 * 0 + 3 * 0 + 4 * 0 + 5 * 1 + 6 * 1 + 7 * 1 + 8 * 1 = 6
            // 1 * 0 + 2 * 0 + 3 * 0 + 4 * 0 + 5 * 0 + 6 * 1 + 7 * 1 + 8 * 1 = 1
            // 1 * 0 + 2 * 0 + 3 * 0 + 4 * 0 + 5 * 0 + 6 * 0 + 7 * 1 + 8 * 1 = 5
            // 1 * 0 + 2 * 0 + 3 * 0 + 4 * 0 + 5 * 0 + 6 * 0 + 7 * 0 + 8 * 1 = 8
            var testData = new List<Tuple<int[], int, int, int>>(new Tuple<int[], int, int, int>[] {
                // Output index: 0
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 0, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 0, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 0, -1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 0, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 0, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 0, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 0, -1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 0, 0),

                // Output index: 1
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 1, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 1, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 1, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 1, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 1, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 1, -1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 1, -1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 1, 0),

                // Output index: 2
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 2, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 2, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 2, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 2, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 2, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 2, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 2, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 2, 0),

                // Output index: 3
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 3, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 3, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 3, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 3, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 3, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 3, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 3, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 3, 0),

                // Output index: 4
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 4, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 4, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 4, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 4, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 4, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 4, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 4, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 4, 1),

                // Output index: 5
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 5, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 5, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 5, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 5, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 5, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 5, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 5, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 5, 1),

                // Output index: 6
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 6, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 6, 1),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 6, 1),

                // Output index: 7
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    0, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    1, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    2, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    3, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    4, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    5, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    6, 7, 0),
                new Tuple<int[], int, int, int>(
                    new int[] { 0, 1, 0, -1 },
                    7, 7, 1)
            });

            foreach (var testExample in testData)
            {
                var result = Day16.GetPhaseEntry(
                    basePhasePattern: testExample.Item1,
                    inputIndex: testExample.Item2,
                    outputIndex: testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public static void GetFFTTest()
        {
            // Test exampels taken from here:
            // https://adventofcode.com/2019/day/16
            // Input signal: 12345678
            // After 1 phase: 48226158
            // After 2 phases: 34040438
            // After 3 phases: 03415518
            // After 4 phases: 01029498
            // Here are the first eight digits of the final output list after 100 phases for some larger inputs:
            // 80871224585914546619083218645595 becomes 24176176.
            // 19617804207202209144916044189917 becomes 73745418.
            // 69317163492948606335995924319873 becomes 52432133.
            var testData = new List<Tuple<int[], string, int, string>>(new Tuple<int[], string, int, string>[] {
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "12345678", 1, "48226158"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "12345678", 2, "34040438"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "12345678", 3, "03415518"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "12345678", 4, "01029498"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "80871224585914546619083218645595", 100, "24176176"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "19617804207202209144916044189917", 100, "73745418"),
                new Tuple<int[], string, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "69317163492948606335995924319873", 100, "52432133"),
            });

            foreach (var testExample in testData)
            {
                var result = Day16.GetFFT(
                    input: testExample.Item2,
                    numberOfPhases: testExample.Item3,
                    basePhasePattern: testExample.Item1);
                Assert.StartsWith(testExample.Item4, result);
            }
        }

        [Fact]
        public void GetDay16Part1AnswerTest()
        {
            string expected = "44098263";
            string actual = Day16.GetDay16Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
