using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AdventOfCode2019.IO
{
    /// <summary>
    /// Provides a way to call Console.ReadLine or Console.Read with a timeout.
    /// </summary>
    public class GameConsoleReader
    {
        // Code found here:
        // https://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;

        static GameConsoleReader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                var keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    input = "-1";
                    gotInput.Set();
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    input = "1";
                    gotInput.Set();
                }
            }
        }

        // omit the parameter to read a line without a timeout
        public static bool TryReadKey(out string line, int timeOutMillisecs = Timeout.Infinite)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                line = input;
            else
                line = null;
            return success;
        }
    }
}
