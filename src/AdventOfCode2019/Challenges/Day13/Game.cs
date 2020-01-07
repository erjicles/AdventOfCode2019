using AdventOfCode2019.Grid;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace AdventOfCode2019.Challenges.Day13
{
    public class Game
    {
        private BigInteger[] _program;
        private IntcodeComputer _computer;
        private int _manualModeThreadSleepTimerMs = 500;
        private int _automatedModeTheradSleepTimerMs = 150;
        public IntcodeProgramStatus GameStatus { get; private set; }

        public HashSet<GameGridCell> GridCells { get; private set; }
        public long Score { get; private set; } = 0;
        public BufferedInputProvider InputProvider { get; set; }
        public ListOutputListener OutputListener { get; set; }
        public GameGridCell[] Last3BallPositions { get; set; } = new GameGridCell[3];
        public IList<int> BallPaddleContactTurns { get; private set; } = new List<int>();
        public IList<int> PreProgrammedInputs { get; set; } = new List<int>();
        public GameGridCell Ball
        {
            get
            {
                return GridCells
                    .Where(cell => GameCellType.Ball.Equals(cell.Type))
                    .FirstOrDefault();
            }
        }
        public GameGridCell HorizontalPaddle
        {
            get
            {
                return GridCells
                    .Where(cell => GameCellType.HorizontalPaddle.Equals(cell.Type))
                    .FirstOrDefault();
            }
        }
        public bool DrawBoard { get; set; } = true;
        public bool ClearConsoleWhenDrawingBoard { get; set; } = true;
        public GameMode Mode { get; set; } = GameMode.Automated;
        public int NumberOfBlocksRemaining
        {
            get
            {
                return GridCells
                    .Where(cell => GameCellType.Block.Equals(cell.Type))
                    .Count();
            }
        }
        private int _timeIndex = 0;

        public Game(BigInteger[] program)
        {
            InitializeGame(program, 0, new List<int>());
        }

        public Game(BigInteger[] program, int numberOfQuarters)
        {
            InitializeGame(program, numberOfQuarters, new List<int>());
        }

        public Game(BigInteger[] program, int numberOfQuarters, IList<int> preProgrammedInputs)
        {
            InitializeGame(program, numberOfQuarters, preProgrammedInputs);
        }

        private void InitializeGame(
            BigInteger[] program, 
            int numberOfQuarters, 
            IList<int> preProgrammedInputs)
        {
            _program = new BigInteger[program.Length];
            Array.Copy(program, _program, program.Length);
            // Memory address 0 represents the number of quarters that have 
            // been inserted; set it to 2 to play for free.
            if (numberOfQuarters > 0)
            {
                _program[0] = numberOfQuarters;
            }
            InputProvider = new BufferedInputProvider();
            if (preProgrammedInputs != null)
            {
                PreProgrammedInputs = preProgrammedInputs.ToList();
            }
            OutputListener = new ListOutputListener();
            _computer = new IntcodeComputer(InputProvider, OutputListener);
            _computer.LoadProgram(_program);

            GridCells = new HashSet<GameGridCell>();
            GameStatus = IntcodeProgramStatus.Running;
        }

        public void RunGame()
        {
            int preProgrammedInputsIndex = 0;
            while (IntcodeProgramStatus.Running.Equals(GameStatus)
                || IntcodeProgramStatus.AwaitingInput.Equals(GameStatus))
            {
                OutputListener.Values.Clear();

                if (preProgrammedInputsIndex < PreProgrammedInputs.Count)
                {
                    InputProvider.AddInputValue(PreProgrammedInputs[preProgrammedInputsIndex]);
                    preProgrammedInputsIndex++;
                }
                if (!InputProvider.HasInput()
                    && IntcodeProgramStatus.AwaitingInput.Equals(GameStatus))
                {
                    int paddleCommand = GetPaddleCommand();
                    InputProvider.AddInputValue(paddleCommand);
                }
                GameStatus = _computer.RunProgram();

                // Output values must be a multiple of 3
                if (OutputListener.Values.Count % 3 != 0)
                    throw new Exception("Invalid output count encountered");

                RefreshGridCells();
                if (DrawBoard)
                    DrawGameBoard();
                if (GameMode.Manual.Equals(Mode)
                    && IntcodeProgramStatus.AwaitingInput.Equals(GameStatus))
                {
                    PauseGameAndCheckForInput();
                }
                else if (DrawBoard)
                {
                    Thread.Sleep(_automatedModeTheradSleepTimerMs);
                }

                _timeIndex++;
            }
        }

        private int GetPaddleCommand()
        {
            if (GameMode.Automated.Equals(Mode))
            {
                var paddle = HorizontalPaddle;
                var ball = Ball;
                if (paddle != null && Ball != null)
                {
                    var dX = ball.X - paddle.X;
                    return Math.Sign(dX);
                }
            }
            return 0;
        }

        private void PauseGameAndCheckForInput()
        {
            // If the joystick is in the neutral position, provide 0.
            // If the joystick is tilted to the left, provide -1.
            // If the joystick is tilted to the right, provide 1.
            Console.WriteLine("Move the paddle: -1: Left, 0: Don't mode; 1: Right");
            string inputValue;
            Thread.Sleep(_manualModeThreadSleepTimerMs);
            bool success = GameConsoleReader.TryReadKey(out inputValue, _manualModeThreadSleepTimerMs);
            //Thread.Sleep(2000);
            if (success)
            {
                if (inputValue == "-1")
                    InputProvider.AddInputValue(-1);
                else if (inputValue == "0")
                    InputProvider.AddInputValue(0);
                else if (inputValue == "1")
                    InputProvider.AddInputValue(1);
                else
                    InputProvider.AddInputValue(0);
            }
            else
                InputProvider.AddInputValue(0);
        }

        private void RefreshGridCells()
        {
            // The software draws tiles to the screen with output instructions: 
            // every three output instructions specify the x position 
            // (distance from the left), y position (distance from the top), 
            // and tile id.
            // The arcade cabinet also has a segment display capable of showing 
            // a single number that represents the player's current score. 
            // When three output instructions specify X=-1, Y=0, the third 
            // output instruction is not a tile; the value instead specifies 
            // the new score to show in the segment display. For example, a 
            // sequence of output values like -1,0,12345 would show 12345 as 
            // the player's current score.
            for (int outputIndex = 0; outputIndex < OutputListener.Values.Count; outputIndex += 3)
            {
                var x = OutputListener.Values[outputIndex];
                var y = OutputListener.Values[outputIndex + 1];
                var param3Val = OutputListener.Values[outputIndex + 2];
                if (x == -1 && y == 0)
                {
                    Score = (long)param3Val;
                } 
                else
                {
                    var gridCell = new GameGridCell((int)x, (int)y, (GameCellType)(int)param3Val);
                    if (GridCells.Contains(gridCell))
                    {
                        GridCells.Remove(gridCell);
                    }
                    GridCells.Add(gridCell);
                    if (GameCellType.Ball.Equals(gridCell.Type))
                    {
                        Last3BallPositions[2] = Last3BallPositions[1];
                        Last3BallPositions[1] = Last3BallPositions[0];
                        Last3BallPositions[0] = gridCell;
                    }
                }
            }
            // Check if this is a ball/paddle contact
            var dX = Math.Abs(Ball.X - HorizontalPaddle.X);
            var dY = Math.Abs(Ball.Y - HorizontalPaddle.Y);
            if (dX <= 1 && dY == 1)
            {
                BallPaddleContactTurns.Add(_timeIndex);
            }
        }

        public void DrawGameBoard()
        {
            string GetBoardCellString(GridPoint gridPoint)
            {
                var point = new GameGridCell(gridPoint.X, gridPoint.Y);
                bool isCellPresent = GridCells.TryGetValue(point, out var cell);
                if (isCellPresent)
                {
                    return cell.DrawCell();
                }
                return " ";
            }

            if (ClearConsoleWhenDrawingBoard)
                Console.Clear();
            GridHelper.DrawGrid2D(
                gridPoints: GridCells.Select(gc => new GridPoint(gc.X, gc.Y)).ToList(),
                GetPointString: GetBoardCellString);
            Console.WriteLine($"     Blocks Remaining: {NumberOfBlocksRemaining}");
            Console.WriteLine($"     Score: {Score}");
            Console.WriteLine($"     Time index: {_timeIndex}");
            Console.WriteLine();
        }
    }
}
