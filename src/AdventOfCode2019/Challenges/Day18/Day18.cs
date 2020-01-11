using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day18
{
    /// <summary>
    /// Solution to the Day 18 challenge:
    /// https://adventofcode.com/2019/day/18
    /// </summary>
    public class Day18
    {
        public const string FILE_NAME = "Day18Input.txt";

        public static int GetDay18Part1Answer()
        {
            // How many steps is the shortest path that collects all of the keys?
            // Answer: 3216
            var mazeDefinition = GetDay18Input();
            var maze = new Maze(mazeDefinition);
            var initialMazeState = new MazeState(maze, maze.StartingPositions, new SortedDictionary<string, string>());
            initialMazeState.DrawMazeState();
            var neighbors = initialMazeState.GetNeighboringMazeStatesWithCosts();
            Console.WriteLine("Neighboring states and costs:");
            Console.WriteLine($"{string.Join("; ", neighbors.Select(n => GetNeighboringMazeStateString(n)))}");
            var shortestPath = GetShortestPathToCollectAllKeys(maze);
            Console.WriteLine($"{MazeState.GetPathString(shortestPath.Path, false)}");
            var result = shortestPath.TotalPathCost;
            //var result = 3216;
            return result;
        }

        public static int GetDay18Part2Answer()
        {
            // After updating your map and using the remote-controlled robots, 
            // what is the fewest steps necessary to collect all of the keys?
            // Answer: 1538
            var mazeDefinition = GetDay18Input();
            var maze = new Maze(mazeDefinition, true);
            var initialMazeState = new MazeState(maze, maze.StartingPositions, new SortedDictionary<string, string>());
            initialMazeState.DrawMazeState();
            var neighbors = initialMazeState.GetNeighboringMazeStatesWithCosts();
            Console.WriteLine("Neighboring states and costs:");
            Console.WriteLine($"{string.Join("; ", neighbors.Select(n => GetNeighboringMazeStateString(n)))}");
            var shortestPath = GetShortestPathToCollectAllKeys(maze);
            Console.WriteLine($"{MazeState.GetPathString(shortestPath.Path, false)}");
            var result = shortestPath.TotalPathCost;
            return result;
        }

        public static string GetNeighboringMazeStateString(Tuple<MazeState, int, int> neighbor)
        {
            return $"({neighbor.Item1}, {neighbor.Item2}, Pos Index: {neighbor.Item3})";
        }

        public static PathResult<MazeState> GetShortestPathToCollectAllKeys(Maze maze)
        {
            var initialMazeState = new MazeState(maze, maze.StartingPositions, new SortedDictionary<string, string>());
            var allKeysDictionary = maze.Keys.ToDictionary(k => k, k => k);
            var finalMazeState = new MazeState(maze, maze.StartingPositions, new SortedDictionary<string, string>(allKeysDictionary));
            var edgeCosts = new Dictionary<Tuple<string, string>, int>();
            var edgePositionIndexes = new Dictionary<Tuple<string, string>, int>();
            int averageManhattanDistanceBetweenKeys = maze.GetAverageManhattanDistanceBetweenKeys();
            int sqrtOfTotalKeys = (int)Math.Sqrt(maze.Keys.Count);

            int Heuristic(MazeState state)
            {
                return state.Maze.Keys.Count - state.KeysCollected.Count;
            }

            IList<MazeState> GetNeighbors(MazeState mazeState)
            {
                var result = new List<MazeState>();

                var neighboringStatesWithCosts = mazeState.GetNeighboringMazeStatesWithCosts();
                foreach (var neighboringStateWithCost in neighboringStatesWithCosts)
                {
                    // Get the new key reached by the position that moved
                    // from the current state to this neighbor
                    //var positionThatMovedIndex = neighboringStateWithCost.Item3;
                    //var finalGridPointOfPositionThatMoved = neighboringStateWithCost.Item1.CurrentPositions[positionThatMovedIndex];
                    //var neighborKey = mazeState.Maze.CellsWithKeys[finalGridPointOfPositionThatMoved];

                    var edgeKey = new Tuple<string, string>(
                        mazeState.MazeStateSignature,
                        neighboringStateWithCost.Item1.MazeStateSignature);

                    if (!edgeCosts.ContainsKey(edgeKey))
                    {
                        edgeCosts.Add(edgeKey, int.MaxValue);
                    }
                    edgeCosts[edgeKey] = neighboringStateWithCost.Item2;

                    if (!edgePositionIndexes.ContainsKey(edgeKey))
                    {
                        edgePositionIndexes.Add(edgeKey, 0);
                    }
                    edgePositionIndexes[edgeKey] = neighboringStateWithCost.Item3;

                    result.Add(neighboringStateWithCost.Item1);
                }
                return result;
            }

            int GetEdgeCost(MazeState s1, MazeState s2)
            {
                var edgeKey = new Tuple<string, string>(
                    s1.MazeStateSignature,
                    s2.MazeStateSignature);

                if (!edgeCosts.ContainsKey(edgeKey))
                {
                    throw new Exception("Edge not in edge costs map");
                }
                return edgeCosts[edgeKey];
            }

            var result = AStar.GetPath<MazeState>(
                startPoint: initialMazeState,
                endPoint: finalMazeState,
                Heuristic: Heuristic,
                GetNeighbors: GetNeighbors,
                GetEdgeCost: GetEdgeCost);

            return result;
        }

        public static IList<string> GetDay18Input()
        {
            var result = FileHelper.ReadInputFileLines(FILE_NAME);
            return result;
        }
    }
}
