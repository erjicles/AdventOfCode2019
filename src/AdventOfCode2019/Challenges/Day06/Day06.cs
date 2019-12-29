using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
#nullable enable

namespace AdventOfCode2019.Challenges.Day06
{
    /// <summary>
    /// Solution to the Day 6 challenge:
    /// https://adventofcode.com/2019/day/6
    /// </summary>
    public class Day06
    {
        public const string FILE_NAME = "Day06Input.txt";

        public class MapEntry
        {
            public string ObjectName { get; set; } = "";
            public string ObjectThisObjectIsOrbitingName { get; set; } = "";
            public HashSet<string> ObjectsOrbitingThisObjectNames { get; set; } = new HashSet<string>();
            public MapEntry(string objectName)
            {
                ObjectName = objectName;
            }
        }

        public static int GetDay6Part1Answer()
        {
            // What is the total number of direct and indirect orbits in your map data?
            // Answer: 261306
            var input = GetDay6Input();
            var orbitMap = ConstructOrbitMap(input);
            var totalOrbits = GetTotalNumberOfOrbits(orbitMap);
            return totalOrbits;
        }

        public static int GetDay6Part2Answer()
        {
            // What is the minimum number of orbital transfers required to 
            // move from the object YOU are orbiting to the object SAN is 
            // orbiting? 
            // (Between the objects they are orbiting - not between YOU and SAN.)
            // Answer: 382
            var input = GetDay6Input();
            var orbitMap = ConstructOrbitMap(input);
            var totalTransfers = GetNumberOfOrbitalTransfers("YOU", "SAN", orbitMap);
            return totalTransfers;
        }

        /// <summary>
        /// Calculates the minimum number of orbital transfers required to
        /// move the source object from its current orbit to an orbit such
        /// that it's orbiting the same object as the targetObject
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="targetObject"></param>
        /// <param name="orbitalMap"></param>
        /// <returns></returns>
        public static int GetNumberOfOrbitalTransfers(
            string sourceObject,
            string targetObject,
            Dictionary<string, MapEntry> orbitalMap)
        {
            int result = 0;

            // Get the first common ancestor
            var sourceAncestryStack = new Dictionary<string, int>();
            var currentEntry = orbitalMap[sourceObject];
            var numberOfTransitions = 0;
            while (!String.IsNullOrWhiteSpace(currentEntry.ObjectThisObjectIsOrbitingName))
            {
                var centralEntry = orbitalMap[currentEntry.ObjectThisObjectIsOrbitingName];
                sourceAncestryStack.Add(centralEntry.ObjectName, numberOfTransitions);
                currentEntry = centralEntry;
                numberOfTransitions++;
            }
            currentEntry = orbitalMap[targetObject];
            numberOfTransitions = 0;
            while (!String.IsNullOrWhiteSpace(currentEntry.ObjectThisObjectIsOrbitingName))
            {
                var centralEntry = orbitalMap[currentEntry.ObjectThisObjectIsOrbitingName];
                if (sourceAncestryStack.ContainsKey(centralEntry.ObjectName))
                {
                    result = numberOfTransitions + sourceAncestryStack[centralEntry.ObjectName];
                    break;
                }
                currentEntry = centralEntry;
                numberOfTransitions++;
            }
            return result;
        }

        public static int GetTotalNumberOfOrbits(Dictionary<string, MapEntry> orbitalMap)
        {
            var result = 0;
            var objectsAlreadyVisited = new HashSet<string>();
            var objectStack = new Stack<Tuple<string, int>>();
            objectStack.Push(new Tuple<string, int>("COM", 0));
            while (objectStack.Count > 0)
            {
                var currentCentralObjectEntry = objectStack.Pop();
                if (objectsAlreadyVisited.Contains(currentCentralObjectEntry.Item1))
                {
                    continue;
                }

                // Process orbits for the current central object
                var currentCentralObject = currentCentralObjectEntry.Item1;
                var numberOfOrbitsForCurrentCentralObject = currentCentralObjectEntry.Item2;
                result += numberOfOrbitsForCurrentCentralObject;
                objectsAlreadyVisited.Add(currentCentralObject);

                // Add the orbiters of the current central object to the stack
                if (orbitalMap.ContainsKey(currentCentralObject))
                {
                    var orbiters = orbitalMap[currentCentralObject].ObjectsOrbitingThisObjectNames;
                    var numberOfOrbitsForOrbiters = numberOfOrbitsForCurrentCentralObject + 1;
                    foreach (var orbiter in orbiters)
                    {
                        if (!objectsAlreadyVisited.Contains(orbiter))
                        {
                            objectStack.Push(new Tuple<string, int>(orbiter, numberOfOrbitsForOrbiters));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Takes an input collection of map entries, and returns the orbital
        /// map, defined as a dictionary where the key is the central object
        /// and the value is the MapEntry object for that central object.
        /// </summary>
        /// <param name="mapEntries"></param>
        /// <returns></returns>
        public static Dictionary<string, MapEntry> ConstructOrbitMap(ICollection<string> mapEntries)
        {
            var result = new Dictionary<string, MapEntry>();
            foreach (var mapEntryString in mapEntries)
            {
                var mapEntry = ParseMapEntry(mapEntryString);
                var centralObjectName = mapEntry.Item1;
                var orbitingObjectName = mapEntry.Item2;

                // Add/update the central object map entry
                if (!result.ContainsKey(centralObjectName))
                {
                    var centralObjectEntry = new MapEntry(centralObjectName);
                    centralObjectEntry.ObjectsOrbitingThisObjectNames.Add(orbitingObjectName);
                    result.Add(centralObjectName, centralObjectEntry);
                }
                else
                {
                    var centralObjectEntry = result[centralObjectName];
                    if (!centralObjectEntry.ObjectsOrbitingThisObjectNames.Contains(orbitingObjectName))
                    {
                        centralObjectEntry.ObjectsOrbitingThisObjectNames.Add(orbitingObjectName);
                    }
                }

                // Add/update the orbiting object map entry
                if (!result.ContainsKey(orbitingObjectName))
                {
                    var orbitingMapEntry = new MapEntry(orbitingObjectName)
                    {
                        ObjectThisObjectIsOrbitingName = centralObjectName
                    };
                    result.Add(orbitingObjectName, orbitingMapEntry);
                }
                else
                {
                    // Set the orbiting object's central object
                    result[orbitingObjectName].ObjectThisObjectIsOrbitingName = centralObjectName;
                }
            }
            return result;
        }

        /// <summary>
        /// Takes an input map entry and returns the objects as a tuple.
        /// E.g., AAA)BBB -> (AAA, BBB)
        /// </summary>
        /// <param name="mapEntry"></param>
        /// <returns></returns>
        public static Tuple<string, string> ParseMapEntry(string mapEntry)
        {
            // Match patterns of the form:
            // <object orbited by>)<other object>
            var match = Regex.Match(mapEntry, @"^(\w+)\)(\w+)$");
            if (match.Success)
            {
                var centerObject = match.Groups[1].Value;
                var orbitingObject = match.Groups[2].Value;
                return new Tuple<string, string>(centerObject, orbitingObject);
            }
            throw new Exception($"Invalid map entry: {mapEntry}");
        }

        public static ICollection<string> GetDay6Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
