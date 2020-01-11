using AdventOfCode2019.Grid;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day19
{
    /// <summary>
    /// Solution to the Day 19 challenge:
    /// https://adventofcode.com/2019/day/19
    /// </summary>
    public class Day19
    {
        public const string FILE_NAME = "Day19Input.txt";
        public static int GetDay19Part1Answer()
        {
            // How many points are affected by the tractor beam in the 50x50 a
            // rea closest to the emitter? 
            // (For each of X and Y, this will be 0 through 49.)
            // Answer: 199
            var program = GetDay19Input();
            var scanResult = ScanArea(program, 50, 50);
            DrawScanResult(scanResult);
            var result = scanResult
                .Where(kvp => DroneStatus.Pulled.Equals(kvp.Value))
                .Count();
            return result;
        }

        public static void DrawScanResult(Dictionary<GridPoint, DroneStatus> scanResult)
        {
            string GetPointString(GridPoint point)
            {
                if (!scanResult.ContainsKey(point))
                    return " ";
                var droneStatus = scanResult[point];
                if (DroneStatus.Stationary.Equals(droneStatus))
                    return ".";
                else if (DroneStatus.Pulled.Equals(droneStatus))
                    return "#";
                return " ";
            }
            GridHelper.DrawGrid2D(
                gridPoints: scanResult.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetPointString);
        }

        public static Dictionary<GridPoint, DroneStatus> ScanArea(BigInteger[] program, int maxX, int maxY)
        {
            var result = new Dictionary<GridPoint, DroneStatus>();
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    var inputProvider = new BufferedInputProvider();
                    var outputListener = new ListOutputListener();
                    var computer = new IntcodeComputer(inputProvider, outputListener);
                    computer.LoadProgram(program);
                    inputProvider.AddInputValue(x);
                    inputProvider.AddInputValue(y);
                    computer.RunProgram();
                    var point = new GridPoint(x, y);
                    var droneStatus = (DroneStatus)(int)outputListener.Values[0];
                    result.Add(point, droneStatus);
                }
            }
            return result;
        }

        public static BigInteger[] GetDay19Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
