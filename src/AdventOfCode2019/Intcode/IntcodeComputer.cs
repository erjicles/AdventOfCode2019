using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public class IntcodeComputer
    {
        private readonly IInputProvider _inputProvider;
        private readonly IOutputListener _outputListener;
        private IList<BigInteger> _program;
        private int _position;
        private int _relativeBase = 0;
        public bool IsDebugMode { get; set; } = false;
        public IntcodeComputer()
        {
            _inputProvider = new ConsoleInputProvider();
            _outputListener = new ConsoleOutputListener();
        }

        public IntcodeComputer(IInputProvider inputProvider, IOutputListener outputListener)
        {
            _inputProvider = inputProvider;
            _outputListener = outputListener;
        }

        public void LoadProgram(IList<BigInteger> inputProgram)
        {
            LoadProgram(inputProgram, 0);
        }

        public void LoadProgram(int[] inputProgram)
        {
            var program = inputProgram.Select(d => (BigInteger)d).ToList();
            LoadProgram(program, 0);
        }

        public void LoadProgram(IList<BigInteger> inputProgram, int initialPosition)
        {
            LoadProgram(inputProgram, initialPosition, 0);
        }

        public void LoadProgram(IList<BigInteger> inputProgram, int initialPosition, int initialRelativeBase)
        {
            _program = inputProgram.ToList();
            _position = initialPosition;
            _relativeBase = initialRelativeBase;
        }

        public void SetPosition(int position)
        {
            _position = position;
        }

        public IList<BigInteger> GetProgramCopy()
        {
            if (_program == null)
                return null;
            return _program.ToList();
        }

        public IntcodeProgramStatus RunProgram()
        {
            IntcodeProgramStatus status = IntcodeProgramStatus.Running;
            while (true)
            {
                LogDebugMessage($"Pos: {_position.ToString("0000")}, Cmd: {_program[_position]}");
                var parsedCommand = ParseCommand(_program[_position]);
                var opcode = parsedCommand[0];
                if (opcode == 1)
                {
                    // Add param1 + param2, store in address pointed to by param3
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    var val3 = GetParameterWritePosition(_position + 3, 3, parsedCommand);
                    SetMemoryValue(val3, val1 + val2);
                    _position += 4;
                }
                else if (opcode == 2)
                {
                    // Multiply param1 * param2, store in address pointed to by param3
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    var val3 = GetParameterWritePosition(_position + 3, 3, parsedCommand);
                    SetMemoryValue(val3, val1 * val2);
                    _position += 4;
                }
                else if (opcode == 3)
                {
                    // Take user input, and store in the parameter location
                    // If the input provider doesn't have any input,
                    // then pause the program and return awaiting input status
                    if (!_inputProvider.HasInput())
                    {
                        status = IntcodeProgramStatus.AwaitingInput;
                        break;
                    }
                    BigInteger input = _inputProvider.GetInput();
                    var val1 = GetParameterWritePosition(_position + 1, 1, parsedCommand);
                    SetMemoryValue(val1, input);
                    _position += 2;
                }
                else if (opcode == 4)
                {
                    // Output a value
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    _outputListener.SendOutput(val1);
                    _position += 2;
                }
                else if (opcode == 5)
                {
                    // Opcode 5 is jump-if-true: if the first parameter is 
                    // non-zero, it sets the instruction pointer to the value 
                    // from the second parameter. Otherwise, it does nothing.
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    if (val1 != 0)
                    {
                        _position = GetMemoryAddress(val2);
                    }
                    else
                    {
                        _position += 3;
                    }
                }
                else if (opcode == 6)
                {
                    // Opcode 6 is jump-if-false: if the first parameter is 
                    // zero, it sets the instruction pointer to the value from 
                    // the second parameter. Otherwise, it does nothing.
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    if (val1 == 0)
                    {
                        _position = GetMemoryAddress(val2);
                    }
                    else
                    {
                        _position += 3;
                    }
                }
                else if (opcode == 7)
                {
                    // Opcode 7 is less than: if the first parameter is less 
                    // than the second parameter, it stores 1 in the position 
                    // given by the third parameter. 
                    // Otherwise, it stores 0.
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    var val3 = GetParameterWritePosition(_position + 3, 3, parsedCommand);
                    var valToStore = val1 < val2 ? 1 : 0;
                    SetMemoryValue(val3, valToStore);
                    _position += 4;
                }
                else if (opcode == 8)
                {
                    // Opcode 8 is equals: if the first parameter is equal to 
                    // the second parameter, it stores 1 in the position given 
                    // by the third parameter.
                    // Otherwise, it stores 0.
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    var val2 = GetParameterValue(_position + 2, 2, parsedCommand);
                    var val3 = GetParameterWritePosition(_position + 3, 3, parsedCommand);
                    var valToStore = val1 == val2 ? 1 : 0;
                    SetMemoryValue(val3, valToStore);
                    _position += 4;
                }
                else if (opcode == 9)
                {
                    // Opcode 9 adjusts the relative base by the value of its 
                    // only parameter. The relative base increases (or 
                    // decreases, if the value is negative) by the value of the 
                    // parameter.
                    // For example, if the relative base is 2000, then after 
                    // the instruction 109,19, the relative base would be 2019. 
                    // If the next instruction were 204,-34, then the value at 
                    // address 1985 would be output.
                    var val1 = GetParameterValue(_position + 1, 1, parsedCommand);
                    _relativeBase += GetMemoryAddress(val1);
                    _position += 2;
                }
                else if (opcode == 99)
                {
                    status = IntcodeProgramStatus.Completed;
                    break;
                }
                else if (opcode != 99)
                {
                    throw new Exception($"Invalid opcode {_program[_position]} at position {_position}");
                }
            }
            return status;
        }

        /// <summary>
        /// Parses a command (containing the opcode and parameter modes), 
        /// and returns these separately as an array.
        /// The first element of the array is the opcode
        /// The subsequent elements are the parameter modes for the parameters.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static BigInteger[] ParseCommand(BigInteger command)
        {
            var commandString = command.ToString();
            if (commandString.Length == 1
                || commandString.Length == 2)
            {
                return new BigInteger[] { command };
            }
            else if (commandString.Length > 2)
            {
                // Parameter modes are stored in the same value as the 
                // instruction's opcode. The opcode is a two-digit number 
                // based only on the ones and tens digit of the value, that is, 
                // the opcode is the rightmost two digits of the first value in 
                // an instruction. Parameter modes are single digits, one per 
                // parameter, read right-to-left from the opcode: the first 
                // parameter's mode is in the hundreds digit, the second 
                // parameter's mode is in the thousands digit, the third 
                // parameter's mode is in the ten-thousands digit, and so on. 
                // Any missing modes are 0.
                var result = new List<BigInteger>();
                result.Add(int.Parse(commandString.Substring(commandString.Length - 2, 2)));
                for (int i = commandString.Length - 3; i >= 0; i--)
                {
                    result.Add(int.Parse(commandString[i].ToString()));
                }
                return result.ToArray();
            }
            throw new Exception($"Invalid command {command}");
        }

        public static BigInteger GetParameterMode(int parameterNumber, BigInteger[] parsedCommand)
        {
            if (parameterNumber >= parsedCommand.Length)
                return 0;
            return parsedCommand[parameterNumber];
        }

        public BigInteger GetParameterWritePosition(
            int parameterIndex,
            int parameterNumber,
            BigInteger[] parsedCommand)
        {
            var parameterMode = GetParameterMode(parameterNumber, parsedCommand);
            if (parameterMode == 1)
                throw new Exception("Instruction write parameter found in immediate mode");
            if (parameterMode == 0)
            {
                return _program[parameterIndex];
            }
            else if (parameterMode == 2)
            {
                return _relativeBase + _program[parameterIndex];
            }
            else
            {
                throw new Exception($"Invalid parameter mode {parameterMode}");
            }
        }

        public BigInteger GetParameterValue(
            int parameterIndex,
            int parameterNumber,
            BigInteger[] parsedCommand)
        {
            // Each parameter of an instruction is handled based on its 
            // parameter mode. Right now, your ship computer already 
            // understands parameter mode 0, position mode, which causes the 
            // parameter to be interpreted as a position - if the parameter is 
            // 50, its value is the value stored at address 50 in memory. 
            // Until now, all parameters have been in position mode.

            // Now, your ship computer will also need to handle parameters in 
            // mode 1, immediate mode. In immediate mode, a parameter is 
            // interpreted as a value - if the parameter is 50, its value is 
            // simply 50.

            // Your existing Intcode computer is missing one key feature: it 
            // needs support for parameters in relative mode.
            // Parameters in mode 2, relative mode, behave very similarly to 
            // parameters in position mode: the parameter is interpreted as a 
            // position.Like position mode, parameters in relative mode can be 
            // read from or written to.
            // The important difference is that relative mode parameters don't 
            // count from address 0. Instead, they count from a value called 
            // the relative base. The relative base starts at 0.
            // The address a relative mode parameter refers to is itself plus 
            // the current relative base.When the relative base is 0, relative 
            // mode parameters and position mode parameters with the same value 
            // refer to the same address.
            // For example, given a relative base of 50, a relative mode 
            // parameter of - 7 refers to memory address 50 + -7 = 43.
            var parameterMode = GetParameterMode(parameterNumber, parsedCommand);
            var parameterValue = _program[parameterIndex];
            if (parameterMode == 0)
            {
                // Interpret as position
                return _program[GetMemoryAddress(parameterValue)];
            }
            else if (parameterMode == 1)
            {
                // Interpret as literal
                return parameterValue;
            }
            else if (parameterMode == 2)
            {
                // Interpret as relative position
                return _program[_relativeBase + GetMemoryAddress(parameterValue)];
            }
            else
            {
                throw new Exception($"Invalid parameter mode {parameterMode}");
            }
        }

        private void SetMemoryValue(BigInteger position, BigInteger value)
        {
            int memoryAddress = GetMemoryAddress(position);
            ExpandMemoryToFitAddress(memoryAddress);
            _program[memoryAddress] = value;
        }

        private int GetMemoryAddress(BigInteger memoryAddress)
        {
            if (memoryAddress > int.MaxValue)
                throw new Exception($"BigInteger being used as memory address {memoryAddress}");
            int position = (int)memoryAddress;
            ExpandMemoryToFitAddress(position);
            return position;
        }

        private void ExpandMemoryToFitAddress(int position)
        {
            if (position >= _program.Count)
            {
                for (int i = _program.Count; i <= position; i++)
                {
                    _program.Add(0);
                }
            }
        }

        // Todo: Log to external logger injected in constructor
        private void LogDebugMessage(object obj)
        {
            if (IsDebugMode)
            {
                Console.WriteLine($"...dbug: {obj}");
            }
        }

        public static BigInteger[] ReadProgramFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"Cannot locate file {filePath}");
            }
            var inputText = File.ReadAllText(filePath);
            return inputText.Split(",").Select(v => BigInteger.Parse(v)).ToArray();
        }
    }
}
