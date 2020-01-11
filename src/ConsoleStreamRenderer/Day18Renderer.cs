using AdventOfCode2019.Challenges.Day18;
using AdventOfCode2019.Grid.PathFinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleStreamRenderer
{
    public static class Day18Renderer
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        public static void RenderDay18Part1()
        {
            RenderDay18(1);
        }
        public static void RenderDay18Part2()
        {
            RenderDay18(2);
        }
        public static void RenderDay18(int part)
        {
            Console.WriteLine($"Processing day 18 - part {part}...");
            PathResult<MazeState> mazeStatePathResult;
            if (part == 1)
                mazeStatePathResult = Day18.GetDay18Part1AnswerPath();
            else
                mazeStatePathResult = Day18.GetDay18Part2AnswerPath();
            int imageNumber = 0;
            for (int i = 0; i < mazeStatePathResult.Path.Count - 1; i++)
            {
                var startState = mazeStatePathResult.Path[i];
                var endState = mazeStatePathResult.Path[i + 1];

                // Get intermediate maze states going from the initial to final state
                int positionIndex = MazeState.GetPositionThatChangedIndex(startState, endState);
                var robotPathBetweenStates = MazeState.GetRobotPathBetweenMazeStates(startState, endState);
                for (int robotMovementIndex = 0; robotMovementIndex < robotPathBetweenStates.Count; robotMovementIndex++)
                {
                    if (i > 0 && robotMovementIndex == 0)
                        continue;
                    var currentPositions = startState.CurrentPositions.ToList();
                    currentPositions[positionIndex] = robotPathBetweenStates[robotMovementIndex];
                    var intermediateState = new MazeState(
                        maze: startState.Maze,
                        currentPositions: currentPositions,
                        keysCollected: startState.KeysCollected);
                    var stateFrame = new Frame(intermediateState.GetMazeStateRenderingData());
                    var renderer = new ConsoleStreamRenderer();
                    renderer.Render(stateFrame);
                    imageNumber++;
                    IntPtr consoleWindowHandle = GetConsoleWindow();
                    var consoleWindowBitmap = ScreenCapture.CaptureWindow(consoleWindowHandle);
                    var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", $"Part1_{imageNumber.ToString("0000")}.png");
                    consoleWindowBitmap.Save(imageFilePath, ImageFormat.Png);
                }
            }
        }
    }
}
