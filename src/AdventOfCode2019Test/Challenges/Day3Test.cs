using AdventOfCode2019.Challenges.Day3;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day3Test
    {
        [Fact]
        public void GetClosestIntersectionToOriginManhattanDistanceTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/3
            // Ex 1:
            // R8,U5,L5,D3
            // U7,R6,D4,L4 = distance = 6
            // Ex 2:
            // R75,D30,R83,U83,L12,D49,R71,U7,L72
            // U62,R66,U55,R34,D71,R55,D58,R83 = distance 159
            // Ex 3:
            // R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
            // U98, R91, D20, R16, D67, R40, U7, R15, U6, R7 = distance 135
            var testData = new List<Tuple<string, string, int>>(new Tuple<string, string, int>[] {
                new Tuple<string, string, int>(
                    "R8,U5,L5,D3",
                    "U7,R6,D4,L4",
                    6
                    ),
                new Tuple<string, string, int>(
                    "R75,D30,R83,U83,L12,D49,R71,U7,L72",
                    "U62,R66,U55,R34,D71,R55,D58,R83",
                    159
                    ),
                new Tuple<string, string, int>(
                    "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                    "U98, R91, D20, R16, D67, R40, U7, R15, U6, R7",
                    135
                    )
            });

            foreach (var testExample in testData)
            {
                var pathInstructions1 = Day3.GetPathInstructionsFromString(testExample.Item1);
                var pathInstructions2 = Day3.GetPathInstructionsFromString(testExample.Item2);
                var path1 = Day3.GeneratePath(pathInstructions1);
                var path2 = Day3.GeneratePath(pathInstructions2);
                var result = Day3.GetClosestIntersectionToOriginManhattanDistance(path1, path2);
                Assert.Equal(testExample.Item3, result);
            }
        }

        [Fact]
        public void GetMinimalIntersectionTotalStepsTest()
        {
            // Test examples taken from https://adventofcode.com/2019/day/3#part2
            // Ex 1:
            // R8,U5,L5,D3
            // U7,R6,D4,L4 = distance = 6
            // Ex 2:
            // R75,D30,R83,U83,L12,D49,R71,U7,L72
            // U62,R66,U55,R34,D71,R55,D58,R83 = distance 159
            // Ex 3:
            // R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
            // U98, R91, D20, R16, D67, R40, U7, R15, U6, R7 = distance 135
            var testData = new List<Tuple<string, string, int>>(new Tuple<string, string, int>[] {
                new Tuple<string, string, int>(
                    "R8,U5,L5,D3",
                    "U7,R6,D4,L4",
                    30
                    ),
                new Tuple<string, string, int>(
                    "R75,D30,R83,U83,L12,D49,R71,U7,L72",
                    "U62,R66,U55,R34,D71,R55,D58,R83",
                    610
                    ),
                new Tuple<string, string, int>(
                    "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                    "U98, R91, D20, R16, D67, R40, U7, R15, U6, R7",
                    410
                    )
            });

            foreach (var testExample in testData)
            {
                var pathInstructions1 = Day3.GetPathInstructionsFromString(testExample.Item1);
                var pathInstructions2 = Day3.GetPathInstructionsFromString(testExample.Item2);
                var path1 = Day3.GeneratePath(pathInstructions1);
                var path2 = Day3.GeneratePath(pathInstructions2);
                var result = Day3.GetMinimalIntersectionTotalSteps(path1, path2);
                Assert.Equal(testExample.Item3, result);
            }
        }
    }
}
