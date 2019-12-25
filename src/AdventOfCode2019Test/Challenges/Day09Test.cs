using AdventOfCode2019.Challenges.Day09;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day09Test
    {
        [Fact]
        public void GetDay9Part1AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("2662308295");
            BigInteger actual = Day09.GetDay9Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay9Part2AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("63441");
            BigInteger actual = Day09.GetDay9Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
