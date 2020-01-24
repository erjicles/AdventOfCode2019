using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ConsoleInputProvider : IInputProvider
    {
        public List<BigInteger> Values { get; } = new List<BigInteger>();
        private int _valueIndex = 0;
        public BigInteger GetInput()
        {
            if (Values == null || Values.Count == 0)
                throw new Exception("No values defined");
            if (_valueIndex >= Values.Count)
                throw new Exception("Not enough values in list");
            var currentValueIndex = _valueIndex;
            _valueIndex++;
            return Values[currentValueIndex];
        }
        public string AddInput(bool appendNewLine)
        {
            bool isValidInput = false;
            var userInputString = string.Empty;
            while (!isValidInput)
            {
                Console.WriteLine("---->Please enter your input:");
                var userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                    continue;
                if (BigInteger.TryParse(userInput, out BigInteger result))
                {
                    isValidInput = true;
                    Values.Add(result);
                }
                else
                {
                    var inputValues = new List<BigInteger>();
                    for (int i = 0; i < userInput.Length; i++)
                    {
                        inputValues.Add(char.ConvertToUtf32(userInput, i));
                    }
                    isValidInput = true;
                    Values.AddRange(inputValues);
                }
                if (isValidInput)
                    userInputString = userInput;
            }
            if (appendNewLine)
            {
                Values.Add(10);
                userInputString += char.ConvertFromUtf32(10);
            }
            return userInputString;
        }

        public bool HasInput()
        {
            if (Values == null || Values.Count == 0)
                return false;
            if (_valueIndex < Values.Count)
                return true;
            return false;
        }
    }
}
