using AdventOfCode2019.Challenges.Day23;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day23Test
    {
        [Fact]
        public void GetDay23Part1AnswerTest()
        {
            BigInteger expected = 24602;
            BigInteger actual = Day23.GetDay23Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
