using AdventOfCode2019.Challenges.Day11;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day11Test
    {
        [Fact]
        public void GetDay11Part1AnswerTest()
        {
            int expected = 1883;
            int actual = Day11.GetDay11Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RunDay11Part2Test()
        {
            int expected = 249;
            int actual = Day11.RunDay11Part2();
            Assert.Equal(expected, actual);
        }
    }
}
