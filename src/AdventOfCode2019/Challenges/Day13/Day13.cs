using AdventOfCode2019.Grid;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day13
{
    /// <summary>
    /// Solution to the Day 13 challenge:
    /// https://adventofcode.com/2019/day/13
    /// </summary>
    public class Day13
    {
        public const string FILE_NAME = "Day13Input.txt";

        public static int GetDay13Part1Answer()
        {
            // Start the game. How many block tiles are on the screen when the 
            // game exits?
            // Answer: 265
            var input = GetDay13Input();
            var game = new Game(input)
            {
                Mode = GameMode.Automated,
                DrawBoard = false,
                ClearConsoleWhenDrawingBoard = false,
            };
            game.RunGame();
            game.DrawGameBoard();
            int numberOfBlocks = game.NumberOfBlocksRemaining;
            return numberOfBlocks;
        }

        public static long GetDay13Part2Answer()
        {
            // Beat the game by breaking all the blocks. What is your score 
            // after the last block is broken?
            // Answer: 13331
            var input = GetDay13Input();
            var game = new Game(input, 2)
            {
                Mode = GameMode.Automated,
                DrawBoard = false,
                ClearConsoleWhenDrawingBoard = false,
            };
            game.RunGame();
            game.DrawGameBoard();
            long finalScore = game.Score;
            return finalScore;
        }

        public static BigInteger[] GetDay13Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
