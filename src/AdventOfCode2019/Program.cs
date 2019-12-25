using AdventOfCode2019.Challenges;
using AdventOfCode2019.Challenges.Day3;
using AdventOfCode2019.Challenges.Day2;
using System;
using AdventOfCode2019.Challenges.Day4;
using AdventOfCode2019.Challenges.Day5;
using AdventOfCode2019.Challenges.Day6;
using AdventOfCode2019.Challenges.Day7;
using AdventOfCode2019.Challenges.Day8;
using AdventOfCode2019.Challenges.Day9;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---Welcome to erjicle's solutions to Advent of Code 2019!!---");
            // Day 1
            Console.WriteLine($"Day 1 - Part 1: {Day1.GetDay1Part1Answer()}");
            Console.WriteLine($"Day 1 - Part 2: {Day1.GetDay1Part2Answer()}");
            // Day 2
            Console.WriteLine($"Day 2 - Part 1: {Day2.GetDay2Part1Answer()}");
            Console.WriteLine($"Day 2 - Part 2: {Day2.GetDay2Part2Answer()}");
            // Day 3
            Console.WriteLine($"Day 3 - Part 1: {Day3.GetDay3Part1Answer()}");
            Console.WriteLine($"Day 3 - Part 2: {Day3.GetDay3Part2Answer()}");
            // Day 4
            Console.WriteLine($"Day 4 - Part 1: {Day4.GetDay4Part1Answer()}");
            Console.WriteLine($"Day 4 - Part 2: {Day4.GetDay4Part2Answer()}");
            // Day 5
            Console.WriteLine("Day 5 - Part 1:");
            Day5.RunDay5Part1();
            Console.WriteLine("Day 5 - Part 2:");
            Day5.RunDay5Part2();
            // Day 6
            Console.WriteLine($"Day 6 - Part 1: {Day6.GetDay6Part1Answer()}");
            Console.WriteLine($"Day 6 - Part 2: {Day6.GetDay6Part2Answer()}");
            // Day 7
            Console.WriteLine($"Day 7 - Part 1: {Day7.GetDay7Part1Answer()}");
            Console.WriteLine($"Day 7 - Part 2: {Day7.GetDay7Part2Answer()}");
            // Day 8
            Console.WriteLine($"Day 8 - Part 1: {Day8.GetDay8Part1Answer()}");
            Console.WriteLine($"Day 8 - Part 2:");
            Day8.RunDay8Part2();
            // Day 9
            Console.WriteLine($"Day 9 - Part 1: {Day9.GetDay9Part1Answer()}");
            Console.WriteLine($"Day 9 - Part 2: {Day9.GetDay9Part2Answer()}");
        }
    }
}
