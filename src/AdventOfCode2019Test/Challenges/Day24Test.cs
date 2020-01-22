using AdventOfCode2019.Challenges.Day24;
using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day24Test
    {
        [Fact]
        public void GetAdjacentBugCountsTest()
        {
            var testData = new List<Tuple<string[], bool, Dictionary<GridPoint3D, int>>>()
            {
                // ....#
                // #..#.
                // #.?##
                // ..#..
                // #....
                // Bug counts for this state (non-recursive):
                // (0,0,0), 1
                // (3,0,0), 2
                // (0,1,0), 1
                // (1,1,0), 1
                // (2,1,0), 1
                // (3,1,0), 1
                // (4,1,0), 3
                // (0,2,0), 1
                // (1,2,0), 1
                // (2,2,0), 2
                // (3,2,0), 2
                // (4,2,0), 1
                // (0,3,0), 2
                // (1,3,0), 1
                // (3,3,0), 2
                // (4,3,0), 1
                // (1,4,0), 1
                // (2,4,0), 1
                Tuple.Create(new string[]
                {
                    "....#",
                    "#..#.",
                    "#.?##",
                    "..#..",
                    "#....",
                },
                false, new Dictionary<GridPoint3D, int>()
                {
                    { new GridPoint3D(0, 0, 0), 1 },
                    { new GridPoint3D(3, 0, 0), 2 },
                    { new GridPoint3D(0, 1, 0), 1 },
                    { new GridPoint3D(1, 1, 0), 1 },
                    { new GridPoint3D(2, 1, 0), 1 },
                    { new GridPoint3D(3, 1, 0), 1 },
                    { new GridPoint3D(4, 1, 0), 3 },
                    { new GridPoint3D(0, 2, 0), 1 },
                    { new GridPoint3D(1, 2, 0), 1 },
                    { new GridPoint3D(2, 2, 0), 2 },
                    { new GridPoint3D(3, 2, 0), 2 },
                    { new GridPoint3D(4, 2, 0), 1 },
                    { new GridPoint3D(0, 3, 0), 2 },
                    { new GridPoint3D(1, 3, 0), 1 },
                    { new GridPoint3D(3, 3, 0), 2 },
                    { new GridPoint3D(4, 3, 0), 1 },
                    { new GridPoint3D(1, 4, 0), 1 },
                    { new GridPoint3D(2, 4, 0), 1 },
                }),

                // ....#
                // #..#.
                // #.?##
                // ..#..
                // #....
                // Bug counts for this state (recursive):
                // (1,2,-1), 3
                // (2,1,-1), 1
                // (3,2,-1), 2
                // (2,3,-1), 1
                // (0,0,0), 1
                // (3,0,0), 2
                // (0,1,0), 1
                // (1,1,0), 1
                // (2,1,0), 1
                // (3,1,0), 1
                // (4,1,0), 3
                // (0,2,0), 1
                // (1,2,0), 1
                // (3,2,0), 2
                // (4,2,0), 1
                // (0,3,0), 2
                // (1,3,0), 1
                // (3,3,0), 2
                // (4,3,0), 1
                // (1,4,0), 1
                // (2,4,0), 1
                // (4,0,1), 1
                // (4,1,1), 1
                // (4,2,1), 1
                // (4,3,1), 1
                // (4,4,1), 2
                // (0,4,1), 1
                // (1,4,1), 1
                // (2,4,1), 1
                // (3,4,1), 1
                Tuple.Create(new string[]
                {
                    "....#",
                    "#..#.",
                    "#.?##",
                    "..#..",
                    "#....",
                },
                true, new Dictionary<GridPoint3D, int>()
                {
                    { new GridPoint3D(1,2,-1), 3 },
                    { new GridPoint3D(2,1,-1), 1 },
                    { new GridPoint3D(3,2,-1), 2 },
                    { new GridPoint3D(2,3,-1), 1 },
                    { new GridPoint3D(0,0,0), 1 },
                    { new GridPoint3D(3,0,0), 2 },
                    { new GridPoint3D(0,1,0), 1 },
                    { new GridPoint3D(1,1,0), 1 },
                    { new GridPoint3D(2,1,0), 1 },
                    { new GridPoint3D(3,1,0), 1 },
                    { new GridPoint3D(4,1,0), 3 },
                    { new GridPoint3D(0,2,0), 1 },
                    { new GridPoint3D(1,2,0), 1 },
                    { new GridPoint3D(3,2,0), 2 },
                    { new GridPoint3D(4,2,0), 1 },
                    { new GridPoint3D(0,3,0), 2 },
                    { new GridPoint3D(1,3,0), 1 },
                    { new GridPoint3D(3,3,0), 2 },
                    { new GridPoint3D(4,3,0), 1 },
                    { new GridPoint3D(1,4,0), 1 },
                    { new GridPoint3D(2,4,0), 1 },
                    { new GridPoint3D(4,0,1), 1 },
                    { new GridPoint3D(4,1,1), 1 },
                    { new GridPoint3D(4,2,1), 1 },
                    { new GridPoint3D(4,3,1), 1 },
                    { new GridPoint3D(4,4,1), 2 },
                    { new GridPoint3D(0,4,1), 1 },
                    { new GridPoint3D(1,4,1), 1 },
                    { new GridPoint3D(2,4,1), 1 },
                    { new GridPoint3D(3,4,1), 1 },
                }),
            };
            foreach (var testExample in testData)
            {
                var map = ErisMapState.CreateMap(testExample.Item1, testExample.Item2);
                var actual = map.GetAdjacentBugCounts(out _);
                var expected = testExample.Item3;
                Assert.Equal(expected.Count, actual.Count);
                foreach (var kvp in actual)
                {
                    Assert.True(expected.ContainsKey(kvp.Key));
                    var expectedValue = expected[kvp.Key];
                    Assert.Equal(expectedValue, kvp.Value);
                }
                var areEqual = (expected.Count == actual.Count 
                    && !expected.Except(actual).Any());
                Assert.True(areEqual);
            }
        }

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
                true, 1, 27),
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
                var result = map.BugCells.Count;
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
