using AdventOfCode2019.Challenges.Day17;
using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day17Test
    {
        [Fact]
        public void GetScaffoldCellsTest()
        {
            // ..#..........
            // ..#..........
            // #######...###
            // #.#...#...#.#
            // #############
            // ..#...#...#..
            // ..#####...^..
            //
            // -->
            //
            //
            // ..#..........
            // ..#..........
            // ##O####...###
            // #.#...#...#.#
            // ##O###O###O##
            // ..#...#...#..
            // ..#####...^..
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                new Tuple<string[], int>(
                    new string[] 
                    {
                        "..#..........",
                        "..#..........",
                        "#######...###",
                        "#.#...#...#.#",
                        "#############",
                        "..#...#...#..",
                        "..#####...^.."
                    }, 4)
            });
            foreach (var testExample in testData)
            {
                var scaffoldMap = Day17.ProcessScan(testExample.Item1);
                var scaffoldCells = Day17.GetScaffoldCells(scaffoldMap);
                var scaffoldIntersections = Day17.GetScaffoldIntersections(scaffoldCells);
                Assert.Equal(testExample.Item2, scaffoldIntersections.Count);
            }
        }

        [Fact]
        public void GetCameraCalibrationNumberTest()
        {
            // Test data taken from here:
            // https://adventofcode.com/2019/day/17
            //
            // ..#..........
            // ..#..........
            // #######...###
            // #.#...#...#.#
            // #############
            // ..#...#...#..
            // ..#####...^..
            //
            // -->
            //
            //
            // ..#..........
            // ..#..........
            // ##O####...###
            // #.#...#...#.#
            // ##O###O###O##
            // ..#...#...#..
            // ..#####...^..
            //
            // For these intersections:
            // The top-left intersection is 2 units from the left of the image and 2 units from the top of the image, so its alignment parameter is 2 * 2 = 4.
            // The bottom-left intersection is 2 units from the left and 4 units from the top, so its alignment parameter is 2 * 4 = 8.
            // The bottom-middle intersection is 6 from the left and 4 from the top, so its alignment parameter is 24.
            // The bottom-right intersection's alignment parameter is 40.
            // To calibrate the cameras, you need the sum of the alignment parameters.In the above example, this is 76.
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                new Tuple<string[], int>(
                    new string[]
                    {
                        "..#..........",
                        "..#..........",
                        "#######...###",
                        "#.#...#...#.#",
                        "#############",
                        "..#...#...#..",
                        "..#####...^.."
                    }, 76)
            });
            foreach (var testExample in testData)
            {
                var scaffoldMap = Day17.ProcessScan(testExample.Item1);
                var scaffoldCells = Day17.GetScaffoldCells(scaffoldMap);
                var scaffoldIntersections = Day17.GetScaffoldIntersections(scaffoldCells);
                var calibrationNumber = Day17.GetCameraCalibrationNumber(scaffoldIntersections);
                Assert.Equal(testExample.Item2, calibrationNumber);
            }
        }

        [Fact]
        public void GetDay17Part1AnswerTest()
        {
            int expected = 8928;
            int actual = Day17.GetDay17Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
