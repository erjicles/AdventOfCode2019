using AdventOfCode2019.Challenges.Day02;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day02Test
    {
        [Fact]
        public void GetDay2Part1AnswerTest()
        {
            BigInteger expected = 11590668;
            BigInteger actual = Day02.GetDay2Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay2Part2AnswerTest()
        {
            int expected = 2254;
            int actual = Day02.GetDay2Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
