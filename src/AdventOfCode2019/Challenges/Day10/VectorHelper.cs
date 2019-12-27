using Radicals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day10
{
    public static class VectorHelper
    {
        public static Radical GetVectorLength(Tuple<int, int> v)
        {
            return GetVectorLength(new Tuple<Radical, Radical>(v.Item1, v.Item2));
        }

        public static Radical GetVectorLength(Tuple<Radical, Radical> v)
        {
            var sq1 = v.Item1 * v.Item1;
            var sq2 = v.Item2 * v.Item2;
            if (!sq1.IsRational)
                throw new Exception("Encountered squared radical taht's not rational");
            if (!sq2.IsRational)
                throw new Exception("Encountered squared radical taht's not rational");
            var rationalSq1 = sq1.ToRational();
            var rationalSq2 = sq2.ToRational();
            var length = Radical.Sqrt(rationalSq1 + rationalSq2);
            return length;
        }

        public static Tuple<Radical, Radical> GetUnitVector(Tuple<int, int> v)
        {
            return GetUnitVector(new Tuple<Radical, Radical>(v.Item1, v.Item2));
        }

        public static Tuple<Radical, Radical> GetUnitVector(Tuple<Radical, Radical> v)
        {
            var lengthV = GetVectorLength(v);
            var unitVector = new Tuple<Radical, Radical>(
                v.Item1 / lengthV,
                v.Item2 / lengthV);
            return unitVector;
        }

        public static RadicalSum GetInnerProduct(
            Tuple<Radical, Radical> v1,
            Tuple<Radical, Radical> v2)
        {
            var innerProduct = (v1.Item1 * v2.Item1)
                + (v1.Item2 * v2.Item2);
            return innerProduct;
        }

        /// <summary>
        /// Takes two tuples (interpreted as vectors) and returns true if the
        /// vectors are parallel and pointing in the same direction.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool GetAreParallel(
            Tuple<int, int> v1,
            Tuple<int, int> v2)
        {

            return GetAreParallelAndUnidirectional(
                new Tuple<Radical, Radical>(v1.Item1, v1.Item2),
                new Tuple<Radical, Radical>(v2.Item1, v2.Item2));
        }

        /// <summary>
        /// Takes two tuples (interpreted as vectors) and returns true if the
        /// vectors are parallel and pointing in the same direction.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool GetAreParallelAndUnidirectional(
            Tuple<Radical, Radical> v1,
            Tuple<Radical, Radical> v2)
        {
            var unitVector1 = GetUnitVector(v1);
            var unitVector2 = GetUnitVector(v2);
            var innerProduct = GetInnerProduct(unitVector1, unitVector2);
            if (innerProduct.IsOne)
                return true;
            return false;
        }

        /// <summary>
        /// Get all vectors such that |x| + |y| = <paramref name="n"/>
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IList<Tuple<int, int>> GetJumpVectors(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            var result = new List<Tuple<int, int>>();
            for (int x = -n; x <= n; x++)
            {
                int y = n - Math.Abs(x);
                result.Add(new Tuple<int, int>(x, y));
                int negativeY = -y;
                if (negativeY != y)
                    result.Add(new Tuple<int, int>(x, negativeY));
            }
            return result;
        }

        public static Tuple<int, int> MultiplyVector(Tuple<int, int> v, int scalar)
        {
            return new Tuple<int, int>(v.Item1 * scalar, v.Item2 * scalar);
        }

        /// <summary>
        /// Given a <paramref name="center"/> point and a 
        /// <paramref name="jumpVector"/>, returns the point reached by moving
        /// from the <paramref name="center"/> along 
        /// <paramref name="jumpStep"/> multiples of the 
        /// <paramref name="jumpVector"/>.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="jumpVector"></param>
        /// <param name="jumpNumber"></param>
        /// <returns></returns>
        public static SolarGridPoint GetJumpPoint(
            SolarGridPoint center, 
            Tuple<int, int> jumpVector, 
            int jumpStep)
        {
            var displacement = MultiplyVector(jumpVector, jumpStep);
            var resultPoint = SolarGridPoint.GetPointAtRayVector(
                center, displacement);
            return resultPoint;
        }
    }
}
