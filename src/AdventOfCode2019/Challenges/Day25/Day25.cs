using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day25
{
    /// <summary>
    /// Solution to the Day 25 challenge:
    /// https://adventofcode.com/2019/day/25
    /// </summary>
    public class Day25
    {
        public const string FILE_NAME = "Day25Input.txt";
        public static BigInteger GetDay25Part1Answer()
        {
            // Look around the ship and see if you can find the password for 
            // the main airlock.
            // Answer: 268468864
            var program = GetDay25Input();
            bool isAutomated = true;
            var shipMap = new ShipMap(program, isAutomated);
            Console.WriteLine("Exploring ship...");
            shipMap.Explore();
            shipMap.DrawMap();
            Console.WriteLine("Gathering items...");
            shipMap.GatherItems();
            shipMap.DrawMap();
            var itemCombo = shipMap.AttemptToFindWinningCombination(out RobotOutputResult robotOutputResult);
            Console.WriteLine($"Required items: {string.Join(", ", itemCombo)}");
            Console.WriteLine(robotOutputResult.FullText);
            var passwordPattern = @"Oh, hello! You should be able to get in by typing ([0-9]+) on the keypad at the main airlock.";
            var matchPassword = Regex.Match(robotOutputResult.FullText, passwordPattern);
            var result = BigInteger.Parse(matchPassword.Groups[1].Value);
            return result;
        }

        public static BigInteger[] GetDay25Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
