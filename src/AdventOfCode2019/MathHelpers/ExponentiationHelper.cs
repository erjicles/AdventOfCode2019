using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.MathHelpers
{
    public static class ExponentiationHelper
    {
        /// <summary>
        /// Returns <paramref name="x"/>^<paramref name="exponent"/>, where
        /// for some values A and B of type <typeparamref name="T"/>,
        /// <paramref name="MultiplyFunction"/>(A, B) returns A*B for some
        /// multiplication operator *.
        /// Assumption: * is associative, i.e., (A*B)*C = A*(B*C)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="exponent"></param>
        /// <param name="SquareFunction"></param>
        /// <param name="identity">Element I of type <typeparamref name="T"/>
        /// such that for any other element A of type <typeparamref name="T"/>,
        /// A * I = A</param>
        /// <returns></returns>
        public static T GetExponentiationByPowers<T>(
            T x, 
            BigInteger exponent, 
            Func<T, T, T> MultiplyFunction,
            T identity)
        {
            if (exponent < 0)
                throw new ArgumentException($"{nameof(exponent)} must be non-negative");
            if (exponent == 0)
                return identity;

            var powers = new Dictionary<BigInteger, T>();
            var exponentStack = new Stack<BigInteger>();

            // Calculate all powers of 2 less than or equal to the given
            // exponent
            BigInteger currentExponent = 1;
            T currentPower = x;
            while (currentExponent <= exponent)
            {
                // Cache the current power
                powers.Add(currentExponent, currentPower);
                exponentStack.Push(currentExponent);

                BigInteger nextExponent = currentExponent * 2;
                if (nextExponent > exponent)
                    break;
                currentExponent = nextExponent;
                currentPower = MultiplyFunction(currentPower, currentPower);
            }

            T result = identity;
            BigInteger exponentAchieved = 0;
            currentExponent = exponentStack.Pop();
            while (exponentAchieved < exponent)
            {
                var nextExponentAchieved = exponentAchieved + currentExponent;
                if (nextExponentAchieved > exponent)
                {
                    if (exponentStack.Count == 0)
                        throw new Exception("No exponents left in stack");
                    currentExponent = exponentStack.Pop();
                    continue;
                }
                result = MultiplyFunction(result, powers[currentExponent]);
                exponentAchieved = nextExponentAchieved;
            }
            return result;
        }
    }
}
