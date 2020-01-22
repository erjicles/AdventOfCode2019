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
            var currentState = ErisMapState.CreateMap(mapDefinition, false);
            currentState.DrawMapState();
            var statesVisited = new HashSet<ErisMapState>() { currentState };
            int loopCount = 0;
            while (true)
            {
                loopCount++;
                currentState = currentState.Evolve();
                if (statesVisited.Contains(currentState))
                    break;
                statesVisited.Add(currentState);
            }
            Console.WriteLine($"...after {loopCount} minute(s):");
            currentState.DrawMapState();
            var result = currentState.GetBiodiversityRating(0);
            return result;
        }

        public static BigInteger GetDay24Part2Answer()
        {
            // Starting with your scan, how many bugs are present after 200 
            // minutes?
            // Answer: 2065
            var mapDefinition = GetDay24Input();
            var currentState = ErisMapState.CreateMap(mapDefinition, true);
            currentState.DrawMapState();
            var statesVisited = new HashSet<ErisMapState>() { currentState };
            Console.WriteLine("Start");
            for (int loopCount = 0; loopCount < 200; loopCount++)
            {
                currentState = currentState.Evolve();
                if (statesVisited.Contains(currentState))
                    break;
                statesVisited.Add(currentState);
                Console.Write($" ---> [Minutes: {loopCount + 1}, MinZ: {currentState.MinZ}, MaxZ: {currentState.MaxZ}]");
            }
            Console.WriteLine($"...after 200 minute(s):");
            currentState.DrawMapState();
            var result = currentState.GetTotalNumberOfBugs();
            return result;
        }

        public static BigInteger GetDay24Part2AnswerTest()
        {
            var mapDefinition = new string[]
                { "....#",
                    "#..#.",
                    "#.?##",
                    "..#..",
                    "#....",
                };
            var map = ErisMapState.CreateMap(mapDefinition, true);
            map.DrawMapState();
            Console.WriteLine("Start");
            for (int i = 0; i < 10; i++)
            {
                map = map.Evolve();
                Console.Write($" ---> [Minutes: {i + 1}, MinZ: {map.MinZ}, MaxZ: {map.MaxZ}]");
            }
            Console.WriteLine($"...after 10 minute(s):");
            map.DrawMapState();
            var result = map.GetTotalNumberOfBugs();
            return result;
        }

        public static IList<string> GetDay24Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
