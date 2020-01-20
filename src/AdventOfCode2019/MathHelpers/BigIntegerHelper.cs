using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.MathHelpers
{
    public static class BigIntegerHelper
    {
        /// <summary>
        /// Returns the GCD of two integers using the extended Euclidean
        /// algorithm. Also provides the Bezout coefficients as as output 
        /// parameters. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="aFactor"></param>
        /// <param name="bFactor"></param>
        /// <returns></returns>
        public static BigInteger GetExtendedGCD(
            BigInteger a, 
            BigInteger b, 
            out BigInteger bezoutCoefficientA, 
            out BigInteger bezoutCoefficientB)
        {
            // Algorithm found here:
            // https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
            bezoutCoefficientA = 0;
            bezoutCoefficientB = 1;
            BigInteger u = 1;
            BigInteger v = 0;
            BigInteger gcd = 0;

            while (a != 0)
            {
                BigInteger q = b / a;
                BigInteger r = b % a;

                BigInteger m = bezoutCoefficientA - u * q;
                BigInteger n = bezoutCoefficientB - v * q;

                b = a;
                a = r;
                bezoutCoefficientA = u;
                bezoutCoefficientB = v;
                u = m;
                v = n;

                gcd = b;
            }

            return BigInteger.Abs(gcd);
        }
    }
}
