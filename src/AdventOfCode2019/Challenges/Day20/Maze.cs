using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day20
{
    public class Maze
    {
        public Dictionary<GridPoint, MazeCell> MazeCells { get; private set; }
        public Dictionary<GridPoint, string> PortalCells { get; private set; }
        public Dictionary<string, IList<GridPoint>> Portals { get; private set; }
        public GridPoint EntrancePoint { get; private set; }
        public GridPoint ExitPoint { get; private set; }
        public const string EntrancePortalId = "AA";
        public const string ExitPortalId = "ZZ";
        public Maze(IList<string> mazeDefinition)
        {
            InitializeMaze(mazeDefinition);
        }
        private void InitializeMaze(
            IList<string> mazeDefinition)
        {
            MazeCells = new Dictionary<GridPoint, MazeCell>();
            for (int y = 0; y < mazeDefinition.Count; y++)
            {
                var rowString = mazeDefinition[y];
                for (int x = 0; x < rowString.Length; x++)
                {
                    var point = new GridPoint(x, y);
                    var cellDefinition = rowString[x].ToString();
                    if (string.IsNullOrWhiteSpace(cellDefinition))
                        continue;
                    var mazeCellType = MazeCellType.Empty;
                    var portalLetter = string.Empty;
                    if ("#".Equals(cellDefinition))
                    {
                        mazeCellType = MazeCellType.Wall;
                    }
                    else if (Regex.IsMatch(cellDefinition, @"^[A-Z]$"))
                    {
                        mazeCellType = MazeCellType.Portal;
                        portalLetter = cellDefinition;
                    }

                    var mazeCell = new MazeCell(point, mazeCellType, portalLetter);
                    MazeCells.Add(point, mazeCell);
                }
            }
            ConstructPortals();
        }

        private void ConstructPortals()
        {
            PortalCells = new Dictionary<GridPoint, string>();
            Portals = new Dictionary<string, IList<GridPoint>>();
            var emptyCells = MazeCells
                .Where(kvp => MazeCellType.Empty.Equals(kvp.Value.Type));
            foreach (var emptyCell in emptyCells)
            {
                var point = emptyCell.Key;
                var movementDirections = new List<MovementDirection>()
                {
                    MovementDirection.Down,
                    MovementDirection.Left,
                    MovementDirection.Right,
                    MovementDirection.Up
                };
                foreach (var direction in movementDirections)
                {
                    // Assumption: One point can only be associated with at
                    // most one portal.
                    var neighbor1 = point.Move(direction, 1);
                    if (!MazeCells.ContainsKey(neighbor1)
                        || !MazeCellType.Portal.Equals(MazeCells[neighbor1].Type))
                    {
                        continue;
                    }
                    var neighbor2 = point.Move(direction, 2);
                    if (!MazeCells.ContainsKey(neighbor2)
                        || !MazeCellType.Portal.Equals(MazeCells[neighbor2].Type))
                        throw new Exception("Only one neighboring portal cell found");
                    var portalId = string.Empty;
                    if (MovementDirection.Right.Equals(direction)
                        || MovementDirection.Up.Equals(direction))
                    {
                        portalId = MazeCells[neighbor1].PortalLetter + MazeCells[neighbor2].PortalLetter;
                    }
                    else
                    {
                        portalId = MazeCells[neighbor2].PortalLetter + MazeCells[neighbor1].PortalLetter;
                    }

                    PortalCells.Add(point, portalId);
                    if (!Portals.ContainsKey(portalId))
                        Portals.Add(portalId, new List<GridPoint>());
                    Portals[portalId].Add(point);
                }
            }
            EntrancePoint = Portals[EntrancePortalId][0];
            ExitPoint = Portals[ExitPortalId][0];
        }

        public IList<GridPoint> GetEmptyNeighbors(GridPoint currentPosition)
        {
            var result = new List<GridPoint>();
            var movementDirections = new List<MovementDirection>()
                {
                    MovementDirection.Down,
                    MovementDirection.Left,
                    MovementDirection.Right,
                    MovementDirection.Up
                };
            foreach (var movementDirection in movementDirections)
            {
                var neighborPoint = currentPosition.Move(movementDirection, 1);
                if (!MazeCells.ContainsKey(neighborPoint))
                    continue;
                var type = MazeCells[neighborPoint].Type;
                if (MazeCellType.Wall.Equals(type))
                    continue;
                if (MazeCellType.Empty.Equals(type))
                {
                    result.Add(neighborPoint);
                }
                else if (MazeCellType.Portal.Equals(type))
                {
                    // Assumption: One point can be associated with at most
                    // one portal
                    var portalId = PortalCells[currentPosition];
                    if (EntrancePortalId.Equals(portalId)
                        || ExitPortalId.Equals(portalId))
                        continue;
                    var connectedPoint = Portals[portalId].Where(p => !p.Equals(currentPosition)).FirstOrDefault();
                    if (connectedPoint == null)
                        throw new Exception("Portal found with no connected point");
                    result.Add(connectedPoint);
                }
                else
                    throw new Exception($"Invalid cell type {type}");
            }
            return result;
        }

        public string GetCellString(GridPoint point)
        {
            if (!MazeCells.ContainsKey(point))
                return " ";
            return MazeCell.GetCellString(MazeCells[point]);
        }

        public void DrawMaze()
        {
            GridHelper.DrawGrid2D(
                gridPoints: MazeCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString);
        }

        public IList<Tuple<string, ConsoleColor>> GetMazeRenderingData()
        {
            return GridHelper.GetGridRenderingData(
                gridPoints: MazeCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString);
        }
    }
}
