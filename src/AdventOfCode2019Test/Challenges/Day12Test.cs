using AdventOfCode2019.Challenges.Day12;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day12Test
    {
        [Fact]
        public void EvolveMoonsAndGetTotalEnergyOfSystemTest()
        {
            // Tests taken from here:
            // https://adventofcode.com/2019/day/12

            // <x=-1, y=0, z=2>
            // <x=2, y=-10, z=-7>
            // <x=4, y=-8, z=8>
            // <x=3, y=5, z=-1>
            // Total energy after 10 steps: 179
            // <x=-8, y=-10, z=0>
            // <x=5, y=5, z=10>
            // <x=2, y=-7, z=3>
            // <x=9, y=-8, z=-3>
            // Total energy after 100 steps:
            var testData = new List<Tuple<string[], int, int>>(new Tuple<string[], int, int>[] {
                new Tuple<string[], int, int>(new string[] 
                {
                    "<x=-1, y=0, z=2>",
                    "<x=2, y=-10, z=-7>",
                    "<x=4, y=-8, z=8>",
                    "<x=3, y=5, z=-1>"
                }, 10, 179),
                new Tuple<string[], int, int>(new string[]
                {
                    "<x=-8, y=-10, z=0>",
                    "<x=5, y=5, z=10>",
                    "<x=2, y=-7, z=3>",
                    "<x=9, y=-8, z=-3>"
                }, 100, 1940),
            });

            foreach (var testExample in testData)
            {
                var moons = Day12.ProcessMoonScanResults(testExample.Item1);
                Day12.EvolveMoons(moons, testExample.Item2);
                int totalEnergy = Day12.GetTotalEnergyOfSystem(moons);
                Assert.Equal(testExample.Item3, totalEnergy);
            }
        }

        [Fact]
        public void GetDay12Part1AnswerTest()
        {
            int expected = 5937;
            int actual = Day12.GetDay12Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
