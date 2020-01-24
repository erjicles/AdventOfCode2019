using AdventOfCode2019.Grid;
using AdventOfCode2019.Grid.PathFinding;
using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day25
{
    public class ShipMap
    {
        public Dictionary<string, ShipSectionInfo> ExploredPoints { get; private set; }
        public Tree<string> ExplorationTree { get; private set; }
        public HashSet<string> UnexploredPoints { get; private set; }
        public IList<string> Items { get; private set; }
        public Dictionary<string, string> ItemLocations { get; private set; }
        public string RobotPosition { get; private set; }
        public HashSet<string> RobotInventory { get; private set; }
        public string RallyPoint { get; private set; }
        public string WeightFloor { get; private set; }
        public bool IsAutomated { get; private set; }
        private IntcodeComputer _computer;
        private ConsoleInputProvider _inputProviderManual;
        private BufferedInputProvider _inputProviderAutomated;
        private ListOutputListener _outputListener;
        private int _outputIndex = 0;
        private IntcodeProgramStatus _programStatus = IntcodeProgramStatus.Running;
        private HashSet<string> _neverPickUpTheseItems = new HashSet<string>()
        {
            "infinite loop", // Program gets stuck in infinite loop
            "photons", // It is suddenly completely dark! You are eaten by a Grue!
            "giant electromagnet", // The giant electromagnet is stuck to you.  You can't move!!
            "molten lava", // The molten lava is way too hot! You melt!
            "escape pod", // You're launched into space! Bye!
        };
        private int _programLoopCount = 0;
        public ShipMap(BigInteger[] program, bool isAutomated)
        {
            InitializeMap(program, isAutomated);
        }

        private void InitializeMap(BigInteger[] program, bool isAutomated)
        {
            ExploredPoints = new Dictionary<string, ShipSectionInfo>();
            ExplorationTree = new Tree<string>();
            UnexploredPoints = new HashSet<string>();
            ItemLocations = new Dictionary<string, string>();
            RobotPosition = "O";
            RobotInventory = new HashSet<string>();
            IsAutomated = isAutomated;
            ExplorationTree.Add(RobotPosition, null);
            UnexploredPoints.Add(RobotPosition);
            _inputProviderManual = new ConsoleInputProvider();
            _inputProviderAutomated = new BufferedInputProvider();
            IInputProvider inputProviderToUse = _inputProviderAutomated;
            if (!IsAutomated)
                inputProviderToUse = _inputProviderManual;
            _outputListener = new ListOutputListener();
            _computer = new IntcodeComputer(inputProviderToUse, _outputListener);
            _computer.LoadProgram(program);
        }

        public void Explore()
        {
            // Initialize the exploration variables
            string targetUnexploredPoint = RobotPosition;
            var movementCommandsToTargetUnexploredPoint = new Queue<string>();
            string nextRobotPosition = RobotPosition;

            // Continue exploring while points remain unexplored
            while (ExplorationTree.Count > 0)
            {
                // Run a loop of the program
                _programStatus = _computer.RunProgram();

                // Process program output
                // Assumption: If there is new output, then the robot has
                // entered a room corresponding to the next robot position
                if (_outputIndex < _outputListener.Values.Count)
                {
                    int numberOfNewOutputValues = _outputListener.Values.Count - _outputIndex;
                    var latestOutput = string.Join("", _outputListener.Values
                        .GetRange(_outputIndex, numberOfNewOutputValues)
                        .Select(v => char.ConvertFromUtf32((int)v)));
                    _outputIndex += numberOfNewOutputValues;
                    if (!IsAutomated)
                    {
                        Console.WriteLine(latestOutput);
                    }

                    var robotOutputResult = RobotOutputHelper.ProcessRobotOutput(latestOutput);
                    if (!RobotOutputType.MovedToPoint.Equals(robotOutputResult.Type)
                        && !RobotOutputType.EjectedBack.Equals(robotOutputResult.Type)
                        && IsAutomated)
                    {
                        throw new Exception("While exploring in automated mode, we should only either move into spaces or get ejected back by the security system");
                    } 

                    // Add this point to the set of explored points
                    if (robotOutputResult.ShipSectionInfo != null)
                    {
                        if (!ExploredPoints.ContainsKey(nextRobotPosition))
                            ExploredPoints.Add(nextRobotPosition, robotOutputResult.ShipSectionInfo);
                        ExploredPoints[nextRobotPosition] = robotOutputResult.ShipSectionInfo;

                        // Remove this point from the set of unxplored points
                        if (UnexploredPoints.Contains(nextRobotPosition))
                            UnexploredPoints.Remove(nextRobotPosition);
                    }

                    // Reset the target unexplored point if we've reached it
                    if (nextRobotPosition.Equals(targetUnexploredPoint))
                        targetUnexploredPoint = null;

                    // Only move the robot if they actually moved
                    if (RobotOutputType.MovedToPoint.Equals(robotOutputResult.Type))
                    {
                        RobotPosition = nextRobotPosition;
                    } 
                    else if (RobotOutputType.EjectedBack.Equals(robotOutputResult.Type))
                    {
                        WeightFloor = nextRobotPosition;
                        RallyPoint = RobotPosition;
                    }

                    // Add any new unexplored points
                    var newUnexploredPoints = GetNewUnexploredPoints();
                    ExplorationTree.Add(newUnexploredPoints, RobotPosition);
                    foreach (var p in newUnexploredPoints)
                    {
                        UnexploredPoints.Add(p);
                    }
                }

                // Provide input for the program
                if (IntcodeProgramStatus.AwaitingInput.Equals(_programStatus))
                {
                    if (!IsAutomated)
                    {
                        DrawMap();
                        var userInput = _inputProviderManual.AddInput(appendNewLine: true).Trim();
                        var isValidMovementCommand = RobotMovementHelper.GetIsValidMovementCommand(userInput);
                        if (isValidMovementCommand)
                        {
                            nextRobotPosition = RobotMovementHelper.Move(RobotPosition, userInput);
                        }
                        targetUnexploredPoint = GetNextTargetUnexploredPoint(
                            currentTargetUnexploredPoint: RobotPosition);
                    }
                    else
                    {
                        if (targetUnexploredPoint != null
                            && movementCommandsToTargetUnexploredPoint.Count == 0)
                            throw new Exception("Ran out of movement commands before reaching target");

                        // Acquire a new target unexplored point if necessary
                        // If the target unexplored point is null, then the
                        // robot reached the previous target
                        // If the next target unexplored point is null, then
                        // we've finished exploring
                        if (targetUnexploredPoint == null)
                        {
                            targetUnexploredPoint = GetNextTargetUnexploredPoint(
                                currentTargetUnexploredPoint: RobotPosition);
                            if (targetUnexploredPoint != null)
                            {
                                var pathToTarget = GetPathToPoint(
                                startPoint: RobotPosition,
                                targetPoint: targetUnexploredPoint);
                                if (pathToTarget.Path.Count == 0)
                                    throw new Exception("Path not found");
                                movementCommandsToTargetUnexploredPoint = GetMovementCommandsForPath(
                                    path: pathToTarget.Path);
                            }
                        }

                        // If the target is null, then we finished exploring
                        // Otherwise, input the new movement command
                        // and update the next robot position
                        if (targetUnexploredPoint != null)
                        {
                            if (movementCommandsToTargetUnexploredPoint.Count == 0)
                                throw new Exception("Failed to properly acquire new target path");

                            string movementCommand = movementCommandsToTargetUnexploredPoint.Dequeue();
                            _inputProviderAutomated.AddInputValue(movementCommand);
                            _inputProviderAutomated.AddInputValue(10);
                            nextRobotPosition = RobotMovementHelper.Move(RobotPosition, movementCommand);
                        }
                    }
                    
                }
                
                _programLoopCount++;
            }

            // Exploration complete, populate item lists
            InitializeItemTrackers();
        }

        public IList<string> AttemptToFindWinningCombination(out RobotOutputResult robotOutputResult)
        {
            var combos = GetAllCombinationsOfAllowedItems();
            foreach (var combo in combos)
            {
                var result = TryCombinationOfItems(combo, out RobotOutputResult robotResult);
                if (result)
                {
                    robotOutputResult = robotResult;
                    return combo;
                }
            }
            throw new Exception("No combination worked");
        }

        public bool TryCombinationOfItems(IList<string> items, out RobotOutputResult robotOutputResult)
        {
            foreach (var item in items)
            {
                PickUpItem(item);
            }
            var result = MoveRobotToPoint(WeightFloor);
            robotOutputResult = result;
            if (RobotOutputType.EjectedBack.Equals(result.Type))
            {
                foreach (var item in items)
                    DropItem(item);
                return false;
            }
            return true;
        }

        public IList<IList<string>> GetAllCombinationsOfAllowedItems()
        {
            var result = new List<IList<string>>();
            var allowedItems = Items
                .Where(item => !_neverPickUpTheseItems.Contains(item))
                .ToList();
            for (int subsetSize = 1; subsetSize <= allowedItems.Count; subsetSize++)
            {
                var combos = new Combinations<string>(allowedItems, subsetSize);
                foreach (var combo in combos)
                {
                    result.Add(combo);
                }
            }
            return result;
        }

        public void GatherItems()
        {
            var itemsToGather = ItemLocations
                .Where(kvp => !_neverPickUpTheseItems.Contains(kvp.Key))
                .Select(kvp => kvp.Key)
                .ToList();
            foreach (var item in itemsToGather)
            {
                MoveItemToRallyPoint(item);
            }
        }

        public void MoveItemToRallyPoint(string item)
        {
            if (!RobotInventory.Contains(item))
            {
                if (!ItemLocations.ContainsKey(item))
                    throw new Exception("Item not found");
                var itemLocation = ItemLocations[item];
                MoveRobotToPoint(itemLocation);
                PickUpItem(item);
            }
            MoveRobotToPoint(RallyPoint);
            DropItem(item);
        }

        private void InitializeItemTrackers()
        {
            ItemLocations = new Dictionary<string, string>();
            Items = new List<string>();
            foreach (var kvp in ExploredPoints)
            {
                var info = kvp.Value;
                foreach (var item in info.Items)
                {
                    ItemLocations.Add(item, kvp.Key);
                    Items.Add(item);
                }
            }
        }

        public void PickUpItem(string item)
        {
            // Provide input for the program
            if (IntcodeProgramStatus.AwaitingInput.Equals(_programStatus))
            {
                _inputProviderAutomated.AddInputValue("take " + item);
                _inputProviderAutomated.AddInputValue(10);
            }

            // Run a loop of the program
            _programStatus = _computer.RunProgram();

            // Process program output
            // Assumption: If there is new output, then the robot has
            // entered a room corresponding to the next robot position
            if (_outputIndex < _outputListener.Values.Count)
            {
                int numberOfNewOutputValues = _outputListener.Values.Count - _outputIndex;
                var latestOutput = string.Join("", _outputListener.Values
                    .GetRange(_outputIndex, numberOfNewOutputValues)
                    .Select(v => char.ConvertFromUtf32((int)v)));
                _outputIndex += numberOfNewOutputValues;

                var robotOutputResult = RobotOutputHelper.ProcessRobotOutput(latestOutput);
                if (!RobotOutputType.TookItem.Equals(robotOutputResult.Type)
                    && IsAutomated)
                {
                    throw new Exception("Failed to take the item");
                }

                RobotInventory.Add(item);
                ExploredPoints[RobotPosition].Items.Remove(item);
                ItemLocations.Remove(item);
            }

            _programLoopCount++;
        }

        public void DropItem(string item)
        {
            // Provide input for the program
            if (IntcodeProgramStatus.AwaitingInput.Equals(_programStatus))
            {
                _inputProviderAutomated.AddInputValue("drop " + item);
                _inputProviderAutomated.AddInputValue(10);
            }

            // Run a loop of the program
            _programStatus = _computer.RunProgram();

            // Process program output
            // Assumption: If there is new output, then the robot has
            // entered a room corresponding to the next robot position
            if (_outputIndex < _outputListener.Values.Count)
            {
                int numberOfNewOutputValues = _outputListener.Values.Count - _outputIndex;
                var latestOutput = string.Join("", _outputListener.Values
                    .GetRange(_outputIndex, numberOfNewOutputValues)
                    .Select(v => char.ConvertFromUtf32((int)v)));
                _outputIndex += numberOfNewOutputValues;

                var robotOutputResult = RobotOutputHelper.ProcessRobotOutput(latestOutput);
                if (!RobotOutputType.DroppedItem.Equals(robotOutputResult.Type)
                    && IsAutomated)
                {
                    throw new Exception("Failed to drop the item");
                }

                RobotInventory.Remove(item);
                ExploredPoints[RobotPosition].Items.Add(item);
                ItemLocations.Add(item, RobotPosition);
            }

            _programLoopCount++;
        }

        public RobotOutputResult MoveRobotToPoint(string targetPoint)
        {
            if (!IsAutomated)
                throw new Exception("This should only be done in automated mode");

            RobotOutputResult result = null;

            // Initialize the movement variables
            var path = GetPathToPoint(RobotPosition, targetPoint);
            if (path.Path.Count == 0)
                throw new Exception("Path not found");
            var movementCommands = GetMovementCommandsForPath(path.Path);
            string nextRobotPosition = RobotPosition;

            // Continue looping until the robot reaches the target
            while (!RobotPosition.Equals(targetPoint))
            {
                // Run a loop of the program
                _programStatus = _computer.RunProgram();

                // Process program output
                // Assumption: If there is new output, then the robot has
                // entered a room corresponding to the next robot position
                if (_outputIndex < _outputListener.Values.Count)
                {
                    int numberOfNewOutputValues = _outputListener.Values.Count - _outputIndex;
                    var latestOutput = string.Join("", _outputListener.Values
                        .GetRange(_outputIndex, numberOfNewOutputValues)
                        .Select(v => char.ConvertFromUtf32((int)v)));
                    _outputIndex += numberOfNewOutputValues;
                    
                    var robotOutputResult = RobotOutputHelper.ProcessRobotOutput(latestOutput);
                    if (!RobotOutputType.MovedToPoint.Equals(robotOutputResult.Type)
                        && !RobotOutputType.EjectedBack.Equals(robotOutputResult.Type)
                        && IsAutomated)
                    {
                        throw new Exception("While exploring in automated mode, we should only either move into spaces or get ejected back by the security system");
                    }

                    // Update the set of explored points
                    if (robotOutputResult.ShipSectionInfo != null)
                    {
                        if (!ExploredPoints.ContainsKey(nextRobotPosition))
                            ExploredPoints.Add(nextRobotPosition, robotOutputResult.ShipSectionInfo);
                        ExploredPoints[nextRobotPosition] = robotOutputResult.ShipSectionInfo;

                        // Remove this point from the set of unxplored points
                        if (UnexploredPoints.Contains(nextRobotPosition))
                            UnexploredPoints.Remove(nextRobotPosition);
                    }

                    // Only move the robot if they actually moved
                    if (RobotOutputType.MovedToPoint.Equals(robotOutputResult.Type))
                    {
                        RobotPosition = nextRobotPosition;
                    }
                    else if (RobotOutputType.EjectedBack.Equals(robotOutputResult.Type))
                    {
                        RallyPoint = RobotPosition;
                        result = robotOutputResult;
                        break;
                    }

                    if (RobotPosition.Equals(targetPoint))
                    {
                        result = robotOutputResult;
                        break;
                    }
                        
                }

                // Provide input for the program
                if (IntcodeProgramStatus.AwaitingInput.Equals(_programStatus))
                {
                    if (movementCommands.Count == 0
                        && !RobotPosition.Equals(targetPoint))
                        throw new Exception("Ran out of commands before robot reached target");

                    string movementCommand = movementCommands.Dequeue();
                    _inputProviderAutomated.AddInputValue(movementCommand);
                    _inputProviderAutomated.AddInputValue(10);
                    nextRobotPosition = RobotMovementHelper.Move(RobotPosition, movementCommand);
                }

                _programLoopCount++;
            }
            return result;
        }

        public Queue<string> GetMovementCommandsForPath(
            IList<string> path)
        {
            var result = new Queue<string>();
            var currentPoint = RobotPosition;
            foreach (var nextPoint in path)
            {
                if (currentPoint.Equals(nextPoint))
                    continue;
                string movementCommand = RobotMovementHelper.GetMovementCommandForNeighbors(currentPoint, nextPoint);
                result.Enqueue(movementCommand);
                currentPoint = nextPoint;
            }
            return result;
        }

        public PathResult<string> GetPathToPoint(
            string startPoint,
            string targetPoint)
        {
            if (startPoint == null)
                return new PathResult<string>(new List<string>(), 0);
            if (targetPoint == null)
                return new PathResult<string>(new List<string>(), 0);

            int Heuristic(string p)
            {
                // TODO: Get a better heuristic
                return 0;
            }

            var aStarResult = AStar.GetPath<string>(
                startPoint: startPoint,
                endPoint: targetPoint,
                Heuristic: Heuristic,
                GetNeighbors: GetNeighbors,
                GetEdgeCost: (p1, p2) => { return 1; });

            return aStarResult;
        }

        

        public IList<string> GetNeighbors(string point)
        {
            var result = new List<string>();
            if (!ExploredPoints.ContainsKey(point))
                return result;
            foreach (var directionString in ExploredPoints[point].Directions)
            {
                var neighbor = RobotMovementHelper.Move(point, directionString);
                result.Add(neighbor);
            }
            return result;
        }

        public string GetNextTargetUnexploredPoint(
            string currentTargetUnexploredPoint)
        {
            if (ExplorationTree.Count == 0)
                return null;
            bool GetIsUnexplored(string p)
            {
                return !ExploredPoints.ContainsKey(p);
            }
            var result = ExplorationTree.FindAndTrim(
                currentTargetUnexploredPoint,
                GetIsUnexplored);
            return result;
        }

        public IList<string> GetNewUnexploredPoints()
        {
            if (!ExploredPoints.ContainsKey(RobotPosition))
                throw new Exception("Robot position not explored");
            IList<string> result = new List<string>();
            foreach (var directionString in ExploredPoints[RobotPosition].Directions)
            {
                var nextPoint = RobotMovementHelper.Move(RobotPosition, directionString);
                if (!ExploredPoints.ContainsKey(nextPoint)
                    && !ExplorationTree.Contains(nextPoint))
                {
                    result.Add(nextPoint);
                }
            }
            return result;
        }

        public void DrawMap()
        {
            Console.WriteLine();
            Console.WriteLine("Explored points:");
            foreach (var kvp in ExploredPoints)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Unexplored points:");
            foreach (var p in UnexploredPoints)
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Item locations:");
            foreach (var kvp in ItemLocations)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
            }
            Console.WriteLine();
        }
    }
}
