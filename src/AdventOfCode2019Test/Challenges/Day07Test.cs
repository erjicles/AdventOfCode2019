using AdventOfCode2019.Challenges.Day07;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day07Test
    {
        [Fact]
        public void GetMaximumAmplifierOutputTest()
        {
            // Normal mode (no feedback loop)
            // Max thruster signal 43210 (from phase setting sequence 4,3,2,1,0):
            // 3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0
            // Max thruster signal 54321(from phase setting sequence 0, 1, 2, 3, 4):
            // 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0
            // Max thruster signal 65210(from phase setting sequence 1, 0, 4, 3, 2):
            // 3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0
            
            // Feedback mode (using feedback loop)
            // Max thruster signal 139629729 (from phase setting sequence 9,8,7,6,5):
            // 3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5
            // Max thruster signal 18216(from phase setting sequence 9, 7, 8, 5, 6):
            // 3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10
            
            // Test data format:
            // Item 1: initial input value
            // Item 2: Number of amplifiers
            // Item 3: Program
            // Item 4: Feedback mode
            // Item 5: Expected value
            var testData = new List<Tuple<int, int, BigInteger[], FeedbackMode, int>>(new Tuple<int, int, BigInteger[], FeedbackMode, int>[] {
                new Tuple<int, int, BigInteger[], FeedbackMode, int>(0, 5, new BigInteger[] { 3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0 }, FeedbackMode.Normal, 43210),
                new Tuple<int, int, BigInteger[], FeedbackMode, int>(0, 5, new BigInteger[] { 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0 }, FeedbackMode.Normal, 54321),
                new Tuple<int, int, BigInteger[], FeedbackMode, int>(0, 5, new BigInteger[] { 3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0 }, FeedbackMode.Normal, 65210),
                new Tuple<int, int, BigInteger[], FeedbackMode, int>(0, 5, new BigInteger[] { 3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5 }, FeedbackMode.Loop, 139629729),
                new Tuple<int, int, BigInteger[], FeedbackMode, int>(0, 5, new BigInteger[] { 3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10 }, FeedbackMode.Loop, 18216),
            });

            foreach (var testExample in testData)
            {
                var result = Day07.GetMaximumAmplifierOutput(
                    initialInput:testExample.Item1, 
                    numberOfAmplifiers:testExample.Item2, 
                    program:testExample.Item3, 
                    feedbackMode:testExample.Item4);
                Assert.Equal(testExample.Item5, result);
            }
        }

        [Fact]
        public void GetDay7Part1AnswerTest()
        {
            int expected = 255840;
            int actual = Day07.GetDay7Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay7Part2AnswerTest()
        {
            int expected = 84088865;
            int actual = Day07.GetDay7Part2Answer();
            Assert.Equal(expected, actual);
        }

    }
}
