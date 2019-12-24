using AdventOfCode2019.Challenges;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day1Test
    {
        [Fact]
        public void CalculateRequiredFuelTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/1
            // For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
            // For a mass of 14, dividing by 3 and rounding down still yields 4, so the fuel required is also 2.
            // For a mass of 1969, the fuel required is 654.
            // For a mass of 100756, the fuel required is 33583.
            var testData = new List<KeyValuePair<int, int>>(new KeyValuePair<int, int>[] {
                new KeyValuePair<int, int>(12, 2),
                new KeyValuePair<int, int>(14, 2),
                new KeyValuePair<int, int>(1969, 654),
                new KeyValuePair<int, int>(100756, 33583)
            });
            
            foreach (var testExample in testData)
            {
                var calculatedValue = Day1.CalculateRequiredFuel(testExample.Key);
                Assert.Equal(testExample.Value, calculatedValue);
            }
        }

        [Fact]
        public void CalculateModuleRequiredFuelTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/1#part2
            // A module of mass 14 requires 2 fuel. This fuel requires no further fuel (2 divided by 3 and rounded down is 0, which would call for a negative fuel), so the total fuel required is still just 2.
            // At first, a module of mass 1969 requires 654 fuel.Then, this fuel requires 216 more fuel(654 / 3 - 2). 216 then requires 70 more fuel, which requires 21 fuel, which requires 5 fuel, which requires no further fuel.So, the total fuel required for a module of mass 1969 is 654 + 216 + 70 + 21 + 5 = 966.
            // The fuel required by a module of mass 100756 and its fuel is: 33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2 = 50346.
            var testData = new List<KeyValuePair<int, int>>(new KeyValuePair<int, int>[] {
                new KeyValuePair<int, int>(12, 2),
                new KeyValuePair<int, int>(14, 2),
                new KeyValuePair<int, int>(1969, 966),
                new KeyValuePair<int, int>(100756, 50346)
            });

            foreach (var testExample in testData)
            {
                var calculatedValue = Day1.CalculateModuleRequiredFuel(testExample.Key);
                Assert.Equal(testExample.Value, calculatedValue);
            }
        }

        [Fact]
        public void GetDay1Part1AnswerTest()
        {
            int expected = 3453056;
            int actual = Day1.GetDay1Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay1Part2AnswerTest()
        {
            int expected = 5176705;
            int actual = Day1.GetDay1Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
