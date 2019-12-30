using AdventOfCode2019.Challenges;
using AdventOfCode2019.Challenges.Day03;
using AdventOfCode2019.Challenges.Day02;
using System;
using AdventOfCode2019.Challenges.Day04;
using AdventOfCode2019.Challenges.Day05;
using AdventOfCode2019.Challenges.Day06;
using AdventOfCode2019.Challenges.Day07;
using AdventOfCode2019.Challenges.Day08;
using AdventOfCode2019.Challenges.Day09;
using AdventOfCode2019.Challenges.Day01;
using AdventOfCode2019.Challenges.Day10;
using AdventOfCode2019.Challenges.Day11;
using AdventOfCode2019.Challenges.Day12;
using AdventOfCode2019.Challenges.Day13;
using AdventOfCode2019.Challenges.Day14;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---Welcome to erjicle's solutions to Advent of Code 2019!!---");
            // Day 1
            Console.WriteLine($"Day 1 - Part 1: {Day01.GetDay1Part1Answer()}");
            Console.WriteLine($"Day 1 - Part 2: {Day01.GetDay1Part2Answer()}");
            // Day 2
            Console.WriteLine($"Day 2 - Part 1: {Day02.GetDay2Part1Answer()}");
            Console.WriteLine($"Day 2 - Part 2: {Day02.GetDay2Part2Answer()}");
            // Day 3
            Console.WriteLine($"Day 3 - Part 1: {Day03.GetDay3Part1Answer()}");
            Console.WriteLine($"Day 3 - Part 2: {Day03.GetDay3Part2Answer()}");
            // Day 4
            Console.WriteLine($"Day 4 - Part 1: {Day04.GetDay4Part1Answer()}");
            Console.WriteLine($"Day 4 - Part 2: {Day04.GetDay4Part2Answer()}");
            // Day 5
            Console.WriteLine("Day 5 - Part 1:");
            Day05.RunDay5Part1();
            Console.WriteLine("Day 5 - Part 2:");
            Day05.RunDay5Part2();
            // Day 6
            Console.WriteLine($"Day 6 - Part 1: {Day06.GetDay6Part1Answer()}");
            Console.WriteLine($"Day 6 - Part 2: {Day06.GetDay6Part2Answer()}");
            // Day 7
            Console.WriteLine($"Day 7 - Part 1: {Day07.GetDay7Part1Answer()}");
            Console.WriteLine($"Day 7 - Part 2: {Day07.GetDay7Part2Answer()}");
            // Day 8
            Console.WriteLine($"Day 8 - Part 1: {Day08.GetDay8Part1Answer()}");
            Console.WriteLine($"Day 8 - Part 2:");
            Day08.RunDay8Part2();
            // Day 9
            Console.WriteLine($"Day 9 - Part 1: {Day09.GetDay9Part1Answer()}");
            Console.WriteLine($"Day 9 - Part 2: {Day09.GetDay9Part2Answer()}");
            // Day 10
            Console.WriteLine($"Day 10 - Part 1: {Day10.GetDay10Part1Answer()}");
            Console.WriteLine($"Day 10 - Part 2: {Day10.GetDay10Part2Answer()}");
            // Day 11
            Console.WriteLine($"Day 11 - Part 1: {Day11.GetDay11Part1Answer()}");
            Console.WriteLine($"Day 11 - Part 2: {Day11.RunDay11Part2()}");
            // Day 12
            Console.WriteLine($"Day 12 - Part 1: {Day12.GetDay12Part1Answer()}");
            Console.WriteLine($"Day 12 - Part 2: {Day12.GetDay12Part2Answer()}");
            // Day 13
            Console.WriteLine($"Day 13 - Part 1: {Day13.GetDay13Part1Answer()}");
            Console.WriteLine($"Day 13 - Part 2: {Day13.GetDay13Part2Answer()}");
            // Day 14
            Console.WriteLine($"Day 14 - Part 1: {Day14.GetDay14Part1Answer()}");
            Console.WriteLine($"Day 14 - Part 2: {Day14.GetDay14Part2Answer()}");
        }
    }
}
