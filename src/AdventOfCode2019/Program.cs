using AdventOfCode2019.Challenges;
using AdventOfCode2019.Challenges.Day3;
using AdventOfCode2019.Challenges.Day2;
using System;
using AdventOfCode2019.Challenges.Day4;
using AdventOfCode2019.Challenges.Day5;

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
            Console.WriteLine($"Day 5 - Part 1: {Day5.GetDay5Part1Answer()}");
        }
    }
}
