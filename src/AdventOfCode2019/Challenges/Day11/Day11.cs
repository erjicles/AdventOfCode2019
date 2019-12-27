using AdventOfCode2019.Grid;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day11
{
    /// <summary>
    /// Solution to the Day 11 challenge:
    /// https://adventofcode.com/2019/day/11
    /// </summary>
    public class Day11
    {
        public const string FILE_NAME = "Day11Input.txt";

        public static int GetDay11Part1Answer()
        {
            // Build a new emergency hull painting robot and run the Intcode 
            // program on it. How many panels does it paint at least once?
            // Answer: 1883
            var program = GetDay11Input();
            var gridPaintColors = RunEmergencyHullPaintingRobot(program);
            return gridPaintColors.Count;
        }

        /// <summary>
        /// Runs the given input program and returns a dictionary containing
        /// the set of points painted at least once and their final paint color.
        /// once, and 
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static Dictionary<GridPoint, int> RunEmergencyHullPaintingRobot(IList<BigInteger> program)
        {
            var gridPaintColors = new Dictionary<GridPoint, int>();
            int defaultPaintColor = 0;

            var inputProvider = new BufferedInputProvider();
            var outputListener = new ListOutputListener();
            var computer = new IntcodeComputer(inputProvider, outputListener);
            computer.LoadProgram(program);

            var currentGridPoint = new GridPoint(0, 0);
            RobotDirection currentRobotDirection = RobotDirection.Up;
            var programStatus = IntcodeProgramStatus.Running;
            int currentOutputCount = 0;
            while (IntcodeProgramStatus.Running.Equals(programStatus)
                || IntcodeProgramStatus.AwaitingInput.Equals(programStatus))
            {
                // Provide input value for the current grid
                int inputValue = gridPaintColors.ContainsKey(currentGridPoint) ? 
                    gridPaintColors[currentGridPoint] : 
                    defaultPaintColor;
                inputProvider.AddInputValue(inputValue);

                // Run the program, given the input
                programStatus = computer.RunProgram();

                // Process new output values
                int numberOfNewValues = outputListener.Values.Count - currentOutputCount;
                currentOutputCount = outputListener.Values.Count;
                if (numberOfNewValues == 0 || numberOfNewValues > 2)
                    throw new Exception($"Program output invalid number of new values during input loop {numberOfNewValues}");
                BigInteger paintCommand = outputListener.Values[currentOutputCount - 2];
                BigInteger turnCommand = outputListener.Values[currentOutputCount - 1];

                // Paint the current cell
                if (paintCommand != 0 && paintCommand != 1)
                    throw new Exception($"Invalid paint command {paintCommand}");
                if (!gridPaintColors.ContainsKey(currentGridPoint))
                    gridPaintColors.Add(currentGridPoint, defaultPaintColor);
                gridPaintColors[currentGridPoint] = (int)paintCommand;

                // Turn the robot
                if (turnCommand != 0 && turnCommand != 1)
                    throw new Exception($"Invalid turn command {turnCommand}");
                currentRobotDirection = GetNewRobotDirection(currentRobotDirection, (int)turnCommand);

                // Move the robot
                currentGridPoint = MoveRobot(currentGridPoint, currentRobotDirection);

            }

            return gridPaintColors;

        }

        public static GridPoint MoveRobot(
            GridPoint initialPoint, 
            RobotDirection robotDirection)
        {
            if (RobotDirection.Left.Equals(robotDirection))
                return initialPoint.MoveLeft(1);
            else if (RobotDirection.Right.Equals(robotDirection))
                return initialPoint.MoveRight(1);
            else if (RobotDirection.Up.Equals(robotDirection))
                return initialPoint.MoveUp(1);
            else if (RobotDirection.Down.Equals(robotDirection))
                return initialPoint.MoveDown(1);
            else
                throw new Exception($"Invalid robot direction {robotDirection}");
        }

        public static RobotDirection GetNewRobotDirection(
            RobotDirection initialRobotDirection, 
            int turnCommand)
        {
            // Second, it will output a value indicating the direction the 
            // robot should turn: 
            // 0 means it should turn left 90 degrees, 
            // and 1 means it should turn right 90 degrees.
            if (turnCommand == 0)
            {
                if (RobotDirection.Left.Equals(initialRobotDirection))
                    return RobotDirection.Down;
                else if (RobotDirection.Down.Equals(initialRobotDirection))
                    return RobotDirection.Right;
                else if (RobotDirection.Right.Equals(initialRobotDirection))
                    return RobotDirection.Up;
                else if (RobotDirection.Up.Equals(initialRobotDirection))
                    return RobotDirection.Left;
                else
                    throw new Exception($"Unrecognized initial direction {initialRobotDirection}");
            }
            else if (turnCommand == 1)
            {
                if (RobotDirection.Left.Equals(initialRobotDirection))
                    return RobotDirection.Up;
                else if (RobotDirection.Up.Equals(initialRobotDirection))
                    return RobotDirection.Right;
                else if (RobotDirection.Right.Equals(initialRobotDirection))
                    return RobotDirection.Down;
                else if (RobotDirection.Down.Equals(initialRobotDirection))
                    return RobotDirection.Left;
                else
                    throw new Exception($"Unrecognized initial direction {initialRobotDirection}");
            }
            else
            {
                throw new Exception($"Unrecognized turn command {turnCommand}");
            }
        }

        public static BigInteger[] GetDay11Input()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }

        public enum RobotDirection
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}
