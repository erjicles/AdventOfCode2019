using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Solvers
{
    public static class SandwichSolver
    {
        /// <summary>
        /// Gets the largest integer greater than or equal to zero where the 
        /// provided <paramref name="GetTestResult"/> function returns true.
        /// This assumes that there is a largest integer such that
        /// <paramref name="GetTestResult"/> returns false for all greater integers
        /// and <paramref name="GetTestResult"/> returns true for all smaller
        /// integers.
        /// If this assumption is not true, then this function will perform
        /// unpredicably.
        /// </summary>
        /// <param name="GetTestResult"></param>
        /// <returns></returns>
        public static BigInteger GetLargestInteger(Func<BigInteger, bool> GetTestResult)
        {
            return GetLargestInteger(GetTestResult, -1);
        }

        /// <summary>
        /// Gets the largest integer greater than
        /// <paramref name="minTrueValue"/> where the provided 
        /// <paramref name="GetTestResult"/> function returns true.
        /// This assumes that there is a smallest integer such that
        /// <paramref name="GetTestResult"/> returns false for all greater integers
        /// and <paramref name="GetTestResult"/> returns true for all smaller
        /// integers.
        /// If this assumption is not true, then this function will perform
        /// unpredicably.
        /// </summary>
        /// <param name="GetIsValid"></param>
        /// <returns></returns>
        public static BigInteger GetLargestInteger(
            Func<BigInteger, bool> GetTestResult,
            BigInteger minTrueValue)
        {
            bool GetNegativeTestResult(BigInteger guess)
            {
                return !GetTestResult(guess);
            }
            var result = GetSmallestInteger(GetNegativeTestResult, minTrueValue);
            return result - 1;
        }

        /// <summary>
        /// Gets the smallest integer greater than or equal to zero where the 
        /// provided <paramref name="GetTestResult"/> function returns true.
        /// This assumes that there is a smallest integer such that
        /// <paramref name="GetTestResult"/> returns true for all greater integers
        /// and <paramref name="GetTestResult"/> returns false for all smaller
        /// integers.
        /// If this assumption is not true, then this function will perform
        /// unpredicably.
        /// </summary>
        /// <param name="GetTestResult"></param>
        /// <returns></returns>
        public static BigInteger GetSmallestInteger(Func<BigInteger, bool> GetTestResult)
        {
            return GetSmallestInteger(GetTestResult, -1);
        }

        /// <summary>
        /// Gets the smallest integer greater than 
        /// <paramref name="minInvalidValue"/> where the provided 
        /// <paramref name="GetTestResult"/> function returns true.
        /// This assumes that there is a smallest integer such that
        /// <paramref name="GetTestResult"/> returns true for all greater integers
        /// and <paramref name="GetTestResult"/> returns false for all smaller
        /// integers.
        /// If this assumption is not true, then this function will perform
        /// unpredicably.
        /// </summary>
        /// <param name="GetIsValid"></param>
        /// <returns></returns>
        public static BigInteger GetSmallestInteger(
            Func<BigInteger, bool> GetTestResult,
            BigInteger minFalseValue)
        {
            // Keep trying output values until it generates the highest
            // required input quantity without going over the provided amount.
            BigInteger maxInvalidValue = minFalseValue;
            BigInteger minValidValue = -1;
            bool hasFoundSmallestValidValue = false;
            BigInteger increment;
            BigInteger incrementFactor = 3;

            // Determine the initial increment
            // Keep increasing the increment until a valid value is found
            // Set the increment to the next smallest value that didn't yield
            // a valid value
            for (increment = 1; ; increment *= incrementFactor)
            {
                var currentGuess = maxInvalidValue + increment;
                var isValid = GetTestResult(currentGuess);
                if (isValid)
                {
                    minValidValue = currentGuess;
                    increment = increment / incrementFactor;
                    if (increment <= 0)
                        increment = 1;
                    break;
                }
            }

            // Each time an invalid value is found, set the new max invalid 
            // value and decrease the increment
            // Keep doing this until the max invalid value found is one less
            // than the min valid value found.
            while (!hasFoundSmallestValidValue)
            {
                var currentGuess = maxInvalidValue + increment;
                var isValid = GetTestResult(currentGuess);
                if (!isValid)
                {
                    maxInvalidValue = currentGuess;
                }
                else
                {
                    minValidValue = currentGuess;
                    increment /= incrementFactor;
                    if (increment <= 0)
                        increment = 1;
                }
                if (minValidValue - maxInvalidValue < 2)
                    hasFoundSmallestValidValue = true;
            }
            return minValidValue;
        }
    }
}
