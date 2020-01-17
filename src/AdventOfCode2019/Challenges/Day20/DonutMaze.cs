using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day20
{
    public class DonutMaze
    {
        public Dictionary<GridPoint, DonutMazeCell> MazeCells { get; private set; }
        public Dictionary<GridPoint, string> PortalCells { get; private set; }
        public Dictionary<string, IList<GridPoint>> Portals { get; private set; }
        public GridPoint3D EntrancePoint { get; private set; }
        public GridPoint3D ExitPoint { get; private set; }
        public bool IsRecursive { get; private set; }
        public int OuterWallLeft { get; private set; }
        public int OuterWallRight { get; private set; }
        public int OuterWallTop { get; private set; }
        public int OuterWallBottom { get; private set; }
        public int InnerWallLeft { get; private set; }
        public int InnerWallRight { get; private set; }
        public int InnerWallTop { get; private set; }
        public int InnerWallBottom { get; private set; }
        public const string EntrancePortalId = "AA";
        public const string ExitPortalId = "ZZ";
        public DonutMaze(IList<string> mazeDefinition, bool isRecursive = false)
        {
            IsRecursive = isRecursive;
            InitializeMaze(mazeDefinition);
        }
        private void InitializeMaze(
            IList<string> mazeDefinition)
        {
            MazeCells = new Dictionary<GridPoint, DonutMazeCell>();
            for (int y = 0; y < mazeDefinition.Count; y++)
            {
                var rowString = mazeDefinition[y];
                for (int x = 0; x < rowString.Length; x++)
                {
                    var point = new GridPoint(x, y);
                    var cellDefinition = rowString[x].ToString();
                    if (string.IsNullOrWhiteSpace(cellDefinition))
                        continue;
                    var mazeCellType = DonutMazeCellType.Empty;
                    var portalLetter = string.Empty;
                    if ("#".Equals(cellDefinition))
                    {
                        mazeCellType = DonutMazeCellType.Wall;
                    }
                    else if (Regex.IsMatch(cellDefinition, @"^[A-Z]$"))
                    {
                        mazeCellType = DonutMazeCellType.Portal;
                        portalLetter = cellDefinition;
                    }

                    var mazeCell = new DonutMazeCell(point, mazeCellType, portalLetter);
                    MazeCells.Add(point, mazeCell);
                }
            }
            CalculateMazeBoundaries();
            ConstructPortals();
        }

        private void CalculateMazeBoundaries()
        {
            OuterWallLeft = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type))
                .Min(kvp => kvp.Key.X);
            OuterWallRight = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type))
                .Max(kvp => kvp.Key.X);
            OuterWallTop = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type))
                .Min(kvp => kvp.Key.Y);
            OuterWallBottom = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type))
                .Max(kvp => kvp.Key.Y);
            int midX = (OuterWallRight - OuterWallLeft) / 2;
            int midY = (OuterWallBottom - OuterWallTop) / 2;
            InnerWallLeft = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type)
                    && kvp.Key.X <= midX
                    && kvp.Key.Y == midY)
                .Max(kvp => kvp.Key.X);
            InnerWallRight = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type)
                    && kvp.Key.X >= midX
                    && kvp.Key.Y == midY)
                .Min(kvp => kvp.Key.X);
            InnerWallTop = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type)
                    && kvp.Key.Y <= midY
                    && kvp.Key.X == midX)
                .Max(kvp => kvp.Key.Y);
            InnerWallBottom = MazeCells
                .Where(kvp => !DonutMazeCellType.Portal.Equals(kvp.Value.Type)
                    && kvp.Key.Y >= midY
                    && kvp.Key.X == midX)
                .Min(kvp => kvp.Key.Y);
        }

        private void ConstructPortals()
        {
            PortalCells = new Dictionary<GridPoint, string>();
            Portals = new Dictionary<string, IList<GridPoint>>();
            var emptyCells = MazeCells
                .Where(kvp => DonutMazeCellType.Empty.Equals(kvp.Value.Type));
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
                        || !DonutMazeCellType.Portal.Equals(MazeCells[neighbor1].Type))
                    {
                        continue;
                    }
                    var neighbor2 = point.Move(direction, 2);
                    if (!MazeCells.ContainsKey(neighbor2)
                        || !DonutMazeCellType.Portal.Equals(MazeCells[neighbor2].Type))
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
            EntrancePoint = new GridPoint3D(Portals[EntrancePortalId][0], 0);
            ExitPoint = new GridPoint3D(Portals[ExitPortalId][0], 0);
        }

        public bool GetIsOnInnerWall(GridPoint3D point)
        {
            if (point.X >= InnerWallLeft
                && point.X <= InnerWallRight
                && (
                    point.Y == InnerWallTop
                    || point.Y == InnerWallBottom))
            {
                return true;
            }
            if (point.Y >= InnerWallTop
                && point.Y <= InnerWallBottom
                && (
                    point.X == InnerWallLeft
                    || point.X == InnerWallRight))
            {
                return true;
            }
            return false;
        }

        public bool GetIsOnOuterWall(GridPoint3D point)
        {
            if (point.X >= OuterWallLeft
                && point.X <= OuterWallRight
                && (
                    point.Y == OuterWallTop
                    || point.Y == OuterWallBottom))
            {
                return true;
            }
            if (point.Y >= OuterWallTop
                && point.Y <= OuterWallBottom
                && (
                    point.X == OuterWallLeft
                    || point.X == OuterWallRight))
            {
                return true;
            }
            return false;
        }

        public IList<GridPoint3D> GetEmptyNeighbors(GridPoint3D currentPosition)
        {
            var result = new List<GridPoint3D>();
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
                if (!MazeCells.ContainsKey(neighborPoint.XYPoint))
                    continue;
                var type = MazeCells[neighborPoint.XYPoint].Type;
                if (DonutMazeCellType.Wall.Equals(type))
                    continue;
                if (DonutMazeCellType.Empty.Equals(type))
                {
                    result.Add(neighborPoint);
                }
                else if (DonutMazeCellType.Portal.Equals(type))
                {
                    // Assumption: One point can be associated with at most
                    // one portal
                    var portalId = PortalCells[currentPosition.XYPoint];
                    if (EntrancePortalId.Equals(portalId)
                        || ExitPortalId.Equals(portalId))
                        continue;

                    bool isOnOuterEdge = GetIsOnOuterWall(currentPosition);
                    // If this is a recursive maze, then outer portals only
                    // work for z > 1
                    if (IsRecursive
                        && isOnOuterEdge
                        && currentPosition.Z == 0)
                        continue;

                    var connectedPoint = Portals[portalId]
                        .Where(p => !p
                            .Equals(currentPosition.XYPoint))
                        .FirstOrDefault();
                    if (connectedPoint == null)
                        throw new Exception("Portal found with no connected point");

                    // Handle recursive maze - if this is an inner portal, then
                    // we are going in a level, so increment z
                    // If this is an outer portal, then we are going out a 
                    // level, so decrease z
                    int connectedPointZ = currentPosition.Z;
                    if (IsRecursive)
                    {
                        if (isOnOuterEdge)
                        {
                            connectedPointZ--;
                        }
                        else if (GetIsOnInnerWall(currentPosition))
                        {
                            connectedPointZ++;
                        }
                    }
                    // The connected point is stored in the maze at z=0, so
                    // move it to the appropriate z index
                    result.Add(new GridPoint3D(connectedPoint, connectedPointZ));
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
            return DonutMazeCell.GetCellString(MazeCells[point]);
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
