using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day24
{
    /// <summary>
    /// Solution to the Day 24 challenge:
    /// https://adventofcode.com/2019/day/24
    /// </summary>
    public class Day24
    {
        public const string FILE_NAME = "Day24Input.txt";
        public static BigInteger GetDay24Part1Answer()
        {
            // What is the biodiversity rating for the first layout that 
            // appears twice?
            // Answer: 10282017
            var mapDefinition = GetDay24Input();
            var currentState = ErisMapState.CreateMap(mapDefinition);
            currentState.DrawMapState();
            var statesVisited = new HashSet<ErisMapState>() { currentState };
            while (true)
            {
                currentState = currentState.Evolve();
                if (statesVisited.Contains(currentState))
                    break;
                statesVisited.Add(currentState);
            }
            currentState.DrawMapState();
            var result = currentState.GetBiodiversityRating();
            return result;
        }

        public static IList<string> GetDay24Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
