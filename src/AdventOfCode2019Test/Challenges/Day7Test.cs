using AdventOfCode2019.Challenges.Day7;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day7Test
    {
        [Fact]
        public void GetMaximumAmplifierOutputTest()
        {
            // Max thruster signal 43210 (from phase setting sequence 4,3,2,1,0):
            // 3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0
            // Max thruster signal 54321(from phase setting sequence 0, 1, 2, 3, 4):
            // 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0
            // Max thruster signal 65210(from phase setting sequence 1, 0, 4, 3, 2):
            // 3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0
            var testData = new List<Tuple<int, int, int[], int>>(new Tuple<int, int, int[], int>[] {
                new Tuple<int, int, int[], int>(0, 5, new int[] { 3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0 }, 43210),
                new Tuple<int, int, int[], int>(0, 5, new int[] { 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0 }, 54321),
                new Tuple<int, int, int[], int>(0, 5, new int[] { 3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0 }, 65210),
            });

            foreach (var testExample in testData)
            {
                var result = Day7.GetMaximumAmplifierOutput(testExample.Item1, testExample.Item2, testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }
    }
}
