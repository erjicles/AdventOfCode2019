using AdventOfCode2019.Challenges.Day8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day8Test
    {
        [Fact]
        public void GetDay8Part1AnswerTest()
        {
            int expected = 2375;
            int actual = Day8.GetDay8Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RunDay8Part2Test()
        {
            string expected = "111001001010010111001000110010101001001010010100011001011000111101001001010111001010010010111000010010100101001001010100001001001010010100101001000100";
            string actual = String.Join(String.Empty, Day8.RunDay8Part2());
            Assert.Equal(expected, actual);
        }
    }
}
