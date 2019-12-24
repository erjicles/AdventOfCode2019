using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
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
                var computer = new IntcodeComputer();
                computer.LoadProgram(testExample.Item1);
                var programStatus = computer.RunProgram();
                var result = computer.GetProgramCopy();
                Assert.Equal(IntcodeProgramStatus.Completed, programStatus);
                Assert.Equal(testExample.Item2, result);
            }
        }

        [Fact]
        public void RunProgramComparisonTests()
        {
            // https://adventofcode.com/2019/day/5
            // 3,9,8,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
            // 3,9,7,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is less than 8; output 1(if it is) or 0(if it is not).
            // 3,3,1108,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is equal to 8; output 1(if it is) or 0(if it is not).
            // 3,3,1107,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is less than 8; output 1(if it is) or 0(if it is not).
            var testData = new List<Tuple<int[], int, int>>(new Tuple<int[], int, int>[] {
                new Tuple<int[], int, int>(new int[] { 3,9,8,9,10,9,4,9,99,-1,8 }, 7, 0),
                new Tuple<int[], int, int>(new int[] { 3,9,8,9,10,9,4,9,99,-1,8 }, 8, 1),
                new Tuple<int[], int, int>(new int[] { 3,9,8,9,10,9,4,9,99,-1,8 }, 9, 0),
                new Tuple<int[], int, int>(new int[] { 3,9,7,9,10,9,4,9,99,-1,8 }, 7, 1),
                new Tuple<int[], int, int>(new int[] { 3,9,7,9,10,9,4,9,99,-1,8 }, 8, 0),
                new Tuple<int[], int, int>(new int[] { 3,9,7,9,10,9,4,9,99,-1,8 }, 9, 0),
                new Tuple<int[], int, int>(new int[] { 3,3,1108,-1,8,3,4,3,99 }, 7, 0),
                new Tuple<int[], int, int>(new int[] { 3,3,1108,-1,8,3,4,3,99 }, 8, 1),
                new Tuple<int[], int, int>(new int[] { 3,3,1108,-1,8,3,4,3,99 }, 9, 0),
                new Tuple<int[], int, int>(new int[] { 3,3,1107,-1,8,3,4,3,99 }, 7, 1),
                new Tuple<int[], int, int>(new int[] { 3,3,1107,-1,8,3,4,3,99 }, 8, 0),
                new Tuple<int[], int, int>(new int[] { 3,3,1107,-1,8,3,4,3,99 }, 9, 0),
            });

            foreach (var testExample in testData)
            {
                var inputProvider = new StaticValueInputProvider(testExample.Item2);
                var outputListener = new ListOutputListener();
                var computer = new IntcodeComputer(inputProvider, outputListener);
                computer.LoadProgram(testExample.Item1);
                var status = computer.RunProgram();
                Assert.Equal(IntcodeProgramStatus.Completed, status);
                Assert.True(outputListener.Values.Count > 0);
                Assert.Equal(testExample.Item3, outputListener.Values[outputListener.Values.Count - 1]);
            }
        }

        [Fact]
        public void RunProgramJumpTests()
        {
            // https://adventofcode.com/2019/day/5
            // Here are some jump tests that take an input, then output 0 if the input was zero or 1 if the input was non-zero:
            // 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9(using position mode)
            // 3,3,1105,-1,9,1101,0,0,12,4,12,99,1(using immediate mode)
            // The following example program uses an input instruction to ask 
            // for a single number. The program will then output 999 if the 
            // input value is below 8, output 1000 if the input value is equal 
            // to 8, or output 1001 if the input value is greater than 8.
            // 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99
            var testData = new List<Tuple<int[], int, int>>(new Tuple<int[], int, int>[] {
                new Tuple<int[], int, int>(new int[] { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, -1, 1),
                new Tuple<int[], int, int>(new int[] { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 0, 0),
                new Tuple<int[], int, int>(new int[] { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 1, 1),
                new Tuple<int[], int, int>(new int[] { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 }, -1, 1),
                new Tuple<int[], int, int>(new int[] { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 }, 0, 0),
                new Tuple<int[], int, int>(new int[] { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 }, 1, 1),
                new Tuple<int[], int, int>(new int[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 7, 999),
                new Tuple<int[], int, int>(new int[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 8, 1000),
                new Tuple<int[], int, int>(new int[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 9, 1001)
            });

            foreach (var testExample in testData)
            {
                var inputProvider = new StaticValueInputProvider(testExample.Item2);
                var outputListener = new ListOutputListener();
                var computer = new IntcodeComputer(inputProvider, outputListener);
                computer.LoadProgram(testExample.Item1);
                var status = computer.RunProgram();
                Assert.Equal(IntcodeProgramStatus.Completed, status);
                Assert.True(outputListener.Values.Count > 0);
                Assert.Equal(testExample.Item3, outputListener.Values[outputListener.Values.Count - 1]);
            }
        }
    }
}
