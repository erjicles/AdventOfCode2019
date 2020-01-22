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
        public Dictionary<GridPoint3D, CellType> GridCells { get; private set; }
        public bool IsRecursive { get; private set; }
        public GridPoint CenterPointXY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MinZ { get; private set; }
        public int MaxZ { get; private set; }
        private string _signature;
        public ErisMapState(
            Dictionary<GridPoint3D, CellType> gridCells,
            int width,
            int height,
            int minZ,
            int maxZ,
            bool isRecursive)
        {
            GridCells = gridCells.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            IsRecursive = isRecursive;
            Width = width;
            Height = height;
            MinZ = minZ;
            MaxZ = maxZ;
            CenterPointXY = new GridPoint((Width - 1)/2, (Height - 1)/2);
            _signature = string.Join("", GetMapRenderingData()
                .Select(t => t.Item1));
        }

        public static ErisMapState CreateMap(
            IList<string> mapDefinition,
            bool isRecursive)
        {
            int height = mapDefinition.Count;
            int width = mapDefinition.Max(row => row.Length);
            var gridCells = new Dictionary<GridPoint3D, CellType>();
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
                    gridCells.Add(point, cellType);
                }
            }
            // If it's recursive, then the middle point is the recursive cell
            // containing the nested grids
            if (isRecursive)
            {
               var middlePoint = new GridPoint3D(
                   x: (width - 1) / 2, 
                   y: (height - 1) / 2, 
                   z: 0);
                gridCells[middlePoint] = CellType.NestedGrid;
            }
            var result = new ErisMapState(
                gridCells: gridCells, 
                width: width, 
                height: height, 
                minZ: 0,
                maxZ: 0,
                isRecursive: isRecursive);
            return result;
        }

        public BigInteger GetTotalNumberOfBugs()
        {
            var numberOfBugs = GridCells
                .Where(kvp => CellType.Bug.Equals(kvp.Value))
                .Count();
            return numberOfBugs;
        }

        public BigInteger GetBiodiversityRating(int layer)
        {
            int minX = 0;
            int maxX = GridCells.Max(kvp => kvp.Key.X);
            int minY = 0;
            int maxY = GridCells.Max(kvp => kvp.Key.Y);
            BigInteger result = 0;
            BigInteger cellPointValue = 1;
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new GridPoint3D(x, y, layer);
                    if (GridCells.ContainsKey(point))
                    {
                        var cellType = GridCells[point];
                        if (CellType.Bug.Equals(cellType))
                        {
                            result += cellPointValue;
                        }
                    }
                    cellPointValue *= 2;
                }
            }
            return result;
        }

        public ErisMapState Evolve()
        {
            var gridCells = new Dictionary<GridPoint3D, CellType>();
            var cellStatuses = new Dictionary<GridPoint3D, CellStatus>();

            // Add new layers as needed
            ExpandMap();

            foreach (var kvp in GridCells)
            {
                var point = kvp.Key;
                var cellStatus = GetCellStatus(point);
                cellStatuses.Add(point, cellStatus);
            }
            foreach (var kvp in cellStatuses)
            {
                var point = kvp.Key;
                var currentType = GridCells[point];
                gridCells.Add(point, currentType);
                // Each minute, The bugs live and die based on the number of 
                // bugs in the four adjacent tiles:
                // A bug dies(becoming an empty space) unless there is exactly 
                // one bug adjacent to it.
                //An empty space becomes infested with a bug if exactly one or 
                // two bugs are adjacent to it.
                if (CellType.Empty.Equals(currentType)
                    && (kvp.Value.NumberOfBugNeighbors ==  1
                        || kvp.Value.NumberOfBugNeighbors == 2))
                {
                    gridCells[point] = CellType.Bug;
                }
                else if (CellType.Bug.Equals(currentType)
                    && kvp.Value.NumberOfBugNeighbors != 1)
                {
                    gridCells[point] = CellType.Empty;
                }
            }
            return new ErisMapState(
                gridCells: gridCells, 
                width: Width, 
                height: Height, 
                minZ: MinZ,
                maxZ: MaxZ,
                isRecursive: IsRecursive);
        }

        private void ExpandMap()
        {
            // Add new layers if needed
            // Add a new inner layer *if* the innermost layer's middle cell
            // has a neighbor with a bug
            var innermostMiddlePoint = new GridPoint3D(CenterPointXY, MaxZ);
            var innermostMiddlePointStatus = GetCellStatus(innermostMiddlePoint);
            if (innermostMiddlePointStatus.NumberOfBugNeighbors > 0)
            {
                AddLayer(MaxZ + 1);
            }

            // Add a new outer layer *if* the outermost layer has any outer
            // cells with bugs
            int outermostLayerOutermostCellsBugCount = GridCells
                .Where(kvp => kvp.Key.Z == MinZ
                    && CellType.Bug.Equals(kvp.Value)
                    && GetIsOuterPoint(kvp.Key))
                .Count();
            if (outermostLayerOutermostCellsBugCount > 0)
            {
                AddLayer(MinZ - 1);
            }
        }

        private void AddLayer(int z)
        {
            if (z <= MaxZ && z >= MinZ)
                throw new Exception($"Layer already exists: {z}");
            if (z > MaxZ)
                MaxZ = z;
            else
                MinZ = z;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var point = new GridPoint3D(x, y, z);
                    GridCells.Add(point, CellType.Empty);
                    if (IsRecursive &&
                        point.XYPoint.Equals(CenterPointXY))
                    {
                        GridCells[point] = CellType.NestedGrid;
                    }
                }
            }
        }

        public CellStatus GetCellStatus(GridPoint3D point)
        {
            var neighbors = GetNeighbors(point);
            int numberOfEmptyNeighbors = 0;
            int numberOfBugNeighbors = 0;
            foreach (var neighbor in neighbors)
            {
                if (!GridCells.ContainsKey(neighbor))
                {
                    numberOfEmptyNeighbors++;
                    continue;
                }
                if (CellType.Empty.Equals(GridCells[neighbor]))
                {
                    numberOfEmptyNeighbors++;
                }
                else if (CellType.Bug.Equals(GridCells[neighbor]))
                {
                    numberOfBugNeighbors++;
                }
                else
                {
                    throw new Exception($"Invalid cell type {GridCells[neighbor]}");
                }
            }
            var result = new CellStatus(numberOfEmptyNeighbors, numberOfBugNeighbors);
            return result;
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
                        var oppositeDirection = MovementDirectionHelper.GetOppositeDirection(direction);
                        var innerLayerOuterCells = GridCells
                            .Where(kvp => kvp.Key.Z == point.Z + 1)
                            .Where(kvp => GetIsOnSide(kvp.Key, oppositeDirection))
                            .Select(kvp => kvp.Key)
                            .ToList();
                        result.AddRange(innerLayerOuterCells);
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
            if (!GridCells.ContainsKey(point))
                return " ";
            return (GridCells[point]) switch
            {
                CellType.Empty => ".",
                CellType.Bug => "#",
                CellType.NestedGrid => "?",
                _ => " ",
            };
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
                gridPoints: GridCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString);
        }

        public IList<Tuple<string, ConsoleColor>> GetMapRenderingData()
        {
            return GridHelper3D.GetGridRenderingData(
                gridPoints: GridCells.Select(kvp => kvp.Key).ToList(),
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
            var hash = _signature.GetHashCode();
            return hash;
        }
    }
}
