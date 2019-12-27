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

        
    }
}
