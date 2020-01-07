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
        public void GetOnesDigitTest()
        {
            var testData = new List<Tuple<int, int>>(new Tuple<int, int>[] {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(4, 4),
                new Tuple<int, int>(7, 7),
                new Tuple<int, int>(10, 0),
                new Tuple<int, int>(13, 3),
                new Tuple<int, int>(17, 7),
                new Tuple<int, int>(65, 5),
                new Tuple<int, int>(82, 2),
                new Tuple<int, int>(882, 2),
                new Tuple<int, int>(8886, 6),
                new Tuple<int, int>(-1, 1),
                new Tuple<int, int>(-4, 4),
                new Tuple<int, int>(-7, 7),
                new Tuple<int, int>(-10, 0),
                new Tuple<int, int>(-13, 3),
                new Tuple<int, int>(-17, 7),
                new Tuple<int, int>(-65, 5),
                new Tuple<int, int>(-82, 2),
                new Tuple<int, int>(-882, 2),
                new Tuple<int, int>(-8886, 6)
            });
            foreach (var testExample in testData)
            {
                var result = Day16.GetOnesDigit(testExample.Item1);
                Assert.Equal(testExample.Item2, result);
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
                    basePhasePattern: testExample.Item1,
                    inputRepeatCount: 1,
                    skip: 0);
                Assert.StartsWith(testExample.Item4, result);
            }
        }

        [Fact]
        public void GetFastFFTTest()
        {
            // Test example format:
            // 1) Base phase pattern
            // 2) Input signal
            // 3) Number of phases
            // 4) Number of times to repeat the input
            // 5) Number to skip
            // 03036732577212944063491565474664 becomes 84462026.
            // 02935109699940807407585447034323 becomes 78725270.
            // 03081770884921959731165446850517 becomes 53553731.
            var testData = new List<Tuple<int[], string, int, int, int, string>>(new Tuple<int[], string, int, int, int, string>[] {
                new Tuple<int[], string, int, int, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "000123", 1, 1, 3, "653"),
                new Tuple<int[], string, int, int, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "00000000000000006789678967896789", 1, 1, 16, "0479047904790479"),
                new Tuple<int[], string, int, int, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "03036732577212944063491565474664", 100, 10000, 0303673, "84462026"),
                new Tuple<int[], string, int, int, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "02935109699940807407585447034323", 100, 10000, 0293510, "78725270"),
                new Tuple<int[], string, int, int, int, string>(
                    new int[] {0, 1, 0, -1 },
                    "03081770884921959731165446850517", 100, 10000, 0308177, "53553731"),
            });

            foreach (var testExample in testData)
            {
                var result = Day16.GetFFT(
                    input: testExample.Item2,
                    numberOfPhases: testExample.Item3,
                    basePhasePattern: testExample.Item1,
                    inputRepeatCount: testExample.Item4,
                    skip: testExample.Item5);
                Assert.StartsWith(testExample.Item6, result);
            }
        }

        [Fact]
        public void GetDay16Part1AnswerTest()
        {
            string expected = "44098263";
            string actual = Day16.GetDay16Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay16Part2AnswerTest()
        {
            string expected = "12482168";
            string actual = Day16.GetDay16Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
