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
            var gameState = RunGame(input);
            DrawGameBoard(gameState);
            int numberOfBlocks = gameState
                .Where(kvp => GameCellType.Block.Equals(kvp.Value.Type))
                .Count();
            return numberOfBlocks;
        }

        public static Dictionary<GridPoint, GameGridCell> RunGame(BigInteger[] program)
        {
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            var programStatus = IntcodeProgramStatus.Running;
            int currentOutputCount = 0;
            var gameCells = new Dictionary<GridPoint, GameGridCell>();
            while (IntcodeProgramStatus.Running.Equals(programStatus)
                || IntcodeProgramStatus.AwaitingInput.Equals(programStatus))
            {
                programStatus = computer.RunProgram();
                int outputCountDifference = outputListener.Values.Count - currentOutputCount;
                if (outputCountDifference % 3 != 0)
                    throw new Exception("Invalid output encountered");
                gameCells = new Dictionary<GridPoint, GameGridCell>();
                for (int outputIndex = currentOutputCount; outputIndex < outputListener.Values.Count; outputIndex+=3)
                {
                    var x = outputListener.Values[outputIndex];
                    var y = outputListener.Values[outputIndex + 1];
                    var type = outputListener.Values[outputIndex + 2];
                    var point = new GridPoint((int)x, (int)y);
                    var gameCell = new GameGridCell((int)x, (int)y, (GameCellType)(int)type);
                    gameCells.Add(point, gameCell);
                }
                currentOutputCount = outputListener.Values.Count;
            }
            return gameCells;
        }

        public static void DrawGameBoard(Dictionary<GridPoint, GameGridCell> gameCells)
        {
            int minX = gameCells.Min(kvp => kvp.Key.X);
            int maxX = gameCells.Max(kvp => kvp.Key.X);
            int minY = gameCells.Min(kvp => kvp.Key.Y);
            int maxY = gameCells.Max(kvp => kvp.Key.Y);
            Console.WriteLine();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new GridPoint(x, y);
                    if (gameCells.ContainsKey(point))
                    {
                        var cell = gameCells[point];
                        var cellString = cell.DrawCell();
                        Console.Write(cellString);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static BigInteger[] GetDay13Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
