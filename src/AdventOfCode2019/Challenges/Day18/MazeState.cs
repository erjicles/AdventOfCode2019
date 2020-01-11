using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day18
{
    public class MazeState
    {
        public IList<GridPoint> CurrentPositions { get; private set; }
        public SortedDictionary<string, string> KeysCollected { get; private set; }
        public string MazeStateSignature { get; private set; }
        public Maze Maze { get; set; }

        public IList<Tuple<string, IList<GridPoint>, HashSet<string>>> Neighbors;

        public MazeState(Maze maze, IList<GridPoint> currentPositions, SortedDictionary<string, string> keysCollected)
        {
            Maze = maze;
            CurrentPositions = currentPositions;
            KeysCollected = keysCollected;
            MazeStateSignature = GetMazeStateSignature();
        }

        private string GetMazeStateSignature()
        {
            var signatureBuilder = new StringBuilder();
            if (KeysCollected.Count < Maze.Keys.Count)
            {
                foreach (var currentPosition in CurrentPositions)
                {
                    if (Maze.CellsWithKeys.ContainsKey(currentPosition))
                    {
                        signatureBuilder.Append(Maze.CellsWithKeys[currentPosition]);
                    }
                    else
                    {
                        signatureBuilder.Append("@");
                    }
                }
            }
            signatureBuilder.Append(":");
            signatureBuilder.Append(string.Join("", KeysCollected.Select(kvp => kvp.Key)));
            return signatureBuilder.ToString();
        }

        /// <summary>
        /// Retrieves all neighboring maze states that can be reached by
        /// moving one robot to an accessible key.
        /// Item2 is the cost of moving from the current state to the neighbor.
        /// Item3 is the index of the position that moved.
        /// </summary>
        /// <returns></returns>
        public IList<Tuple<MazeState, int, int>> GetNeighboringMazeStatesWithCosts()
        {
            var result = new List<Tuple<MazeState, int, int>>();
            for (int i = 0; i < CurrentPositions.Count; i++)
            {
                var neighborsForPosition = GetNeighboringMazeStatesWithCostsForPosition(i);
                result.AddRange(neighborsForPosition);
            }

            return result;
        }

        /// <summary>
        /// Retrieves all neighboring maze states that can be reached by moving
        /// the given position.
        /// </summary>
        /// <param name="positionIndex"></param>
        /// <returns></returns>
        private IList<Tuple<MazeState, int, int>> GetNeighboringMazeStatesWithCostsForPosition(int positionIndex)
        {
            var result = new List<Tuple<MazeState, int, int>>();
            var currentPosition = CurrentPositions[positionIndex];
            foreach (var targetKey in Maze.Keys)
            {
                if (KeysCollected.ContainsKey(targetKey))
                    continue;

                var targetKeyCell = Maze.KeyCells[targetKey];

                // SIMPLIFYING ASSUMPTION: 
                // The shortest paths between all pairs of keys when ignoring
                // doors always yields the ultimate shortest path
                var edgeKey = new Tuple<GridPoint, GridPoint>(currentPosition, targetKeyCell);
                if (!Maze.ShortestPathsBetweenKeysIgnoringDoors.ContainsKey(edgeKey))
                    continue;
                bool canReachTarget = true;
                foreach (var door in Maze.DoorsAlongShortestPathBetweenKeys[edgeKey])
                {
                    if (!KeysCollected.ContainsKey(door.ToLower()))
                        canReachTarget = false;
                }
                if (!canReachTarget)
                    continue;
                var shortestPath = Maze.ShortestPathsBetweenKeysIgnoringDoors[edgeKey];
                var keysCollectedAlongPath = Maze.GetKeysAlongPath(shortestPath);
                var totalCost = shortestPath.Count - 1;

                //// THIS CODE WORKS IN THE GENERAL CASE WHEN THE SIMPLIFYING ASSUMPTION IS NOT MET
                //int Heuristic(GridPoint currentCell)
                //{
                //    return GridPoint.GetManhattanDistance(currentCell, targetKeyCell);
                //}
                //bool GetCanEnterNode(GridPoint point)
                //{
                //    return Maze.GetCanEnterCell(point, KeysCollected);
                //}
                //var nodePathToTargetKeyCell = Maze.MazeGraph.GetShortestPathViaNodes(
                //    start: currentPosition,
                //    end: targetKeyCell,
                //    Heuristic: Heuristic,
                //    GetCanEnterNode: GetCanEnterNode);
                //if (nodePathToTargetKeyCell.Path.Count == 0)
                //    continue;
                //var keysCollectedAlongPath = Maze.GetKeysCollectedAlongNodePath(nodePathToTargetKeyCell.Path);
                //var totalCost = nodePathToTargetKeyCell.TotalPathCost;

                var endKeysCollected = new SortedDictionary<string, string>(KeysCollected);
                foreach (var key in keysCollectedAlongPath)
                {
                    if (!endKeysCollected.ContainsKey(key))
                        endKeysCollected.Add(key, key);
                }
                var finalPositions = CurrentPositions.ToList();
                finalPositions[positionIndex] = targetKeyCell;
                var newMazeState = new MazeState(
                    maze: Maze,
                    currentPositions: finalPositions,
                    keysCollected: endKeysCollected);
                var resultEntry = new Tuple<MazeState, int, int>(
                    newMazeState,
                    totalCost,
                    positionIndex);
                result.Add(resultEntry);
            }
            return result;
        }

        public void DrawMazeState()
        {
            GridHelper.DrawGrid2D(
                gridPoints: Maze.MazeCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString,
                GetPointColor: GetCellColor);
        }

        public string GetCellString(GridPoint point)
        {
            if (!Maze.MazeCells.ContainsKey(point))
                return " ";

            var cell = Maze.MazeCells[point];

            if (MazeCellType.Wall.Equals(cell.Type))
                return "#";
            if (Maze.CellsWithDoors.ContainsKey(point)
                && !KeysCollected.ContainsKey(Maze.CellsWithDoors[point].ToLower()))
                return Maze.CellsWithDoors[point];
            if (Maze.CellsWithKeys.ContainsKey(point)
                && !KeysCollected.ContainsKey(Maze.CellsWithKeys[point]))
                return Maze.CellsWithKeys[point];
            if (CurrentPositions.Contains(point))
                return "@";
            return ".";
        }

        public ConsoleColor GetCellColor(GridPoint point)
        {
            if (!Maze.MazeCells.ContainsKey(point))
                return Console.ForegroundColor;

            var cell = Maze.MazeCells[point];

            if (MazeCellType.Wall.Equals(cell.Type))
                return ConsoleColor.DarkGray;
            if (Maze.CellsWithDoors.ContainsKey(point)
                && !KeysCollected.ContainsKey(Maze.CellsWithDoors[point].ToLower()))
                return ConsoleColor.Red;
            if (Maze.CellsWithKeys.ContainsKey(point)
                && !KeysCollected.ContainsKey(Maze.CellsWithKeys[point]))
                return ConsoleColor.Cyan;
            if (CurrentPositions.Contains(point))
                return ConsoleColor.Green;
            return Console.ForegroundColor;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return MazeStateSignature.Equals(((MazeState)obj).MazeStateSignature);
            }
        }

        public override int GetHashCode()
        {
            return MazeStateSignature.GetHashCode();
        }

        public override string ToString()
        {
            return MazeStateSignature;
        }

        public static string GetPathString(IList<MazeState> path, bool includeFullState)
        {
            var pathStringBuilder = new StringBuilder();
            pathStringBuilder.Append("Start");
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathStringBuilder.Append("  --->  ");
                var startState = path[i];
                var endState = path[i+1];
                if (includeFullState)
                {
                    pathStringBuilder.Append("[");
                    pathStringBuilder.Append(startState.MazeStateSignature);
                    pathStringBuilder.Append(" -> ");
                    pathStringBuilder.Append(endState.MazeStateSignature);
                    pathStringBuilder.Append("]");
                    continue;
                }
                
                int positionThatChangedIndex = GetPositionThatChangedIndex(startState, endState);
                if (positionThatChangedIndex < 0)
                {
                    pathStringBuilder.Append("[No movement]");
                    continue;
                }

                pathStringBuilder.Append($"[{positionThatChangedIndex}:");
                var startPositionForPositionThatChanged = startState.CurrentPositions[positionThatChangedIndex];
                if (startState.Maze.CellsWithKeys.ContainsKey(startPositionForPositionThatChanged))
                    pathStringBuilder.Append(startState.Maze.CellsWithKeys[startPositionForPositionThatChanged]);
                else
                    pathStringBuilder.Append("@");
                pathStringBuilder.Append("->");
                var finalPositionForPositionThatChanged = endState.CurrentPositions[positionThatChangedIndex];
                if (endState.Maze.CellsWithKeys.ContainsKey(finalPositionForPositionThatChanged))
                    pathStringBuilder.Append(endState.Maze.CellsWithKeys[finalPositionForPositionThatChanged]);
                else
                    pathStringBuilder.Append("@");
                pathStringBuilder.Append("]");
            }
            return pathStringBuilder.ToString();
        }

        public static int GetPositionThatChangedIndex(MazeState s1, MazeState s2)
        {
            for (int positionThatChangedIndex = 0; positionThatChangedIndex < s1.CurrentPositions.Count; positionThatChangedIndex++)
            {
                if (!s1.CurrentPositions[positionThatChangedIndex].Equals(
                    s2.CurrentPositions[positionThatChangedIndex]))
                {
                    return positionThatChangedIndex;
                }
            }
            return -1;
        }
    }
}
