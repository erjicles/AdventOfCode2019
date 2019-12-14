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
    }
}
