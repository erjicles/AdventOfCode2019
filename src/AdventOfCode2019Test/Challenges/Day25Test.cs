using AdventOfCode2019.Challenges.Day25;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day25Test
    {
        [Fact]
        public void GetDay25Part1AnswerTest()
        {
            BigInteger expected = 268468864;
            BigInteger actual = Day25.GetDay25Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
