using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day4
{
    public static class Day4
    {
        public const int Day4Input_RangeMinValue = 108457;
        public const int Day4Input_RangeMaxValue = 562041;

        public static int GetDay4Part1Answer()
        {
            // https://adventofcode.com/2019/day/4
            // How many different passwords within the range given in your puzzle input meet these criteria?
            // Answer: 2779
            return GetNumberOfValidPasswordsInRangePart1(Day4Input_RangeMinValue, Day4Input_RangeMaxValue);
        }

        public static int GetDay4Part2Answer()
        {
            // https://adventofcode.com/2019/day/4#part2
            // How many different passwords within the range given in your puzzle input meet all of the criteria?
            // Answer: 1972
            return GetNumberOfValidPasswordsInRangePart2(Day4Input_RangeMinValue, Day4Input_RangeMaxValue);
        }

        public static int GetNumberOfValidPasswordsInRangePart1(int minValue, int maxValue)
        {
            int count = 0;
            for (int i = minValue; i <= maxValue; i++)
            {
                if (GetIsValidPart1(i))
                {
                    count++;
                }
            }
            return count;
        }

        public static int GetNumberOfValidPasswordsInRangePart2(int minValue, int maxValue)
        {
            int count = 0;
            for (int i = minValue; i <= maxValue; i++)
            {
                if (GetIsValidPart2(i))
                {
                    count++;
                }
            }
            return count;
        }

        public static bool GetIsValidPart1(int number)
        {
            var digits = GetDigits(number);
            // It is a six-digit number.
            // The value is within the range given in your puzzle input.
            // Two adjacent digits are the same(like 22 in 122345).
            // Going from left to right, the digits never decrease; 
            // they only ever increase or stay the same(like 111123 or 135679).
            if (digits.Length != 6)
                return false;
            bool hasTwoAdjacentSameDigits = false;
            int previousDigit = -1;
            for (int i = 0; i < digits.Length; i++)
            {
                int currentDigit = digits[i];
                if (currentDigit < previousDigit)
                    return false;
                if (currentDigit == previousDigit)
                    hasTwoAdjacentSameDigits = true;
                previousDigit = currentDigit;
            }
            return hasTwoAdjacentSameDigits;
        }

        public static bool GetIsValidPart2(int number)
        {
            var digits = GetDigits(number);
            // It is a six-digit number.
            // The value is within the range given in your puzzle input.
            // Two adjacent digits are the same(like 22 in 122345).
            //   The two adjacent matching digits are not part of a larger group of matching digits
            //   i.e., there has to be at least one *pair* of matching digits
            // Going from left to right, the digits never decrease; 
            // they only ever increase or stay the same(like 111123 or 135679).
            if (digits.Length != 6)
                return false;
            int previousDigit = -1;
            int sameDigitCount = 1;
            bool hasPairOfAdjacentSameDigits = false;
            for (int i = 0; i < digits.Length; i++)
            {
                int currentDigit = digits[i];
                if (currentDigit < previousDigit)
                    return false;
                if (currentDigit == previousDigit)
                {
                    sameDigitCount++;
                }
                else
                {
                    if (sameDigitCount == 2)
                    {
                        hasPairOfAdjacentSameDigits = true;
                    }
                    sameDigitCount = 1;
                }
                    
                previousDigit = currentDigit;
            }
            if (sameDigitCount == 2)
                hasPairOfAdjacentSameDigits = true;
            return hasPairOfAdjacentSameDigits;
        }

        /// <summary>
        /// Gets array of digits of a given input value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] GetDigits(int value)
        {
            // // https://stackoverflow.com/questions/829174/is-there-an-easy-way-to-turn-an-int-into-an-array-of-ints-of-each-digit/829181#829181
            var numbers = new Stack<int>();

            for (; value > 0; value /= 10)
                numbers.Push(value % 10);

            return numbers.ToArray();
        }
    }
}
