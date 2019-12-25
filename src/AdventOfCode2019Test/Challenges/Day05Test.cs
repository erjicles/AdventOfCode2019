using AdventOfCode2019.Challenges.Day05;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day05Test
    {
        [Fact]
        public void GetDay5Part1AnswerTest()
        {
            int expected = 12896948;
            var outputListener = Day05.RunDay5Part1();
            Assert.True(outputListener.OutputValues.Count > 0);
            var actual = outputListener.OutputValues.Last();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay5Part2AnswerTest()
        {
            int expected = 7704130;
            var outputListener = Day05.RunDay5Part2();
            Assert.True(outputListener.OutputValues.Count > 0);
            var actual = outputListener.OutputValues.Last();
            Assert.Equal(expected, actual);
        }
    }
}
