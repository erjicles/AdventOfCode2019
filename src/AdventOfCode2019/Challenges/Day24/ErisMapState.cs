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
        public Dictionary<GridPoint, CellType> GridCells { get; private set; }
        private string _signature;
        public ErisMapState(Dictionary<GridPoint, CellType> gridCells)
        {
            GridCells = gridCells.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _signature = string.Join("", GetMapRenderingData()
                .Select(t => t.Item1));
        }

        public static ErisMapState CreateMap(IList<string> mapDefinition)
        {
            var gridCells = new Dictionary<GridPoint, CellType>();
            for (int y = 0; y < mapDefinition.Count; y++)
            {
                var rowDefinition = mapDefinition[y];
                for (int x = 0; x < rowDefinition.Length; x++)
                {
                    var cellDefinition = rowDefinition[x];
                    var point = new GridPoint(x, y);
                    CellType cellType = cellDefinition switch
                    {
                        '.' => CellType.Empty,
                        '#' => CellType.Bug,
                        _ => CellType.Empty,
                    };
                    gridCells.Add(point, cellType);
                }
            }
            var result = new ErisMapState(gridCells);
            return result;
        }

        public BigInteger GetBiodiversityRating()
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
                    var point = new GridPoint(x, y);
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
            var gridCells = new Dictionary<GridPoint, CellType>();
            var cellStatuses = new Dictionary<GridPoint, CellStatus>();
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
            return new ErisMapState(gridCells);
        }

        public CellStatus GetCellStatus(GridPoint point)
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

        public static IList<GridPoint> GetNeighbors(GridPoint point)
        {
            return new List<GridPoint>()
            {
                point.MoveLeft(1),
                point.MoveUp(1),
                point.MoveRight(1),
                point.MoveDown(1)
            };
        }

        public string GetCellString(GridPoint point)
        {
            if (!GridCells.ContainsKey(point))
                return " ";
            return (GridCells[point]) switch
            {
                CellType.Empty => ".",
                CellType.Bug => "#",
                _ => " ",
            };
        }

        public void DrawMapState()
        {
            GridHelper.DrawGrid2D(
                gridPoints: GridCells.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetCellString);
        }

        public IList<Tuple<string, ConsoleColor>> GetMapRenderingData()
        {
            return GridHelper.GetGridRenderingData(
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
