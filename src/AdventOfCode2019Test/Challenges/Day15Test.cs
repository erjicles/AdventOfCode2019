using AdventOfCode2019.Challenges.Day15;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day15Test
    {
        [Fact]
        public void GetDay15Part1AnswerTest()
        {
            int expected = 280;
            int actual = Day15.GetDay15Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay15Part2AnswerTest()
        {
            int expected = 400;
            int actual = Day15.GetDay15Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
