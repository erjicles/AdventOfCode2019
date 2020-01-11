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

        public GridPoint3D Move(GridPoint3D displacementVector)
        {
            return Move(new Tuple<int, int, int>(
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
            var tuple = new Tuple<int, int, int>(X, Y, Z);
            int hash = tuple.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("GridPoint3D({0}, {1}, {2})", X, Y, Z);
        }
    }
}
