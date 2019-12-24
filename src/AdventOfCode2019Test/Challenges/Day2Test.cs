﻿using AdventOfCode2019.Challenges.Day2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day2Test
    {
        [Fact]
        public void GetDay2Part1AnswerTest()
        {
            int expected = 11590668;
            int actual = Day2.GetDay2Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay2Part2AnswerTest()
        {
            int expected = 2254;
            int actual = Day2.GetDay2Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
