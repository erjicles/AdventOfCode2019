using AdventOfCode2019.Grid;
using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day12
{
    /// <summary>
    /// Solution to the Day 12 challenge:
    /// https://adventofcode.com/2019/day/12
    /// </summary>
    public class Day12
    {
        public const string FILE_NAME = "Day12Input.txt";

        public static int GetDay12Part1Answer()
        {
            // What is the total energy in the system after simulating the 
            // moons given in your scan for 1000 steps?
            // Answer: 5937
            string[] input = GetDay12Input();
            IList<Moon> moons = ProcessMoonScanResults(input);
            EvolveMoons(moons, 1000);
            int totalEnergy = GetTotalEnergyOfSystem(moons);
            return totalEnergy;
        }

        public static BigInteger GetDay12Part2Answer()
        {
            // How many steps does it take to reach the first state that 
            // exactly matches a previous state?
            // Answer: 376203951569712
            string[] input = GetDay12Input();
            IList<Moon> moons = ProcessMoonScanResults(input);
            BigInteger result = EvolveMoonsUntilStateIsRepeated(moons);
            return result;
        }

        public static int GetTotalEnergyOfSystem(IList<Moon> moons)
        {
            int totalEnergy = 0;
            foreach (var moon in moons)
            {
                totalEnergy += moon.GetTotalEnergy();
            }
            return totalEnergy;
        }

        public static BigInteger EvolveMoonsUntilStateIsRepeated(IList<Moon> moons)
        {
            // Since each state has a unique predecessor, the first state
            // that will be repeated is the initial state.
            // Also, since the coordinates are independent of each other,
            // we only need to see how long it takes for the X coordinate to
            // cycle, then the Y coordinate to cycle, and the Z coordinate to
            // cycle, and then find the LCM of the 3 cycle step counts.
            long totalStepsX = EvolveMoonsUntilStateIsRepeated(moons, 0);
            long totalStepsY = EvolveMoonsUntilStateIsRepeated(moons, 1);
            long totalStepsZ = EvolveMoonsUntilStateIsRepeated(moons, 2);
            var gcdXY = BigInteger.GreatestCommonDivisor(totalStepsX, totalStepsY);
            var lcmXY = (totalStepsX * totalStepsY) / gcdXY;
            var gcdXYZ = BigInteger.GreatestCommonDivisor(lcmXY, totalStepsZ);
            var lcmXYZ = (lcmXY * totalStepsZ) / gcdXYZ;
            return lcmXYZ;
        }

        public static long EvolveMoonsUntilStateIsRepeated(IList<Moon> moons, int coordinate)
        {
            if (coordinate < 0 || coordinate > 2)
                throw new Exception($"Invalid coordinate {coordinate}");
            var hasher = MD5.Create();
            long timeStep = 0;
            var initialPositionHash = GetSystemMD5Hash(hasher, moons, coordinate);
            while(true)
            {
                if (timeStep > 0)
                {
                    var systemHash = GetSystemMD5Hash(hasher, moons, coordinate);
                    if (initialPositionHash.Contains(systemHash))
                        break;
                }
                EvolveMoons(moons, 1);
                timeStep++;
            }
            return timeStep;
        }

        public static string GetSystemMD5Hash(MD5 hasher, IList<Moon> moons, int coordinate)
        {
            var systemString = GetSystemString(moons, coordinate);
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(systemString));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string GetSystemString(IList<Moon> moons, int coordinate)
        {
            var sb = new StringBuilder();
            foreach (var moon in moons)
            {
                if (sb.Length > 0)
                    sb.Append(";");
                sb.Append(moon.GetCoordinateString(coordinate));
            }
            return sb.ToString();
        }

        public static void EvolveMoons(IList<Moon> moons, int numberOfTimeSteps)
        {
            for (int timeStep = 0; timeStep < numberOfTimeSteps; timeStep++)
            {
                // Apply gravity to velocities
                // For each pair of moons, update velocities:
                // To apply gravity, consider every pair of moons. On each 
                // axis (x, y, and z), the velocity of each moon changes by 
                // exactly +1 or -1 to pull the moons together. For example, 
                // if Ganymede has an x position of 3, and Callisto has a x 
                // position of 5, then Ganymede's x velocity changes by +1 
                // (because 5 > 3) and Callisto's x velocity changes by -1 
                // (because 3 < 5). However, if the positions on a given axis 
                // are the same, the velocity on that axis does not change for 
                // that pair of moons.
                for (int firstMoonIndex = 0; firstMoonIndex < moons.Count; firstMoonIndex++)
                {
                    for (int secondMoonIndex = firstMoonIndex + 1; secondMoonIndex < moons.Count; secondMoonIndex++)
                    {
                        var moon1 = moons[firstMoonIndex];
                        var moon2 = moons[secondMoonIndex];
                        ApplyGravity(moon1, moon2);
                    }
                }

                // Apply velocities to positions
                // Once all gravity has been applied, apply velocity: simply 
                // add the velocity of each moon to its own position.For 
                // example, if Europa has a position of x = 1, y = 2, z = 3 
                // and a velocity of x = -2, y = 0,z = 3, then its new 
                // position would be x = -1, y = 2, z = 6.This process does 
                // not modify the velocity of any moon.
                foreach (var moon in moons)
                {
                    moon.UpdatePosition();
                }
            }
        }

        public static void ApplyGravity(Moon moon1, Moon moon2)
        {
            int aX = GetAccelerationForCoordinate(moon1.Position.X, moon2.Position.X);
            int aY = GetAccelerationForCoordinate(moon1.Position.Y, moon2.Position.Y);
            int aZ = GetAccelerationForCoordinate(moon1.Position.Z, moon2.Position.Z);

            moon1.UpdateVelocity(new Tuple<int, int, int>(aX, aY, aZ));
            moon2.UpdateVelocity(new Tuple<int, int, int>(-aX, -aY, -aZ));
        }

        public static int GetAccelerationForCoordinate(int c1, int c2)
        {
            if (c1 < c2)
                return 1;
            else if (c1 > c2)
                return -1;
            return 0;
        }

        public static IList<Moon> ProcessMoonScanResults(string[] moonScanResults)
        {
            var result = new List<Moon>();
            foreach (var moonScanResult in moonScanResults)
            {
                var matchResult = Regex.Match(moonScanResult, @"^\<x=(-?\d+),\s*y=(-?\d+),\s*z=(-?\d+)\>$");
                if (!matchResult.Success)
                    throw new Exception($"Invalid moon scan format: {moonScanResult}");
                int x = int.Parse(matchResult.Groups[1].Value);
                int y = int.Parse(matchResult.Groups[2].Value);
                int z = int.Parse(matchResult.Groups[3].Value);
                var moon = new Moon(x, y, z);
                result.Add(moon);
            }
            return result;
        }

        public static string[] GetDay12Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME).ToArray();
        }
    }
}
