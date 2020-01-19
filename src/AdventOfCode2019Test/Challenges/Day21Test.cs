using AdventOfCode2019.Challenges.Day21;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day21Test
    {
        [Fact]
        public void GetDay21Part1AnswerTest()
        {
            BigInteger expected = 19360724;
            BigInteger actual = Day21.GetDay21Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay21Part2AnswerTest()
        {
            BigInteger expected = 1140450681;
            BigInteger actual = Day21.GetDay21Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
