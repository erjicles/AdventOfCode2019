﻿using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace AdventOfCode2019.Challenges.Day15
{
    /// <summary>
    /// Solution to the Day 15 challenge:
    /// https://adventofcode.com/2019/day/15
    /// </summary>
    public class Day15
    {
        public const string FILE_NAME = "Day15Input.txt";

        public static int GetDay15Part1Answer()
        {
            // What is the fewest number of movement commands required to move 
            // the repair droid from its starting position to the location of 
            // the oxygen system?
            // Answer: 280
            BigInteger[] program = GetDay15Input();
            var hullMap = MapHull(program, out GridPoint robotPosition);
            DrawMap(robotPosition, hullMap);
            var movementCommands = GetMovementCommandsToMoveRobotToOxygenSystem(
                hullMap);
            return movementCommands.Count;
        }

        public static Queue<int> GetMovementCommandsToMoveRobotToOxygenSystem(
            Dictionary<GridPoint, HullCellType> hullMap)
        {
            var robotStartingPosition = new GridPoint(0, 0);
            var oxygenSystemPosition = hullMap
                .Where(kvp => HullCellType.OxygenSystem.Equals(kvp.Value))
                .Select(kvp => kvp.Key)
                .FirstOrDefault();
            var path = GetPathToPoint(robotStartingPosition, oxygenSystemPosition, hullMap);
            var pathCommands = GetMovementCommandsForPath(robotStartingPosition, path);
            return pathCommands;
        }

        public static void DrawMap(
            GridPoint robotPosition,
            Dictionary<GridPoint, HullCellType> hullMap)
        {
            int minX = hullMap.Min(kvp => kvp.Key.X);
            int maxX = hullMap.Max(kvp => kvp.Key.X);
            int minY = hullMap.Min(kvp => kvp.Key.Y);
            int maxY = hullMap.Max(kvp => kvp.Key.Y);
            Console.WriteLine();
            for (int y = minY; y <= maxY; y++)
            {
                Console.Write("     ");
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new GridPoint(x, y);
                    if (point.Equals(robotPosition))
                    {
                        Console.Write("R");
                        continue;
                    }
                    if (hullMap.ContainsKey(point))
                    {
                        var type = hullMap[point];
                        if (HullCellType.Wall.Equals(type))
                            Console.Write("#");
                        else if (HullCellType.Empty.Equals(type))
                            Console.Write(".");
                        else if (HullCellType.OxygenSystem.Equals(type))
                            Console.Write("X");
                        else
                            throw new Exception($"Invalid type {type}");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static Dictionary<GridPoint, HullCellType> MapHull(
            BigInteger[] program,
            out GridPoint robotPosition)
        {
            // Initialize the hull map
            robotPosition = new GridPoint(0, 0);
            var exploredPoints = new Dictionary<GridPoint, HullCellType>()
            {
                { robotPosition, HullCellType.Empty }
            };
            var unexploredPoints = new Tree<GridPoint>();
            var newUnexploredPoints = GetNewUnexploredPoints(
                robotPosition,
                exploredPoints,
                unexploredPoints);
            unexploredPoints.Add(newUnexploredPoints, null);

            // Setup the computer
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            var programStatus = IntcodeProgramStatus.Running;

            // Initialize the target
            SetNextTargetUnexploredPoint(
                exploredPoints,
                unexploredPoints,
                null,
                out GridPoint targetUnexploredPoint);

            var pathToTargetMovementCommands = GetMovementCommandsForPath(
                robotPosition,
                GetPathToPoint(
                    robotPosition,
                    targetUnexploredPoint,
                    exploredPoints));
            Console.WriteLine();
            int loopCount = 0;
            while (!IntcodeProgramStatus.Completed.Equals(programStatus)
                && targetUnexploredPoint != null)
            {
                loopCount++;
                // Take the next step in the path
                int movementCommand = pathToTargetMovementCommands.Dequeue();
                inputProvider.AddInputValue(movementCommand);

                programStatus = computer.RunProgram();

                // Process output:
                // 1) Set the type of the next cell based on the status code
                // 2a) Remove the next cell from the unexplored cells
                // 2b) Add the next cell to the map
                // 3) Move the robot
                // 4) Add new unexplored cells
                var nextPoint = GetNextPoint(robotPosition, movementCommand);
                var statusCode = outputListener.Values.Last();
                var nextPointType = ParseHullCellType((int)statusCode);
                if (!exploredPoints.ContainsKey(nextPoint))
                    exploredPoints.Add(nextPoint, nextPointType);
                if (!HullCellType.Wall.Equals(nextPointType))
                    robotPosition = nextPoint;
                newUnexploredPoints = GetNewUnexploredPoints(
                    robotPosition,
                    exploredPoints,
                    unexploredPoints);
                unexploredPoints.Add(newUnexploredPoints, robotPosition);

                // Update the target if necessary
                if (exploredPoints.ContainsKey(targetUnexploredPoint))
                {
                    var currentTargetUnexploredPoint = targetUnexploredPoint;
                    SetNextTargetUnexploredPoint(
                        exploredPoints,
                        unexploredPoints,
                        currentTargetUnexploredPoint,
                        out targetUnexploredPoint);
                    pathToTargetMovementCommands = GetMovementCommandsForPath(
                        robotPosition,
                        GetPathToPoint(
                            robotPosition,
                            targetUnexploredPoint,
                            exploredPoints));
                }
            }
            Console.WriteLine();
            return exploredPoints;
        }

        public static Queue<int> GetMovementCommandsForPath(
            GridPoint robotPosition,
            IList<GridPoint> path)
        {
            var result = new Queue<int>();
            var currentPoint = robotPosition;
            foreach (var nextPoint in path)
            {
                if (currentPoint.Equals(nextPoint))
                    continue;
                int movementCommand = GetMovementCommandForNeighbors(currentPoint, nextPoint);
                result.Enqueue(movementCommand);
                currentPoint = nextPoint;
            }
            return result;
        }

        public static IList<GridPoint> GetPathToPoint(
            GridPoint robotPosition,
            GridPoint targetPoint,
            Dictionary<GridPoint, HullCellType> exploredPoints)
        {
            if (targetPoint == null)
                return new List<GridPoint>();
            IList<GridPoint> GetNeighbors(GridPoint p)
            {
                List<GridPoint> result = new List<GridPoint>();
                IList<GridPoint> neighbors = GetAdjacentPoints(p);
                foreach (var n in neighbors)
                {
                    // Only include explored points in the path
                    // unless the neighbor is the target
                    if (exploredPoints.ContainsKey(n)
                        && !HullCellType.Wall.Equals(exploredPoints[n]))
                    {
                        result.Add(n);
                    }
                    else if (targetPoint.Equals(n))
                        result.Add(n);
                }
                return result;
            }

            int Heuristic(GridPoint p)
            {
                return GridPoint.GetManhattanDistance(p, targetPoint);
            }

            var path = AStar.GetPath<GridPoint>(
                startPoint: robotPosition,
                endPoint: targetPoint,
                Heuristic: Heuristic,
                GetNeighbors: GetNeighbors,
                GetEdgeCost: (p1, p2) => { return 1; });

            return path;
        }

        public static IList<GridPoint> GetAdjacentPoints(GridPoint point)
        {
            var result = new List<GridPoint>() 
            {
                point.MoveLeft(1),
                point.MoveRight(1),
                point.MoveUp(1),
                point.MoveDown(1)
            };
            return result;
        }

        public static void SetNextTargetUnexploredPoint(
            Dictionary<GridPoint, HullCellType> exploredPoints,
            Tree<GridPoint> unexploredPoints,
            GridPoint currentTargetUnexploredPoint,
            out GridPoint newTargetUnexploredPoint)
        {
            bool GetIsUnexplored(GridPoint p)
            {
                return !exploredPoints.ContainsKey(p);
            }
            newTargetUnexploredPoint = unexploredPoints.FindAndTrim(
                currentTargetUnexploredPoint, 
                GetIsUnexplored);
        }

        public static GridPoint GetClosestUnexploredPoint(
            GridPoint robotPosition,
            HashSet<GridPoint> unexploredPoints,
            Dictionary<GridPoint, HullCellType> exploredPoints,
            out Queue<int> pathToTargetCommands)
        {
            GridPoint bestTarget = null;
            IList<GridPoint> bestPath = new List<GridPoint>();
            int bestPathDistance = int.MaxValue;
            foreach (var target in unexploredPoints)
            {
                var path = GetPathToPoint(robotPosition, target, exploredPoints);
                if (path.Count == 0)
                    continue;
                if (path.Count < bestPathDistance)
                {
                    bestTarget = target;
                    bestPath = path;
                    bestPathDistance = path.Count;
                }
            }
            if (bestTarget != null)
            {
                pathToTargetCommands = GetMovementCommandsForPath(robotPosition, bestPath);
            }
            else
            {
                pathToTargetCommands = new Queue<int>();
            }
            return bestTarget;
        }

        public static IList<GridPoint> GetNewUnexploredPoints(
            GridPoint robotPosition,
            Dictionary<GridPoint, HullCellType> exploredPoints,
            Tree<GridPoint> unexploredPoints)
        {
            IList<GridPoint> result = new List<GridPoint>();
            for (int i = 1; i <= 4; i++)
            {
                var nextPoint = GetNextPoint(robotPosition, i);
                if (!exploredPoints.ContainsKey(nextPoint) 
                    && !unexploredPoints.Contains(nextPoint))
                {
                    result.Add(nextPoint);
                }
            }
            return result;
        }

        public static int GetMovementCommandForNeighbors(
            GridPoint startPoint,
            GridPoint endPoint)
        {
            // Only four movement commands are understood: north (1), 
            // south (2), west (3), and east (4). Any other command is invalid. 
            // The movements differ in direction, but not in distance: in a 
            // long enough east-west hallway, a series of commands like 
            // 4,4,4,4,3,3,3,3 would leave the repair droid back where it 
            // started.
            if (endPoint.Y == startPoint.Y + 1)
                return 1;
            else if (endPoint.Y == startPoint.Y - 1)
                return 2;
            else if (endPoint.X == startPoint.X - 1)
                return 3;
            else if (endPoint.X == startPoint.X + 1)
                return 4;
            else
                throw new Exception("Points are not adjacent");
        }

        public static GridPoint GetNextPoint(
            GridPoint robotPosition,
            int movementCommand)
        {
            // Only four movement commands are understood: north (1), 
            // south (2), west (3), and east (4). Any other command is invalid. 
            // The movements differ in direction, but not in distance: in a 
            // long enough east-west hallway, a series of commands like 
            // 4,4,4,4,3,3,3,3 would leave the repair droid back where it 
            // started.
            if (movementCommand == 1)
                return robotPosition.MoveUp(1);
            else if (movementCommand == 2)
                return robotPosition.MoveDown(1);
            else if (movementCommand == 3)
                return robotPosition.MoveLeft(1);
            else if (movementCommand == 4)
                return robotPosition.MoveRight(1);
            else
                throw new Exception($"Invalid movement command {movementCommand}");
        }

        public static HullCellType ParseHullCellType(int statusCode)
        {
            // The repair droid can reply with any of the following status codes:
            // 0: The repair droid hit a wall. Its position has not changed.
            // 1: The repair droid has moved one step in the requested direction.
            // 2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
            if (statusCode == 0)
                return HullCellType.Wall;
            else if (statusCode == 1)
                return HullCellType.Empty;
            else if (statusCode == 2)
                return HullCellType.OxygenSystem;
            else
                throw new Exception($"Invalid status code {statusCode}");
        }

        public static BigInteger[] GetDay15Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
