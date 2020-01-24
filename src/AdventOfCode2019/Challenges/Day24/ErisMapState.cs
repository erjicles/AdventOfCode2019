using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day24
{
    public class ErisMapState
    {
        public HashSet<GridPoint3D> BugCells { get; private set; }
        public bool IsRecursive { get; private set; }
        public GridPoint CenterPointXY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MinZ { get; private set; }
        public int MaxZ { get; private set; }
        public string Signature { get; private set; }
        public ErisMapState(
            HashSet<GridPoint3D> bugCells,
            int width,
            int height,
            int minZ,
            int maxZ,
            bool isRecursive)
        {
            BugCells = bugCells;
            IsRecursive = isRecursive;
            Width = width;
            Height = height;
            MinZ = minZ;
            MaxZ = maxZ;
            CenterPointXY = new GridPoint((Width - 1)/2, (Height - 1)/2);
            Signature = GetSignature();
        }

        public static ErisMapState CreateMap(
            IList<string> mapDefinition,
            bool isRecursive)
        {
            int height = mapDefinition.Count;
            int width = mapDefinition.Max(row => row.Length);
            var bugCells = new HashSet<GridPoint3D>();
            for (int y = 0; y < mapDefinition.Count; y++)
            {
                var rowDefinition = mapDefinition[y];
                for (int x = 0; x < rowDefinition.Length; x++)
                {
                    var cellDefinition = rowDefinition[x];
                    var point = new GridPoint3D(x, y, 0);
                    CellType cellType = cellDefinition switch
                    {
                        '.' => CellType.Empty,
                        '#' => CellType.Bug,
                        _ => CellType.Empty,
                    };
                    if (CellType.Bug.Equals(cellType))
                        bugCells.Add(point);
                }
            }
            var result = new ErisMapState(
                bugCells: bugCells,
                width: width, 
                height: height, 
                minZ: 0,
                maxZ: 0,
                isRecursive: isRecursive);
            return result;
        }

        public BigInteger GetBiodiversityRating(int layer)
        {
            BigInteger result = 0;
            BigInteger cellPointValue = 1;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var point = new GridPoint3D(x, y, layer);
                    if (BugCells.Contains(point))
                    {
                        result += cellPointValue;
                    }
                    cellPointValue *= 2;
                }
            }
            return result;
        }

        public ErisMapState Evolve()
        {
            var cellsAdjacentToBugs = GetAdjacentBugCounts(
                out HashSet<GridPoint3D> bugsNotAdjacentToOtherBugs);

            // Update cells based on rules
            var bugCells = BugCells.ToHashSet();
            foreach (var cellAdjacentToBug in cellsAdjacentToBugs)
            {
                var point = cellAdjacentToBug.Key;

                var bugCount = cellAdjacentToBug.Value;
                // Each minute, The bugs live and die based on the number of 
                // bugs in the four adjacent tiles:
                // A bug dies(becoming an empty space) unless there is exactly 
                // one bug adjacent to it.
                //An empty space becomes infested with a bug if exactly one or 
                // two bugs are adjacent to it.
                if (!bugCells.Contains(point)
                    && (bugCount == 1
                        || bugCount == 2))
                {
                    bugCells.Add(point);
                    if (MinZ > point.Z)
                    {
                        MinZ = point.Z;
                    }
                    else if (MaxZ < point.Z)
                    {
                        MaxZ = point.Z;
                    }   
                }
                else if (bugCells.Contains(point)
                    && bugCount != 1)
                {
                    bugCells.Remove(point);
                }
            }
            // Kill all bugs not adjacent to other bugs
            foreach (var bugPoint in bugsNotAdjacentToOtherBugs)
            {
                bugCells.Remove(bugPoint);
            }

            return new ErisMapState(
                bugCells: bugCells,
                width: Width, 
                height: Height, 
                minZ: MinZ,
                maxZ: MaxZ,
                isRecursive: IsRecursive);
        }

        public Dictionary<GridPoint3D, int> GetAdjacentBugCounts(
            out HashSet<GridPoint3D> bugsNotAdjacentToOtherBugs)
        {
            // Build a dictionary of all cells adjacent to bugs
            // and count the number of bugs adjacent to them
            // If the map is recursive, then add new layers if bugs are
            // adjacent to cells in the next inner or outer layer and the
            // layers haven't yet been added to the map
            var cellsAdjacentToBugs = new Dictionary<GridPoint3D, int>();
            bugsNotAdjacentToOtherBugs = BugCells.ToHashSet();
            foreach (var bugCell in BugCells)
            {
                var neighbors = GetNeighbors(bugCell);
                foreach (var neighbor in neighbors)
                {
                    if (GetIsPointOffMap(neighbor))
                    {
                        // We should only get points off the map if the map is
                        // not recursive
                        if (IsRecursive)
                            throw new Exception("Encountered point off map");
                        continue;
                    }
                    if (!cellsAdjacentToBugs.ContainsKey(neighbor))
                    {
                        cellsAdjacentToBugs.Add(neighbor, 0);
                    }
                    cellsAdjacentToBugs[neighbor]++;
                    if (bugsNotAdjacentToOtherBugs.Contains(neighbor))
                    {
                        bugsNotAdjacentToOtherBugs.Remove(neighbor);
                    }
                }
            }
            return cellsAdjacentToBugs;
        }

        public IList<GridPoint3D> GetNeighbors(GridPoint3D point)
        {
            var result = new List<GridPoint3D>();
            var directions = new List<MovementDirection>()
            {
                MovementDirection.Left,
                MovementDirection.Right,
                MovementDirection.Up,
                MovementDirection.Down
            };
            foreach (var direction in directions)
            {
                var neighbor = point.Move(direction, 1);
                if (!IsRecursive)
                {
                    result.Add(neighbor);
                }
                else // Recursive map
                {
                    // Check if the neighbor is the middle point
                    if (neighbor.XYPoint.Equals(CenterPointXY))
                    {
                        // Get the outer cells adjacent to this one in the
                        // inner layer
                        // Handle y is inverted (up actually decreases y),
                        // the inner layer side is actually the same as the
                        // vertical movement direction
                        var innerLayerSide = direction;
                        if (MovementDirection.Left.Equals(direction)
                            || MovementDirection.Right.Equals(direction))
                        {
                            innerLayerSide = MovementDirectionHelper.GetOppositeDirection(direction);
                        }
                        int[] edgeValues;
                        var coordinateOrder = string.Empty;
                        switch (innerLayerSide)
                        {
                            case MovementDirection.Left:
                                edgeValues = new int[4] { 0, point.Z + 1, 0, Height - 1 };
                                coordinateOrder = "XZY";
                                break;

                            case MovementDirection.Right:
                                edgeValues = new int[4] { Width - 1, point.Z + 1, 0, Height - 1 };
                                coordinateOrder = "XZY";
                                break;

                            case MovementDirection.Up:
                                edgeValues = new int[4] { 0, point.Z + 1, 0, Width - 1 };
                                coordinateOrder = "YZX";
                                break;

                            case MovementDirection.Down:
                                edgeValues = new int[4] { Height - 1, point.Z + 1, 0, Width - 1 };
                                coordinateOrder = "YZX";
                                break;

                            default:
                                throw new Exception($"Invalid side {direction}");
                        }

                        var neighborsOnInnerLayer = GridHelper3D.GetPointsAlongEdge(
                            edgeValues: edgeValues,
                            coordinateOrder: coordinateOrder);
                        result.AddRange(neighborsOnInnerLayer);
                    }
                    // Check if the neighbor is off the map
                    else if (GetIsPointOffMap(neighbor))
                    {
                        // Get the adjacent cell to the center in the outer layer
                        var outerLayerCenterCell = new GridPoint3D(CenterPointXY, point.Z - 1);
                        neighbor = outerLayerCenterCell.Move(direction, 1);
                        result.Add(neighbor);
                    }
                    else
                    {
                        result.Add(neighbor);
                    }
                }
            }
            return result;
        }

        public string GetCellString(GridPoint3D point)
        {
            if (BugCells.Contains(point))
                return "#";
            if (GetIsPointOffMap(point))
                return " ";
            if (IsRecursive && CenterPointXY.Equals(point.XYPoint))
                return "?";
            return ".";
        }

        public bool GetIsOnSide(GridPoint3D point, MovementDirection side)
        {
            // Note - because "up" increases Y, whereas the top is at 0 and
            // increasing y goes down,
            // we need to invert the meaning of Up and Down here
            return side switch
            {
                MovementDirection.Left => point.X == 0,
                MovementDirection.Right => point.X == Width - 1,
                MovementDirection.Down => point.Y == 0,
                MovementDirection.Up => point.Y == Height - 1,
                _ => throw new Exception($"Invalid side {side}"),
            };
        }

        public bool GetIsOuterPoint(GridPoint3D point)
        {
            if (point.X == 0)
                return true;
            if (point.Y == 0)
                return true;
            if (point.X == Width - 1)
                return true;
            if (point.Y == Height - 1)
                return true;
            return false;
        }

        public bool GetIsPointOffMap(GridPoint3D point)
        {
            if (point.X < 0)
                return true;
            if (point.Y < 0)
                return true;
            if (point.X >= Width)
                return true;
            if (point.Y >= Height)
                return true;
            return false;
        }

        public void DrawMapState()
        {
            GridHelper3D.DrawGrid3D(
                minX: 0,
                maxX: Width - 1,
                minY: 0,
                maxY: Height - 1,
                minZ: MinZ,
                maxZ: MaxZ,
                GetPointString: GetCellString);
        }

        public IList<Tuple<string, ConsoleColor>> GetMapRenderingData()
        {
            return GridHelper3D.GetGridRenderingData(
                minX: 0,
                maxX: Width - 1,
                minY: 0,
                maxY: Height - 1,
                minZ: MinZ,
                maxZ: MaxZ,
                GetPointString: GetCellString);
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
                ErisMapState s = (ErisMapState)obj;
                return GetHashCode() == s.GetHashCode();
            }
        }

        public override int GetHashCode()
        {
            var hash = Signature.GetHashCode();
            return hash;
        }

        public string GetSignature()
        {
            var builder = new StringBuilder();
            for (int z = MinZ; z <= MaxZ; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var point = new GridPoint3D(x, y, z);
                        if (BugCells.Contains(point))
                            builder.Append(point.ToString());
                    }
                }
            }
            return builder.ToString();
        }
    }
}
