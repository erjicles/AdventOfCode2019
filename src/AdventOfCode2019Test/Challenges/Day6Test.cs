using AdventOfCode2019.Challenges.Day6;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day6Test
    {
        [Fact]
        public void ParseMapEntryTest()
        {
            var testData = new List<Tuple<string, Tuple<string, string>>>(new Tuple<string, Tuple<string, string>>[] {
                new Tuple<string, Tuple<string, string>>("AAA)BBB", new Tuple<string, string>("AAA", "BBB")),
                new Tuple<string, Tuple<string, string>>("COM)B", new Tuple<string, string>("COM", "B")),
                new Tuple<string, Tuple<string, string>>("D)EEE", new Tuple<string, string>("D", "EEE"))
            });

            foreach (var testExample in testData)
            {
                var parsedMapEntry = Day6.ParseMapEntry(testExample.Item1);
                Assert.Equal(testExample.Item2, parsedMapEntry);
            }

            Assert.ThrowsAny<Exception>(() => Day6.ParseMapEntry("ABC"));
            Assert.ThrowsAny<Exception>(() => Day6.ParseMapEntry(""));
            Assert.ThrowsAny<Exception>(() => Day6.ParseMapEntry("A)"));
            Assert.ThrowsAny<Exception>(() => Day6.ParseMapEntry(")B"));
            Assert.ThrowsAny<Exception>(() => Day6.ParseMapEntry("AAA(BBB"));
        }

        [Fact]
        public void GetTotalNumberOfOrbitsTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/6
            // COM)B
            // B)C
            // C)D
            // D)E
            // E)F
            // B)G
            // G)H
            // D)I
            // E)J
            // J)K
            // K)L
            // The total number of direct and indirect orbits in this example is 42.
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                new Tuple<string[], int>(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" }, 42)
            });

            foreach (var testExample in testData)
            {
                var orbitalMap = Day6.ConstructOrbitMap(testExample.Item1);
                var numberOfOrbits = Day6.GetTotalNumberOfOrbits(orbitalMap);
                Assert.Equal(testExample.Item2, numberOfOrbits);
            }
        }

        [Fact]
        public void GetNumberOfOrbitalTransfersTest()
        {
            // Test examples found here:
            // https://adventofcode.com/2019/day/6#part2
            // COM)B
            // B)C
            // C)D
            // D)E
            // E)F
            // B)G
            // G)H
            // D)I
            // E)J
            // J)K
            // K)L
            // K)YOU
            // I)SAN
            // In this example, YOU are in orbit around K, and SAN is in orbit 
            // around I. To move from K to I, a minimum of 4 orbital transfers 
            // are required:
            // K to J
            // J to E
            //E to D
            //D to I
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                new Tuple<string[], int>(new string[] { 
                    "COM)B", 
                    "B)C", 
                    "C)D", 
                    "D)E", 
                    "E)F", 
                    "B)G", 
                    "G)H", 
                    "D)I", 
                    "E)J", 
                    "J)K", 
                    "K)L",
                    "K)YOU",
                    "I)SAN" }, 4)
            });

            foreach (var testExample in testData)
            {
                var orbitalMap = Day6.ConstructOrbitMap(testExample.Item1);
                var numberOfOrbits = Day6.GetNumberOfOrbitalTransfers("YOU", "SAN", orbitalMap);
                Assert.Equal(testExample.Item2, numberOfOrbits);
            }
        }
    }
}
