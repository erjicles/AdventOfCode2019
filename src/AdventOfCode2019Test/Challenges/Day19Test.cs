using AdventOfCode2019.Challenges.Day19;
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
    }
}
