using AdventOfCode2019.Solvers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.MathHelpers
{
    /// <summary>
    /// Represents a linear congruential generator of the form
    /// f(x) = a*x + b (mod m)
    /// </summary>
    public class LCG
    {
        // https://en.wikipedia.org/wiki/Linear_congruential_generator
        public BigInteger A { get; private set; }
        public BigInteger B { get; private set; }
        public BigInteger M { get; private set; }

        /// <summary>
        /// Creates a new linear congruential generator of the form
        /// f(x) = a*x + b (mod m)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="m"></param>
        public LCG(BigInteger a, BigInteger b, BigInteger m)
        {
            if (m <= 0)
                throw new ArgumentException("m must be a positive integer");
            A = ModHelper.GetPositiveMod(a, m);
            B = ModHelper.GetPositiveMod(b, m);
            M = m;
        }

        /// <summary>
        /// Returns the composition of two linear congruential functions:
        /// g(f(x)). Assumes m is the same.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static LCG Compose(LCG f, LCG g)
        {
            if (f.M != g.M)
                throw new ArgumentException("m is not the same");
            // g(f(x)) = g(ax + b) = c(ax + b) + d = (ca)x + (cb + d) (mod m)
            return new LCG(f.A * g.A, (f.B * g.A) + g.B, f.M);
        }

        public LCG GetInverse()
        {
            // f(x) = A*x + b (mod m)
            // f^(-1)(x) = 
            var isInvertible = ModHelper.TryGetModInverse(A, M, out BigInteger aInverse);
            if (!isInvertible)
                throw new Exception($"{A} is not invertible mod {M}");
            var result = new LCG(aInverse, -1*B* aInverse, M);
            return result;
        }

        public static LCG GetIdentity(BigInteger m)
        {
            return new LCG(1, 0, m);
        }

        public BigInteger Evaluate(BigInteger x)
        {
            var aPart = A * x;
            var sumPart = aPart + B;
            var result = ModHelper.GetPositiveMod(sumPart, M);
            return result;
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
                LCG other = (LCG)obj;
                return (A == other.A)
                    && (B == other.B)
                    && (M == other.M);
            }
        }

        public override int GetHashCode()
        {
            var tuple = Tuple.Create(A, B, M);
            int hash = tuple.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"[{A}*x + {B} (mod {M})]";
        }
    }
}
