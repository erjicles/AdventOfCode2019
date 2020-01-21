using AdventOfCode2019.Challenges.Day24;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day24Test
    {
        [Fact]
        public void GetDay24Part1AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("10282017");
            BigInteger actual = Day24.GetDay24Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
