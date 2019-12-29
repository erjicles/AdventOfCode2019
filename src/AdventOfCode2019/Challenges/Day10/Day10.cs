using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day10
{
    /// <summary>
    /// Solution to the Day 10 challenge:
    /// https://adventofcode.com/2019/day/10
    /// </summary>
    public class Day10
    {
        public const string FILE_NAME = "Day10Input.txt";

        public static int GetDay10Part1Answer()
        {
            // Answer: 282
            var input = GetDay10Input();
            var map = new SolarSystemMap(input);
            var result = map.GetObjectThatSeesMostOtherObjects();
            return result.Item2;
        }

        public static int GetDay10Part2Answer()
        {
            // The Elves are placing bets on which will be the 200th asteroid 
            // to be vaporized. Win the bet by determining which asteroid that 
            // will be; what do you get if you multiply its X coordinate by 
            // 100 and then add its Y coordinate? (For example, 8,2 becomes 
            // 802.)
            // Answer: 1008
            var input = GetDay10Input();
            var map = new SolarSystemMap(input);
            var stationLocation = map.GetObjectThatSeesMostOtherObjects();
            var nthAsteroid = map.GetNthAsteroidVaporized(stationLocation.Item1.GridPoint, 200);
            var result = (100 * nthAsteroid.GridPoint.X) + nthAsteroid.GridPoint.Y;
            return result;
        }

        public static string[] GetDay10Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME).ToArray();
        }
    }
}
