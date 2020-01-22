using AdventOfCode2019.Challenges.Day24;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day24Test
    {
        [Fact]
        public void GetTotalBugCountTest()
        {
            // Test example taken here:
            // https://adventofcode.com/2019/day/24
            // ....#
            // #..#.
            // #.?##
            // ..#..
            // #....
            // In this example, after 10 minutes, a total of 99 bugs are present.
            var testData = new List<Tuple<string[], bool, int, int>>()
            {
                Tuple.Create(new string[]
                {
                    "....#",
                    "#..#.",
                    "#.?##",
                    "..#..",
                    "#....",
                },
                true, 10, 99),
            };
            foreach (var testExample in testData)
            {
                var map = ErisMapState.CreateMap(testExample.Item1, testExample.Item2);
                for (int i = 0; i < testExample.Item3; i++)
                {
                    map = map.Evolve();
                }
                var result = map.GetTotalNumberOfBugs();
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void GetDay24Part1AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("10282017");
            BigInteger actual = Day24.GetDay24Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay24Part2AnswerTestTest()
        {
            BigInteger expected = 99;
            BigInteger actual = Day24.GetDay24Part2AnswerTest();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay24Part2AnswerTest()
        {
            BigInteger expected = 2065;
            BigInteger actual = Day24.GetDay24Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
