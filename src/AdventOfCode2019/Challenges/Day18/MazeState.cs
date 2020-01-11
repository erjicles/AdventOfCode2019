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
        public GridPoint CurrentPosition { get; private set; }
        public SortedDictionary<string, string> KeysCollected { get; private set; }
        //public IList<Tuple<string, IList<GridPoint>, IList<string>>> AccessibleUncollectedKeysWithPaths { get; private set; }
        //public HashSet<string> AccessibleUncollectedKeys { get; private set; }
        //public HashSet<string> InaccessibleUncollectedKeys { get; private set; }
        public string MazeStateSignature { get; private set; }
        public Maze Maze { get; set; }

        public IList<Tuple<string, IList<GridPoint>, HashSet<string>>> Neighbors;

        public MazeState(Maze maze, GridPoint currentPosition, SortedDictionary<string, string> keysCollected)
        {
            Maze = maze;
            CurrentPosition = currentPosition;
            KeysCollected = keysCollected;
            //AccessibleUncollectedKeysWithPaths = GetAccessibleKeysWithPaths();
            //AccessibleUncollectedKeys = AccessibleUncollectedKeysWithPaths.Select(t => t.Item1).ToHashSet();
            //InaccessibleUncollectedKeys = maze.Keys.ToHashSet();
            //InaccessibleUncollectedKeys.RemoveWhere(k => KeysCollected.Contains(k) || AccessibleUncollectedKeys.Contains(k));
            MazeStateSignature = GetMazeStateSignature();
        }

        private string GetMazeStateSignature()
        {
            var mostRecentKeyPart = string.Empty;
            if (KeysCollected.Count < Maze.Keys.Count)
            {
                if (Maze.CellsWithKeys.ContainsKey(CurrentPosition))
                {
                    mostRecentKeyPart = Maze.CellsWithKeys[CurrentPosition] + ":";
                }   
            }
            var collectedKeysPart = string.Join("", KeysCollected.Select(kvp => kvp.Key));
            return mostRecentKeyPart + collectedKeysPart;
        }

        public IList<Tuple<MazeState, int>> GetNeighboringMazeStatesWithCosts()
        {
            var result = new List<Tuple<MazeState, int>>();
            foreach (var targetKey in Maze.Keys)
            {
                if (KeysCollected.ContainsKey(targetKey))
                    continue;

                var targetKeyCell = Maze.KeyCells[targetKey];

                //// SIMPLIFYING ASSUMPTION: 
                //// The shortest paths between all pairs of keys when ignoring
                //// doors always yields the ultimate shortest path
                //var edgeKey = new Tuple<GridPoint, GridPoint>(CurrentPosition, targetKeyCell);
                //if (!Maze.ShortestPathsBetweenKeysIgnoringDoors.ContainsKey(edgeKey))
                //    throw new Exception("Edge not in map");
                //bool canReachTarget = true;
                //foreach (var door in Maze.DoorsAlongShortestPathBetweenKeys[edgeKey])
                //{
                //    if (!KeysCollected.ContainsKey(door.ToLower()))
                //        canReachTarget = false;
                //}
                //if (!canReachTarget)
                //    continue;
                //var shortestPath = Maze.ShortestPathsBetweenKeysIgnoringDoors[edgeKey];
                //var keysCollectedAlongPath = Maze.GetKeysAlongPath(shortestPath);
                //var totalCost = shortestPath.Count - 1;

                // THIS CODE WORKS IN THE GENERAL CASE WHEN THE SIMPLIFYING ASSUMPTION IS NOT MET
                int Heuristic(GridPoint currentCell)
                {
                    return GridPoint.GetManhattanDistance(currentCell, targetKeyCell);
                }
                bool GetCanEnterNode(GridPoint point)
                {
                    return Maze.GetCanEnterCell(point, KeysCollected);
                }
                var nodePathToTargetKeyCell = Maze.MazeGraph.GetShortestPathViaNodes(
                    start: CurrentPosition,
                    end: targetKeyCell,
                    Heuristic: Heuristic,
                    GetCanEnterNode: GetCanEnterNode);
                if (nodePathToTargetKeyCell.Path.Count == 0)
                    continue;
                var keysCollectedAlongPath = Maze.GetKeysCollectedAlongNodePath(nodePathToTargetKeyCell.Path);
                var totalCost = nodePathToTargetKeyCell.TotalPathCost;

                var endKeysCollected = new SortedDictionary<string, string>(KeysCollected);
                foreach (var key in keysCollectedAlongPath)
                {
                    if (!endKeysCollected.ContainsKey(key))
                        endKeysCollected.Add(key, key);
                }
                var newMazeState = new MazeState(
                    maze: Maze,
                    currentPosition: targetKeyCell,
                    keysCollected: endKeysCollected);
                var resultEntry = new Tuple<MazeState, int>(
                    newMazeState,
                    totalCost);
                result.Add(resultEntry);
            }

            return result;
        }

        public void DrawMazeState()
        {
            GridHelper.DrawGrid2D(
                gridPoints: Maze.MazeCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString);
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
            if (CurrentPosition.Equals(point))
                return "@";
            return ".";
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
    }
}
