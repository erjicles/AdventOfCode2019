using AdventOfCode2019.Challenges.Day19;
using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day19Test
    {
        [Fact]
        public void GetDay19Part1AnswerTest()
        {
            int expected = 199;
            int actual = Day19.GetDay19Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetClosestBoxThatFitsInsideBeamTest()
        {
            var testData = new List<Tuple<int, int, GridPoint>>(new Tuple<int, int, GridPoint>[] {
                new Tuple<int, int, GridPoint>(1, 1, new GridPoint(0, 0)),
                new Tuple<int, int, GridPoint>(2, 2, new GridPoint(14, 10)),
                new Tuple<int, int, GridPoint>(3, 3, new GridPoint(21, 15)),
                new Tuple<int, int, GridPoint>(4, 4, new GridPoint(35, 25)),
                new Tuple<int, int, GridPoint>(5, 5, new GridPoint(42, 30))
            });
            foreach (var testExample in testData)
            {
                var program = Day19.GetDay19Input();
                var actual = Day19.GetClosestBoxThatFitsInsideBeam(
                    program: program,
                    boxWidth: testExample.Item1,
                    boxHeight: testExample.Item2);
                Assert.Equal(testExample.Item3, actual);
            }
        }

        [Fact]
        public void GetDay19Part2AnswerTest()
        {
            int expected = 10180726;
            int actual = Day19.GetDay19Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
