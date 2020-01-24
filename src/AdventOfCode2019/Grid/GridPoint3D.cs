using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public class GridPoint3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public GridPoint XYPoint { get { return new GridPoint(X, Y); } }
        public static GridPoint3D Origin = new GridPoint3D(0, 0, 0);

        public GridPoint3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public GridPoint3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public GridPoint3D(GridPoint point2D)
        {
            X = point2D.X;
            Y = point2D.Y;
            Z = 0;
        }

        public GridPoint3D(GridPoint point2D, int z)
        {
            X = point2D.X;
            Y = point2D.Y;
            Z = z;
        }

        public GridPoint3D Move(MovementDirection direction, int d)
        {
            return direction switch
            {
                MovementDirection.Down => MoveDown(d),
                MovementDirection.Left => MoveLeft(d),
                MovementDirection.Right => MoveRight(d),
                MovementDirection.Up => MoveUp(d),
                MovementDirection.In => MoveIn(d),
                MovementDirection.Out => MoveOut(d),
                _ => throw new Exception($"Invalid movement direction {direction}"),
            };
        }

        public GridPoint3D Move(GridPoint3D displacementVector)
        {
            return Move(Tuple.Create(
                displacementVector.X,
                displacementVector.Y,
                displacementVector.Z));
        }

        public GridPoint3D Move(Tuple<int, int, int> displacementVector)
        {
            return new GridPoint3D(
                X + displacementVector.Item1,
                Y + displacementVector.Item2,
                Z + displacementVector.Item3);
        }

        public GridPoint3D MoveRight(int d)
        {
            return new GridPoint3D
            {
                X = X + d,
                Y = Y,
                Z = Z
            };
        }
        public GridPoint3D MoveLeft(int d)
        {
            return new GridPoint3D
            {
                X = X - d,
                Y = Y,
                Z = Z
            };
        }
        public GridPoint3D MoveUp(int d)
        {
            return new GridPoint3D
            {
                X = X,
                Y = Y + d,
                Z = Z
            };
        }
        public GridPoint3D MoveDown(int d)
        {
            return new GridPoint3D
            {
                X = X,
                Y = Y - d,
                Z = Z
            };
        }

        public GridPoint3D MoveIn(int d)
        {
            return new GridPoint3D
            {
                X = X,
                Y = Y,
                Z = Z - d
            };
        }

        public GridPoint3D MoveOut(int d)
        {
            return new GridPoint3D
            {
                X = X,
                Y = Y,
                Z = Z + d
            };
        }

        public static bool GetAreAdjacent(GridPoint3D p1, GridPoint3D p2)
        {
            return GetManhattanDistance(p1, p2) == 1;
        }

        /// <summary>
        /// Computes the manhattan distance between two points on the grid.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int GetManhattanDistance(GridPoint3D p1, GridPoint3D p2)
        {
            return Math.Abs(p1.X - p2.X)
                + Math.Abs(p1.Y - p2.Y)
                + Math.Abs(p1.Z - p2.Z);
        }

        // Equals, GetHashCode, and ToString() adapted from Microsoft example here:
        // https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netcore-3.1
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GridPoint3D p = (GridPoint3D)obj;
                return (X == p.X) && (Y == p.Y) && (Z == p.Z);
            }
        }

        public override int GetHashCode()
        {
            var tuple = Tuple.Create(X, Y, Z);
            int hash = tuple.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("GridPoint3D({0}, {1}, {2})", X, Y, Z);
        }
    }
}
