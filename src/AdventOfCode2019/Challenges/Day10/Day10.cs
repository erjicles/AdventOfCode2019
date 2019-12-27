using System;
using System.Collections.Generic;
using System.IO;
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

        public static string[] GetDay10Input()
        {
            var result = new List<string>();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Cannot locate file {filePath}");
            }
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (sr.Peek() >= 0)
                {
                    string? currentLine = sr.ReadLine();
                    if (currentLine != null)
                    {
                        result.Add(currentLine);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
