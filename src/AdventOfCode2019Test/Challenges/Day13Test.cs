using AdventOfCode2019.Challenges.Day13;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day13Test
    {
        [Fact]
        public void GetDay13Part1AnswerTest()
        {
            int expected = 265;
            int actual = Day13.GetDay13Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay13Part2AnswerTest()
        {
            long expected = 13331;
            long actual = Day13.GetDay13Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
