using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019.IO
{
    public static class FileHelper
    {
        public static string GetInputFilePath(string inputFileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "InputData", inputFileName);
            return filePath;
        }
        public static string ReadInputFileAsString(string inputFileName)
        {
            var filePath = GetInputFilePath(inputFileName);
            var result = File.ReadAllText(filePath);
            return result;
        }
        public static IList<string> ReadInputFileLines(string inputFileName)
        {
            var filePath = GetInputFilePath(inputFileName);
            if (!File.Exists(filePath))
            {
                throw new Exception($"Cannot locate file {filePath}");
            }
            var result = File.ReadAllLines(filePath);
            return result;
        }
    }
}
