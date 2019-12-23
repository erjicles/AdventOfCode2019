using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public class ConsoleInputProvider : IInputProvider
    {
        public int GetUserInput()
        {
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---->Please input an integer:");
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int result))
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
    }
}
