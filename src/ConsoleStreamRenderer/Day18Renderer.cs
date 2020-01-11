using AdventOfCode2019.Challenges.Day18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleStreamRenderer
{
    public static class Day18Renderer
    {
        public static void RenderDay18Part1()
        {
            Console.WriteLine("Processing day 18 - part 1...");
            var mazeStatePathResult = Day18.GetDay18Part1AnswerPath();
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
                    renderer.Frames = new List<Frame>() { stateFrame };
                    renderer.Render();
                }
            }
        }
    }
}
