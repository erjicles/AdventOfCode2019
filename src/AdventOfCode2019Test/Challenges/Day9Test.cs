using AdventOfCode2019.Challenges.Day9;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day9Test
    {
        [Fact]
        public void GetDay9Part1AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("2662308295");
            BigInteger actual = Day9.GetDay9Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
