using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.MathHelpers
{
    public static class ModHelper
    {
        /// <summary>
        /// Calculates the multiplicative inverse of <paramref name="a"/>
        /// modulo <paramref name="m"/>. 
        /// Returns <seealso cref="true"/> if <paramref name="a"/> is
        /// invertible modulo <paramref name="m"/>, and assigns the inverse of
        /// <paramref name="a"/> to the output parameter 
        /// <paramref name="aInverse"/>.
        /// Returns <seealso cref="false"/> if <paramref name="a"/> is not
        /// invertible modulo <paramref name="m"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool TryGetModInverse(
            BigInteger a, 
            BigInteger m,
            out BigInteger aInverse)
        {
            a = GetPositiveMod(a, m);
            // Utilizes extended Euclidean algorithm
            // Calculates a^(-1) such that a * a^(-1) = 1 (mod m)
            // https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
            // https://stackoverflow.com/questions/51327337/how-to-perform-modinverse-in-c-sharp
            BigInteger gcd = BigIntegerHelper.GetExtendedGCD(a, m, out aInverse, out BigInteger bezoutCoefficientM);
            if (gcd != 1)
            {
                aInverse = 0;
                return false;
            }

            if (aInverse < 0)
                aInverse += m;
            aInverse %= m;
            return true;
        }

        /// <summary>
        /// Returns the positive residual a (mod m)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static BigInteger GetPositiveMod(BigInteger a, BigInteger m)
        {
            var result = a % m;
            if (result < 0)
                result += m;
            return result;
        }
    }
}
