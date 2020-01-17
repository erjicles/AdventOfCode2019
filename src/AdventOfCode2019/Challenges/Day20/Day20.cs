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
            // In your maze, how many steps does it take to get from the open 
            // tile marked AA to the open tile marked ZZ?
            // Answer: 588
            var mazeDefinition = GetDay20Input();
            var maze = new DonutMaze(mazeDefinition);
            maze.DrawMaze();
            var pathResult = GetShortestPathThroughMaze(maze);
            return pathResult.TotalPathCost;
        }

        public static int GetDay20Part2Answer()
        {
            // In your maze, when accounting for recursion, how many steps 
            // does it take to get from the open tile marked AA to the open 
            // tile marked ZZ, both at the outermost layer?
            // Answer: 6834
            var mazeDefinition = GetDay20Input();
            var maze = new DonutMaze(mazeDefinition, isRecursive: true);
            maze.DrawMaze();
            var pathResult = GetShortestPathThroughMaze(maze);
            return pathResult.TotalPathCost;
        }

        public static PathResult<GridPoint3D> GetShortestPathThroughMaze(DonutMaze maze)
        {
            int Heuristic(GridPoint3D point)
            {
                // We can no longer use a heuristic because the portals make
                // it so that the manhattan distance can overestimate the
                // remaining distance.
                // TODO: Use the distance to the closest portal OR to the
                // finish instead (whichever is closer)
                if (!maze.IsRecursive)
                    return 0;
                return point.Z;
            }
            int GetEdgeCost(GridPoint3D p1, GridPoint3D p2)
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
