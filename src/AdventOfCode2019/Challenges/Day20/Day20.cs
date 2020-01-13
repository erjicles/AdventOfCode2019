using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day20
{
    /// <summary>
    /// Solution to the Day 20 challenge:
    /// https://adventofcode.com/2019/day/20
    /// </summary>
    public class Day20
    {
        public const string FILE_NAME = "Day20Input.txt";
        public static int GetDay20Part1Answer()
        {
            var mazeDefinition = GetDay20Input();
            var maze = new Maze(mazeDefinition);
            maze.DrawMaze();
            var pathResult = GetShortestPathThroughMaze(maze);
            return pathResult.TotalPathCost;
        }

        public static PathResult<GridPoint> GetShortestPathThroughMaze(Maze maze)
        {
            int Heuristic(GridPoint point)
            {
                // We can no longer use a heuristic because the portals make
                // it so that the manhattan distance can overestimate the
                // remaining distance.
                // TODO: Use the distance to the closest portal OR to the
                // finish instead (whichever is closer)
                return 0;
            }
            int GetEdgeCost(GridPoint p1, GridPoint p2)
            {
                return 1;
            }
            var pathResult = AStar.GetPath(
                startPoint: maze.EntrancePoint,
                endPoint: maze.ExitPoint,
                Heuristic: Heuristic,
                GetNeighbors: maze.GetEmptyNeighbors,
                GetEdgeCost: GetEdgeCost);
            return pathResult;
        }

        public static IList<string> GetDay20Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
