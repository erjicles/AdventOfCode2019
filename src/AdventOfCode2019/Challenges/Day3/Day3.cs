using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
#nullable enable

namespace AdventOfCode2019.Challenges.Day3
{
    public static class Day3
    {
        public const string FILE_NAME = "Day3Input.txt";
        public static GridPoint origin = new GridPoint();
        public static int GetDay3Part1Answer()
        {
            // What is the Manhattan distance from the central port to the closest intersection?
            // Answer: 227
            GetDay3Input(out string[] path1Instructions, out string[] path2Instructions);
            var path1 = GeneratePath(path1Instructions);
            var path2 = GeneratePath(path2Instructions);
            return GetClosestIntersectionToOriginManhattanDistance(path1, path2);
        }
        public static int GetDay3Part2Answer()
        {
            // What is the fewest combined steps the wires must take to reach an intersection?
            // Answer: 20286
            GetDay3Input(out string[] path1Instructions, out string[] path2Instructions);
            var path1 = GeneratePath(path1Instructions);
            var path2 = GeneratePath(path2Instructions);
            return GetMinimalIntersectionTotalSteps(path1, path2);
        }
        public static int GetClosestIntersectionToOriginManhattanDistance(GridPoint[] path1, GridPoint[] path2)
        {
            var intersections = GetIntersections(path1, path2);
            var shortestDistance = int.MaxValue;
            foreach (var intersection in intersections)
            {
                int distance = GridPoint.GetManhattanDistance(intersection, origin);
                if (distance < shortestDistance)
                    shortestDistance = distance;
            }
            return shortestDistance;
        }
        public static int GetMinimalIntersectionTotalSteps(GridPoint[] path1, GridPoint[] path2)
        {
            var intersections = GetIntersections(path1, path2);
            int shortestTotalSteps = int.MaxValue;
            foreach (var intersection in intersections)
            {
                var stepsPath1 = GetStepsToPoint(path1, intersection);
                var stepsPath2 = GetStepsToPoint(path2, intersection);
                var totalSteps = stepsPath1 + stepsPath2;
                if (totalSteps < shortestTotalSteps)
                    shortestTotalSteps = totalSteps;
            }
            return shortestTotalSteps;
        }
        public static int GetStepsToPoint(GridPoint[] path1, GridPoint point)
        {
            int steps = 0;
            for (steps = 0; steps < path1.Length; steps++)
            {
                if (path1[steps].Equals(point))
                    break;
            }
            return steps;
        }
        public static GridPoint[] GetIntersections(GridPoint[] path1, GridPoint[] path2)
        {
            var intersections = new List<GridPoint>();
            var shorterPath = path1.Length < path2.Length ? path1 : path2;
            var longerPath = path1.Length < path2.Length ? path2 : path1;
            var shorterPathHashSet = new HashSet<GridPoint>();
            foreach (var point1 in shorterPath)
            {
                if (!point1.Equals(origin))
                    shorterPathHashSet.Add(point1);
            }
            foreach (var point2 in longerPath)
            {
                if (shorterPathHashSet.Contains(point2))
                    intersections.Add(point2);
            }
            return intersections.ToArray();
        }
        public static GridPoint[] GeneratePath(string[] instructions)
        {
            // Initialize the path with the origin
            var currentPoint = new GridPoint();
            var path = new List<GridPoint>(new GridPoint[] { currentPoint });
            
            // Generate the path by reading each instruction and moving the
            // current point to the next point as instructed
            foreach (var rawInstruction in instructions)
            {
                // The instruction should be of the following pattern:
                // 1) L/R/U/D for the direction
                // 2) A number indicating how far to move
                var instruction = rawInstruction.Trim();
                var match = Regex.Match(instruction, "^(L|R|U|D)([0-9]+)$");
                if (!match.Success)
                {
                    throw new Exception($"Invalid instruction: {instruction}");
                }
                var direction = match.Groups[1].Value;
                var distance = int.Parse(match.Groups[2].Value);
                for (int i = 0; i < distance; i++)
                {
                    if ("L".Equals(direction))
                        currentPoint = currentPoint.MoveLeft(1);
                    else if ("R".Equals(direction))
                        currentPoint = currentPoint.MoveRight(1);
                    else if ("U".Equals(direction))
                        currentPoint = currentPoint.MoveUp(1);
                    else if ("D".Equals(direction))
                        currentPoint = currentPoint.MoveDown(1);
                    path.Add(currentPoint);
                }
            }
            return path.ToArray();
        }

        public static string[] GetPathInstructionsFromString(string line)
        {
            return line.Split(",");
        }

        public static void GetDay3Input(out string[] pathInstructions1, out string[] pathInstructions2)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Cannot locate file {filePath}");
            }
            var fileLines = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (sr.Peek() >= 0)
                {
                    string? currentLine = sr.ReadLine();
                    if (currentLine != null)
                    {
                        fileLines.Add(currentLine);
                    }
                }
            }
            if (fileLines.Count < 2)
                throw new Exception("Fewer than 2 lines read");
            pathInstructions1 = GetPathInstructionsFromString(fileLines[0]);
            pathInstructions2 = GetPathInstructionsFromString(fileLines[1]);
        }
    }
}
