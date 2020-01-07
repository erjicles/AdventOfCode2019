using AdventOfCode2019.Grid;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day17
{
    /// <summary>
    /// Solution to the Day 17 challenge:
    /// https://adventofcode.com/2019/day/17
    /// </summary>
    public class Day17
    {
        public const string FILE_NAME = "Day17Input.txt";

        public static int GetDay17Part1Answer()
        {
            // To calibrate the cameras, you need the sum of the alignment 
            // parameters.
            // Run your ASCII program. What is the sum of the alignment 
            // parameters for the scaffold intersections?
            // Answer: 8928
            var scaffoldMap = PerformInitialScan();
            DrawScaffold(scaffoldMap);
            var scaffoldCells = GetScaffoldCells(scaffoldMap);
            var scaffoldIntersections = GetScaffoldIntersections(scaffoldCells);
            int alignmentParameterSum = GetCameraCalibrationNumber(scaffoldIntersections);
            return alignmentParameterSum;
        }

        public static BigInteger GetDay17Part2Answer()
        {
            // After visiting every part of the scaffold at least once, how 
            // much dust does the vacuum robot report it has collected?
            // Answer: 880360
            BigInteger result = RunVaccuumRobot(false);
            return result;
        }

        public static BigInteger RunVaccuumRobot(bool isManualInput)
        {
            // Upon inspection, the following commands solve the problem:
            // A,B,A,B,A,C,B,C,A,C
            // A: L,6,R,6,6,L,6 
            // B: R,6,6,L,5,5,L,4,L,6
            // C: L,5,5,L,5,5,L,4,L,6
            BigInteger[] program = GetDay17Input();
            program[0] = 2;
            var encodedCommands = EncodeRobotCommands(
                mainMovementRoutine: "A,B,A,B,A,C,B,C,A,C",
                movementFunctionA: "L,6,R,6,6,L,6",
                movementFunctionB: "R,6,6,L,5,5,L,4,L,6",
                movementFunctionC: "L,5,5,L,5,5,L,4,L,6",
                continuousVideoFeed: false);
            IInputProvider inputProvider;
            if (isManualInput)
            {
                inputProvider = new ConsoleInputProvider();
            }
            else
            {
                inputProvider = new BufferedInputProvider();
            }
            var outputListener = new ListOutputListener();
            IntcodeComputer computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            var computerStatus = IntcodeProgramStatus.Running;
            int outputStartIndex = 0;
            int commandIndex = 0;
            while (IntcodeProgramStatus.Running.Equals(computerStatus)
                || IntcodeProgramStatus.AwaitingInput.Equals(computerStatus))
            {
                // Provide inputs if automated
                if (IntcodeProgramStatus.AwaitingInput.Equals(computerStatus)
                    && !isManualInput)
                {
                    ((BufferedInputProvider)inputProvider).AddInputValue(encodedCommands[commandIndex]);
                    Console.Write(encodedCommands[commandIndex]);
                    commandIndex++;
                }

                // Run program
                computerStatus = computer.RunProgram();

                // Display output
                if (outputListener.Values.Count > 0)
                {
                    DisplayProgramOutput(outputListener, outputStartIndex);
                    outputStartIndex = outputListener.Values.Count;
                }
            }
            return outputListener.Values.LastOrDefault();
        }

        public static void DisplayProgramOutput(ListOutputListener outputListener, int startIndex)
        {
            var outputStrings = GetProgramOutputStrings(outputListener.Values.GetRange(startIndex, outputListener.Values.Count - startIndex));
            Console.WriteLine();
            foreach (var outputString in outputStrings)
            {
                Console.WriteLine("     " + outputString);
            }
        }

        public static IList<BigInteger> EncodeRobotCommands(
            string mainMovementRoutine,
            string movementFunctionA,
            string movementFunctionB,
            string movementFunctionC,
            bool continuousVideoFeed)
        {
            var result = new List<BigInteger>();
            result.AddRange(EncodeRobotCommandString(mainMovementRoutine));
            result.AddRange(EncodeRobotCommandString(movementFunctionA));
            result.AddRange(EncodeRobotCommandString(movementFunctionB));
            result.AddRange(EncodeRobotCommandString(movementFunctionC));
            result.AddRange(EncodeRobotCommandString(continuousVideoFeed ? "y" : "n"));
            return result;
        }

        public static BigInteger[] EncodeRobotCommandString(string commandString)
        {
            var result = new List<BigInteger>();
            var separatedCommands = commandString.Split(",");
            foreach (var command in separatedCommands)
            {
                if (result.Count > 0)
                    result.Add(char.ConvertToUtf32(",", 0));
                result.Add(char.ConvertToUtf32(command, 0));
            }
            result.Add(10);
            return result.ToArray();
        }

        public static int GetCameraCalibrationNumber(ICollection<GridPoint> scaffoldIntersections)
        {
            int alignmentParameterSum = 0;
            foreach (var point in scaffoldIntersections)
            {
                int alignmentParameter = GetAlignmentParameter(point);
                alignmentParameterSum += alignmentParameter;
            }
            return alignmentParameterSum;
        }

        public static int GetAlignmentParameter(GridPoint point)
        {
            // The first step is to calibrate the cameras by getting the 
            // alignment parameters of some well-defined points. Locate all 
            // scaffold intersections; for each, its alignment parameter is 
            // the distance between its left edge and the left edge of the 
            // view multiplied by the distance between its top edge and the 
            // top edge of the view.
            return point.X * point.Y;
        }

        public static void DrawScaffold(Dictionary<GridPoint, string> scaffoldMap)
        {
            string GetScaffoldCellString(GridPoint point)
            {
                if (scaffoldMap.ContainsKey(point))
                    return scaffoldMap[point];
                return " ";
            }
            GridHelper.DrawGrid2D(
                gridPoints: scaffoldMap.Select(kvp => kvp.Key).ToList(),
                GetPointString: GetScaffoldCellString);
        }

        public static HashSet<GridPoint> GetScaffoldIntersections(HashSet<GridPoint> scaffoldCells)
        {
            var result = new HashSet<GridPoint>();
            foreach (var scaffoldCell in scaffoldCells)
            {
                // It is only an intersection if the cells to the left, right,
                // top, and bottom are all also scaffold cells
                bool isScaffoldLeft = scaffoldCells.Contains(scaffoldCell.MoveLeft(1));
                bool isScaffoldRight = scaffoldCells.Contains(scaffoldCell.MoveRight(1));
                bool isScaffoldTop = scaffoldCells.Contains(scaffoldCell.MoveDown(1));
                bool isScaffoldBottom = scaffoldCells.Contains(scaffoldCell.MoveUp(1));
                if (isScaffoldLeft 
                    && isScaffoldRight 
                    && isScaffoldTop 
                    && isScaffoldBottom)
                {
                    result.Add(scaffoldCell);
                }
            }
            return result;
        }

        public static HashSet<GridPoint> GetScaffoldCells(Dictionary<GridPoint, string> scaffoldMap)
        {
            var result = new HashSet<GridPoint>();
            foreach (var kvp in scaffoldMap)
            {
                if (".".Equals(kvp.Value))
                    continue;
                if ("X".Equals(kvp.Value))
                    continue;
                result.Add(kvp.Key);
            }
            return result;
        }

        public static Dictionary<GridPoint, string> PerformInitialScan()
        {
            BigInteger[] program = GetDay17Input();
            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            IntcodeComputer computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);
            computer.RunProgram();
            var scaffoldMap = ProcessInitialScan(outputListener.Values);
            return scaffoldMap;
        }

        public static Dictionary<GridPoint, string> ProcessInitialScan(IList<BigInteger> cameraScanOutput)
        {
            var rowStrings = GetProgramOutputStrings(cameraScanOutput);
            var result = ProcessScan(rowStrings);
            return result;
        }

        public static IList<string> GetProgramOutputStrings(IList<BigInteger> programOutput)
        {
            var rowStrings = new List<string>();
            var rowStringBuilder = new StringBuilder();
            foreach (int cameraOutputCode in programOutput)
            {
                // If it's a newline, then set x and y for the next line
                if (cameraOutputCode == 10)
                {
                    rowStrings.Add(rowStringBuilder.ToString());
                    rowStringBuilder.Clear();
                    continue;
                }
                string cameraOutputString = char.ConvertFromUtf32(cameraOutputCode);
                rowStringBuilder.Append(cameraOutputString);
            }
            if (rowStringBuilder.Length > 0)
                rowStrings.Add(rowStringBuilder.ToString());
            return rowStrings;
        }

        public static Dictionary<GridPoint, string> ProcessScan(IList<string> rowStrings)
        {
            var result = new Dictionary<GridPoint, string>();
            int y = 0;
            foreach (var rowString in rowStrings)
            {
                for (int x = 0; x < rowString.Length; x++)
                {
                    result.Add(new GridPoint(x, y), rowString.Substring(x, 1));
                }
                y++;
            }
            return result;
        }

        public static BigInteger[] GetDay17Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
