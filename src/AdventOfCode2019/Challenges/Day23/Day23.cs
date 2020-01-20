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
            // Boot up all 50 computers and attach them to your network. What 
            // is the Y value of the first packet sent to address 255?
            // Answer: 24602
            var result = RunNetwork(1);
            return result;
        }

        public static BigInteger GetDay23Part2Answer()
        {
            // Monitor packets released to the computer at address 0 by the 
            // NAT. What is the first Y value delivered by the NAT to the 
            // computer at address 0 twice in a row?
            // Answer: 19641
            var result = RunNetwork(2);
            return result;
        }

        public static BigInteger RunNetwork(int part)
        {
            var program = GetDay23Input();
            var computers = new Dictionary<int, IntcodeComputer>();
            var programStatuses = new Dictionary<int, IntcodeProgramStatus>();
            var inputProviders = new Dictionary<int, BufferedInputProvider>();
            var outputListeners = new Dictionary<int, ListOutputListener>();
            var outputListenerAddress = new Dictionary<int, int>();
            var natYValues = new Stack<BigInteger>();

            for (int computerAddress = 0; computerAddress < 256; computerAddress++)
            {
                if (computerAddress >= 50 && computerAddress < 255)
                    continue;
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

            for (int loopCount = 1; ; loopCount++)
            {
                bool isIdle = true;
                for (int computerAddress = 0; computerAddress < 50; computerAddress++)
                {
                    var computer = computers[computerAddress];
                    var currentStatus = programStatuses[computerAddress];
                    var inputProvider = inputProviders[computerAddress];
                    if (IntcodeProgramStatus.AwaitingInput.Equals(currentStatus))
                    {
                        if (inputProvider.HasInput())
                        {
                            isIdle = false;
                        }
                        else
                        {
                            inputProviders[computerAddress].AddInputValue(-1);
                        }
                    }
                    else if (IntcodeProgramStatus.Running.Equals(currentStatus))
                    {
                        isIdle = false;
                    }
                    var status = computer.RunProgram();
                    programStatuses[computerAddress] = status;

                    // Process output
                    var outputListener = outputListeners[computerAddress];
                    var currentOutputListenerAddress = outputListenerAddress[computerAddress];
                    while (currentOutputListenerAddress + 2 < outputListener.Values.Count)
                    {
                        isIdle = false;
                        var targetAddress = outputListener.Values[currentOutputListenerAddress];
                        var X = outputListener.Values[currentOutputListenerAddress + 1];
                        var Y = outputListener.Values[currentOutputListenerAddress + 2];
                        currentOutputListenerAddress += 3;
                        if (targetAddress < 50 || targetAddress == 255)
                        {
                            var targetInputProvider = inputProviders[(int)targetAddress];
                            targetInputProvider.AddInputValue(X);
                            targetInputProvider.AddInputValue(Y);
                            if (targetAddress == 255)
                            {
                                var targetOutputListener = outputListeners[255];
                                targetOutputListener.Values.Add(X);
                                targetOutputListener.Values.Add(Y);
                                outputListenerAddress[255] = targetOutputListener.Values.Count - 2;
                                if (part == 1)
                                {
                                    return Y;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"targetAddress out of range: {targetAddress}");
                        }
                    }
                    outputListenerAddress[computerAddress] = currentOutputListenerAddress;
                }
                if (isIdle)
                {
                    // Get the last packet sent to the NAT and send it to
                    // address 0
                    var outputListener = outputListeners[255];
                    var address = outputListenerAddress[255];
                    if (address + 1 >= outputListener.Values.Count)
                        throw new Exception("Invalid state");
                    var X = outputListener.Values[address];
                    var Y = outputListener.Values[address + 1];
                    inputProviders[0].AddInputValue(X);
                    inputProviders[0].AddInputValue(Y);
                    if (natYValues.Count > 0)
                    {
                        var previousYValue = natYValues.Peek();
                        if (part == 2 && Y == previousYValue)
                        {
                            return Y;
                        }
                    }
                    natYValues.Push(Y);
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
