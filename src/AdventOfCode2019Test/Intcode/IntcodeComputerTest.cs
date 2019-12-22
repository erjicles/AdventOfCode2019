using AdventOfCode2019.Intcode;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Intcode
{
    public class IntcodeComputerTest
    {
        [Fact]
        public void ParseCommandTest()
        {
            var testData = new List<Tuple<int, int[]>>(new Tuple<int, int[]>[] {
                new Tuple<int, int[]>(1002, new int[] { 2, 0, 1 }),
                new Tuple<int, int[]>(200146, new int[] { 46, 1, 0, 0, 2 })
            });

            foreach (var testExample in testData)
            {
                var result = IntcodeComputer.ParseCommand(testExample.Item1);
                Assert.Equal(testExample.Item2, result);
            }
        }

        [Fact]
        public void GetParameterModeTest()
        {
            var testData = new List<Tuple<int, int, int>>(new Tuple<int, int, int>[] {
                new Tuple<int, int, int>(1002, 1, 0),
                new Tuple<int, int, int>(1002, 2, 1),
                new Tuple<int, int, int>(1002, 3, 0),
                new Tuple<int, int, int>(200146, 1, 1),
                new Tuple<int, int, int>(200146, 2, 0),
                new Tuple<int, int, int>(200146, 3, 0),
                new Tuple<int, int, int>(200146, 4, 2),
                new Tuple<int, int, int>(200146, 5, 0),
            });

            foreach (var testExample in testData)
            {
                var parsedCommand = IntcodeComputer.ParseCommand(testExample.Item1);
                var result = IntcodeComputer.GetParameterMode(testExample.Item2, parsedCommand);
                Assert.Equal(testExample.Item3, result);
            }
        }

        [Fact]
        public void RunProgramTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/2
            // Here are the initial and final states of a few more small programs:
            // 1,0,0,0,99 becomes 2,0,0,0,99 (1 + 1 = 2).
            // 2,3,0,3,99 becomes 2,3,0,6,99 (3 * 2 = 6).
            // 2,4,4,5,99,0 becomes 2,4,4,5,99,9801 (99 * 99 = 9801).
            // 1,1,1,4,99,5,6,0,99 becomes 30,1,1,4,2,5,6,0,99.
            var testData = new List<Tuple<int[], int[]>>(new Tuple<int[], int[]>[] {
                new Tuple<int[], int[]>(new int[] {1,0,0,0,99}, new int[] {2,0,0,0,99}),
                new Tuple<int[], int[]>(new int[] {2,3,0,3,99}, new int[] {2,3,0,6,99}),
                new Tuple<int[], int[]>(new int[] {2,4,4,5,99,0}, new int[] {2,4,4,5,99,9801}),
                new Tuple<int[], int[]>(new int[] {1,1,1,4,99,5,6,0,99}, new int[] {30,1,1,4,2,5,6,0,99}),
            });

            foreach (var testExample in testData)
            {
                var result = IntcodeComputer.RunProgram(testExample.Item1);
                Assert.Equal(testExample.Item2, result);
            }
        }
    }
}
