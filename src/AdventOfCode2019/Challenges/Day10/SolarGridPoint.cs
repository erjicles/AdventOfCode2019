using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day10
{
    public class SolarGridPoint
    {
        private Tuple<int, int> _coordinates;
        public int X 
        {
            get
            {
                return _coordinates.Item1;
            }
        }
        public int Y
        {
            get
            {
                return _coordinates.Item2;
            }
        }
        public SolarGridPoint(int x, int y)
        {
            _coordinates = new Tuple<int, int>(x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(SolarGridPoint))
                return false;
            var otherPoint = (SolarGridPoint)obj;
            return X == otherPoint.X
                && Y == otherPoint.Y;
        }

        public override int GetHashCode()
        {
            return _coordinates.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        /// <summary>
        /// Get the point lying at the end of the <paramref name="rayVector"/> 
        /// extending from <paramref name="centralPoint"/>.
        /// </summary>
        /// <param name="rayVector"></param>
        /// <returns></returns>
        public static SolarGridPoint GetPointAtRayVector(
            SolarGridPoint centralPoint, 
            Vector<int> rayVector)
        {
            return new SolarGridPoint(
                centralPoint.X + rayVector[0], 
                centralPoint.Y + rayVector[1]);
        }

        public static Tuple<int, int> GetDifferenceVector(
            SolarGridPoint pFrom, 
            SolarGridPoint pTo)
        {
            return new Tuple<int, int>(
                pTo.X - pFrom.X,
                pTo.Y - pFrom.Y);
        }
    }
}
