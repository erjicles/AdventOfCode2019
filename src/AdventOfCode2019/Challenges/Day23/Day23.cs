using AdventOfCode2019.Intcode;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day23
{
    /// <summary>
    /// Solution to the Day 23 challenge:
    /// https://adventofcode.com/2019/day/23
    /// </summary>
    public class Day23
    {
        public const string FILE_NAME = "Day23Input.txt";
        public static BigInteger GetDay23Part1Answer()
        {
            // Answer: 24602
            var program = GetDay23Input();
            var computers = new Dictionary<int, IntcodeComputer>();
            var programStatuses = new Dictionary<int, IntcodeProgramStatus>();
            var inputProviders = new Dictionary<int, BufferedInputProvider>();
            var outputListeners = new Dictionary<int, ListOutputListener>();
            var outputListenerAddress = new Dictionary<int, int>();

            for (int computerAddress = 0; computerAddress < 50; computerAddress++)
            {
                var inputProvider = new BufferedInputProvider();
                inputProvider.AddInputValue(computerAddress);
                var outputListener = new ListOutputListener();
                var computer = new IntcodeComputer(inputProvider, outputListener);
                computer.LoadProgram(program);
                computers.Add(computerAddress, computer);
                programStatuses.Add(computerAddress, IntcodeProgramStatus.Running);
                inputProviders.Add(computerAddress, inputProvider);
                outputListeners.Add(computerAddress, outputListener);
                outputListenerAddress.Add(computerAddress, 0);
            }
            for (int loopCount = 1; ;loopCount++)
            {
                for (int computerAddress = 0; computerAddress < 50; computerAddress++)
                {
                    var computer = computers[computerAddress];
                    var currentStatus = programStatuses[computerAddress];
                    if (IntcodeProgramStatus.AwaitingInput.Equals(currentStatus)
                        && !inputProviders[computerAddress].HasInput())
                    {
                        inputProviders[computerAddress].AddInputValue(-1);
                    }
                    var status = computer.RunProgram();
                    programStatuses[computerAddress] = status;

                    // Process output
                    var outputListener = outputListeners[computerAddress];
                    var currentOutputListenerAddress = outputListenerAddress[computerAddress];
                    if (currentOutputListenerAddress+2 < outputListener.Values.Count)
                    {
                        var targetAddress = outputListener.Values[currentOutputListenerAddress];
                        var X = outputListener.Values[currentOutputListenerAddress + 1];
                        var Y = outputListener.Values[currentOutputListenerAddress + 2];
                        outputListenerAddress[computerAddress] += 3;
                        if (targetAddress > 49)
                        {
                            if (targetAddress == 255)
                            {
                                return Y;
                            }
                            else
                            {
                                throw new Exception($"targetAddress out of range: {targetAddress}");
                            }
                        }
                        else
                        {
                            var targetInputProvider = inputProviders[(int)targetAddress];
                            targetInputProvider.AddInputValue(X);
                            targetInputProvider.AddInputValue(Y);
                        }
                    }
                }
            }
        }

        public static BigInteger[] GetDay23Input()
        {
            var filePath = FileHelper.GetInputFilePath(FILE_NAME);
            return IntcodeComputer.ReadProgramFromFile(filePath);
        }
    }
}
