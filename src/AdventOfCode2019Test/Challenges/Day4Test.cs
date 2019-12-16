using AdventOfCode2019.Challenges.Day4;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day4Test
    {
        

        [Fact]
        public void GetIsValidPart1Test()
        {
            // Test examples taken from https://adventofcode.com/2019/day/4
            // 111111 meets these criteria (double 11, never decreases).
            // 223450 does not meet these criteria(decreasing pair of digits 50).
            // 123789 does not meet these criteria(no double).
            var testData = new List<Tuple<int, bool>>(new Tuple<int, bool>[] {
                new Tuple<int, bool>(111111, true),
                new Tuple<int, bool>(223450, false),
                new Tuple<int, bool>(123789, false)
            });

            foreach (var testExample in testData)
            {
                bool isValid = Day4.GetIsValidPart1(testExample.Item1);
                Assert.Equal(testExample.Item2, isValid);
            }
        }

        [Fact]
        public void GetIsValidPart2Test()
        {
            // Test examples taken from https://adventofcode.com/2019/day/4
            // 112233 meets these criteria because the digits never decrease and all repeated digits are exactly two digits long.
            // 123444 no longer meets the criteria(the repeated 44 is part of a larger group of 444).
            // 111122 meets the criteria(even though 1 is repeated more than twice, it still contains a double 22).
            var testData = new List<Tuple<int, bool>>(new Tuple<int, bool>[] {
                new Tuple<int, bool>(112233, true),
                new Tuple<int, bool>(123444, false),
                new Tuple<int, bool>(111122, true)
            });

            foreach (var testExample in testData)
            {
                bool isValid = Day4.GetIsValidPart2(testExample.Item1);
                Assert.Equal(testExample.Item2, isValid);
            }
        }
    }
}
