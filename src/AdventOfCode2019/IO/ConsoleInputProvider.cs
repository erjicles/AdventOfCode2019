using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class ConsoleInputProvider : IInputProvider
    {
        public BigInteger GetInput()
        {
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---->Please input an integer:");
                var userInput = Console.ReadLine();
                if (BigInteger.TryParse(userInput, out BigInteger result))
                {
                    isValidInput = true;
                    return result;
                }
                else
                {
                    Console.WriteLine($"Invalid user input: {userInput}");
                }
            }
            throw new Exception("This code should never be reached");
        }

        public bool HasInput()
        {
            return true;
        }
    }
}
