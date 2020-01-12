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

        public static int GetDay19Part2Answer()
        {
            // Find the 100x100 square closest to the emitter that fits 
            // entirely within the tractor beam; within that square, find the 
            // point closest to the emitter. What value do you get if you take 
            // that point's X coordinate, multiply it by 10000, then add the 
            // point's Y coordinate? (In the example above, this would be 
            // 250020.)
            // Answer: 10180726
            var program = GetDay19Input();
            var closestPoint = GetClosestBoxThatFitsInsideBeam(
                program: program,
                boxWidth: 100,
                boxHeight: 100);
            int result = (closestPoint.X * 10000) + closestPoint.Y;
            return result;
        }

        public static GridPoint GetClosestBoxThatFitsInsideBeam(
            BigInteger[] program,
            int boxWidth,
            int boxHeight)
        {
            int previousLineBeamStartX = 0;
            int previousMaxBeamWidth = 0;

            // Initialize the y-increment to a large value, and then on each
            // loop, decrease it to a minimum of 1
            int yIncrement = int.MaxValue;
            int y = 0;
            // Assumption:
            // Based on observation, the beam has to be at least this wide
            // at the line containing the top of the box.
            int minBeamWidth = 2 * boxWidth - 1;
            while(true)
            {
                bool lineContainsBeam = ScanLine(
                    program: program,
                    y: y,
                    xStart: previousLineBeamStartX,
                    previousMaxBeamWidth: previousMaxBeamWidth,
                    beamBoundaries: out Tuple<GridPoint, GridPoint> beamBoundaries);
                previousLineBeamStartX = beamBoundaries.Item1.X;
                int beamWidth = beamBoundaries.Item2.X - beamBoundaries.Item1.X + 1;
                if (beamWidth > previousMaxBeamWidth)
                    previousMaxBeamWidth = beamWidth;
                if (!lineContainsBeam)
                {
                    y++;
                    continue;
                }

                // Assumption:
                // The beam width never *increases* by more than 1 plus the
                // previous widest beam, so it's safe to try to move y by
                // the difference between the min beam width and the current
                if (beamWidth < minBeamWidth)
                {
                    yIncrement = Math.Min(yIncrement, minBeamWidth - beamWidth);
                    y += yIncrement;
                    continue;
                }

                // Start checking a little into the beam
                // Assumption: The beam is constantly shifting to the right as
                // y increases, so X has to start by nearly the box width into
                // the beam
                int startX = Math.Max(beamBoundaries.Item1.X, beamBoundaries.Item1.X + boxWidth - 2);
                // Don't check x-values where the distance to the end of the
                // beam is less than the box width
                int endX = beamBoundaries.Item2.X - boxWidth + 1;
                for (int x = startX; x <= endX; x++)
                {
                    var topLeft = new GridPoint(x, y);
                    bool isBoxInsideBeam = GetIsBoxInsideBeam(
                        program: program,
                        topLeft: topLeft,
                        width: boxWidth,
                        height: boxHeight,
                        isTopLeftAlreadyScanned: true);
                    if (isBoxInsideBeam)
                    {
                        return topLeft;
                    }
                }
                yIncrement = 1;
                y += yIncrement;
            }
        }

        public static bool GetIsBoxInsideBeam(
            BigInteger[] program, 
            GridPoint topLeft,
            int width, 
            int height,
            bool isTopLeftAlreadyScanned)
        {
            var boxCorners = new List<GridPoint>()
            {
                topLeft.MoveRight(width-1),
                topLeft.MoveUp(height-1)
            };
            if (!isTopLeftAlreadyScanned)
                boxCorners.Add(topLeft);
            foreach (var corner in boxCorners)
            {
                var scanResult = ScanPoint(program, corner);
                if (!DroneStatus.Pulled.Equals(scanResult))
                    return false;
            }
            return true;
        }

        public static Dictionary<GridPoint, DroneStatus> ScanArea(BigInteger[] program, int maxX, int maxY)
        {
            var result = new Dictionary<GridPoint, DroneStatus>();
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    var point = new GridPoint(x, y);
                    var droneStatus = ScanPoint(program, point);
                    result.Add(point, droneStatus);
                }
            }
            return result;
        }

        /// <summary>
        /// Scans a given y-value and returns the start and end points of the
        /// beam.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool ScanLine(
            BigInteger[] program, 
            int y, 
            int xStart,
            int previousMaxBeamWidth,
            out Tuple<GridPoint, GridPoint> beamBoundaries)
        {
            bool foundBeam = false;
            GridPoint startPoint = new GridPoint(-1, -1);
            GridPoint endPoint = new GridPoint(-1, -1);
            beamBoundaries = new Tuple<GridPoint, GridPoint>(startPoint, endPoint);
            int maxX = (y + 1) * 10;
            int xIncrement;
            for (int x = xStart; foundBeam || (x < maxX) ; x+= xIncrement)
            {
                xIncrement = 1;
                var point = new GridPoint(x, y);
                var scanResult = ScanPoint(program, point);
                if (DroneStatus.Pulled.Equals(scanResult))
                {
                    if (!foundBeam)
                    {
                        startPoint = point;
                        // Assumption: The beam width is never narrower than the
                        // previous max beam width minus one.
                        // Therefore, once we've found the beam, we can move
                        // x to a bit before that previous max to more quickly
                        // scan for the end of the beam
                        xIncrement = Math.Max(1, previousMaxBeamWidth - 2);
                    }   
                    foundBeam = true;
                    endPoint = point;
                }
                else if (foundBeam)
                {
                    break;
                }
            }
            if (foundBeam)
            {
                beamBoundaries = new Tuple<GridPoint, GridPoint>(startPoint, endPoint);
            }
            return foundBeam;
        }

        public static DroneStatus ScanPoint(BigInteger[] program, GridPoint point)
        {
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            inputProvider.AddInputValue(point.X);
            inputProvider.AddInputValue(point.Y);
            computer.RunProgram();
            var droneStatus = (DroneStatus)(int)outputListener.Values[0];
            return droneStatus;
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

        public static BigInteger[] GetDay19Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
