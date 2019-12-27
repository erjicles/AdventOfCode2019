using AdventOfCode2019.Challenges.Day10;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day10Test
    {
        [Fact]
        public void GetObjectThatSeesMostOtherObjectsTest()
        {
            // Tests taken from here:
            // https://adventofcode.com/2019/day/10
            var testData = new List<Tuple<string[], SolarGridPoint, int>>(new Tuple<string[], SolarGridPoint, int>[] {
                // The best location for a new monitoring station on this map is 
                // the highlighted asteroid at 3,4 because it can detect 8 
                // asteroids, more than any other location. (The only asteroid it 
                // cannot detect is the one at 1,0; its view of this asteroid is 
                // blocked by the asteroid at 2,2.) All other asteroids are worse 
                // locations; they can detect 7 or fewer other asteroids. 
                // .#..#
                //  .....
                //  #####
                //  ....#
                //  ...##
                new Tuple<string[], SolarGridPoint, int>(
                    new string[] 
                    { 
                        ".#..#",
                        ".....",
                        "#####",
                        "....#",
                        "...##"
                    },
                    new SolarGridPoint(3,4), 8),

                // Best is 5,8 with 33 other asteroids detected:
                //......#.#.
                //#..#.#....
                //..#######.
                //.#.#.###..
                //.#..#.....
                //..#....#.#
                //#..#....#.
                //.##.#..###
                //##...#..#.
                //.#....####
                new Tuple<string[], SolarGridPoint, int>(
                    new string[]
                    {
                        "......#.#.",
                        "#..#.#....",
                        "..#######.",
                        ".#.#.###..",
                        ".#..#.....",
                        "..#....#.#",
                        "#..#....#.",
                        ".##.#..###",
                        "##...#..#.",
                        ".#....####"
                    },
                    new SolarGridPoint(5,8), 33),

                // Best is 1,2 with 35 other asteroids detected:
                //#.#...#.#.
                //.###....#.
                //.#....#...
                //##.#.#.#.#
                //....#.#.#.
                //.##..###.#
                //..#...##..
                //..##....##
                //......#...
                //.####.###.
                new Tuple<string[], SolarGridPoint, int>(
                    new string[]
                    {
                        "#.#...#.#.",
                        ".###....#.",
                        ".#....#...",
                        "##.#.#.#.#",
                        "....#.#.#.",
                        ".##..###.#",
                        "..#...##..",
                        "..##....##",
                        "......#...",
                        ".####.###."
                    },
                    new SolarGridPoint(1,2), 35),

                // Best is 6,3 with 41 other asteroids detected:
                //.#..#..###
                //####.###.#
                //....###.#.
                //..###.##.#
                //##.##.#.#.
                //....###..#
                //..#.#..#.#
                //#..#.#.###
                //.##...##.#
                //.....#.#..
                new Tuple<string[], SolarGridPoint, int>(
                    new string[]
                    {
                        ".#..#..###",
                        "####.###.#",
                        "....###.#.",
                        "..###.##.#",
                        "##.##.#.#.",
                        "....###..#",
                        "..#.#..#.#",
                        "#..#.#.###",
                        ".##...##.#",
                        ".....#.#.."
                    },
                    new SolarGridPoint(6,3), 41),

                // Best is 11,13 with 210 other asteroids detected:
                //.#..##.###...#######
                //##.############..##.
                //.#.######.########.#
                //.###.#######.####.#.
                //#####.##.#.##.###.##
                //..#####..#.#########
                //####################
                //#.####....###.#.#.##
                //##.#################
                //#####.##.###..####..
                //..######..##.#######
                //####.##.####...##..#
                //.#####..#.######.###
                //##...#.##########...
                //#.##########.#######
                //.####.#.###.###.#.##
                //....##.##.###..#####
                //.#.#.###########.###
                //#.#.#.#####.####.###
                //###.##.####.##.#..##
                new Tuple<string[], SolarGridPoint, int>(
                    new string[]
                    {
                        ".#..##.###...#######",
                        "##.############..##.",
                        ".#.######.########.#",
                        ".###.#######.####.#.",
                        "#####.##.#.##.###.##",
                        "..#####..#.#########",
                        "####################",
                        "#.####....###.#.#.##",
                        "##.#################",
                        "#####.##.###..####..",
                        "..######..##.#######",
                        "####.##.####...##..#",
                        ".#####..#.######.###",
                        "##...#.##########...",
                        "#.##########.#######",
                        ".####.#.###.###.#.##",
                        "....##.##.###..#####",
                        ".#.#.###########.###",
                        "#.#.#.#####.####.###",
                        "###.##.####.##.#..##",
                    },
                    new SolarGridPoint(11,13), 210),
            });

            foreach (var testExample in testData)
            {
                var map = new SolarSystemMap(testExample.Item1);
                var result = map.GetObjectThatSeesMostOtherObjects();
                Assert.Equal(testExample.Item2, result.Item1.GridPoint);
                Assert.Equal(testExample.Item3, result.Item2);
            }
        }

        [Fact]
        public void GetNthAsteroidVaporizedTest()
        {
            // Tests taken from here:
            // https://adventofcode.com/2019/day/10
            var testData = new List<Tuple<string[], SolarGridPoint, int, SolarGridPoint>>(new Tuple<string[], SolarGridPoint, int, SolarGridPoint>[] {
                // In the large example above (the one with the best monitoring station location at 11,13):
                // The 1st asteroid to be vaporized is at 11,12.
                // The 2nd asteroid to be vaporized is at 12,1.
                // The 3rd asteroid to be vaporized is at 12,2.
                // The 10th asteroid to be vaporized is at 12,8.
                // The 20th asteroid to be vaporized is at 16,0.
                // The 50th asteroid to be vaporized is at 16,9.
                // The 100th asteroid to be vaporized is at 10,16.
                // The 199th asteroid to be vaporized is at 9,6.
                // The 200th asteroid to be vaporized is at 8,2.
                // The 201st asteroid to be vaporized is at 10,9.
                // The 299th and final asteroid to be vaporized is at 11,1.
                //.#..##.###...#######
                //##.############..##.
                //.#.######.########.#
                //.###.#######.####.#.
                //#####.##.#.##.###.##
                //..#####..#.#########
                //####################
                //#.####....###.#.#.##
                //##.#################
                //#####.##.###..####..
                //..######..##.#######
                //####.##.####...##..#
                //.#####..#.######.###
                //##...#.##########...
                //#.##########.#######
                //.####.#.###.###.#.##
                //....##.##.###..#####
                //.#.#.###########.###
                //#.#.#.#####.####.###
                //###.##.####.##.#..##
                new Tuple<string[], SolarGridPoint, int, SolarGridPoint>(
                    new string[]
                    {
                        ".#..##.###...#######",
                        "##.############..##.",
                        ".#.######.########.#",
                        ".###.#######.####.#.",
                        "#####.##.#.##.###.##",
                        "..#####..#.#########",
                        "####################",
                        "#.####....###.#.#.##",
                        "##.#################",
                        "#####.##.###..####..",
                        "..######..##.#######",
                        "####.##.####...##..#",
                        ".#####..#.######.###",
                        "##...#.##########...",
                        "#.##########.#######",
                        ".####.#.###.###.#.##",
                        "....##.##.###..#####",
                        ".#.#.###########.###",
                        "#.#.#.#####.####.###",
                        "###.##.####.##.#..##",
                    },
                    new SolarGridPoint(11,13), 200, new SolarGridPoint(8, 2)),
            });

            foreach (var testExample in testData)
            {
                var map = new SolarSystemMap(testExample.Item1);
                var result = map.GetNthAsteroidVaporized(testExample.Item2, testExample.Item3);
                Assert.Equal(testExample.Item4, result.GridPoint);
            }
        }

        [Fact]
        public void GetDay10Part1AnswerTest()
        {
            int expected = 282;
            int actual = Day10.GetDay10Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay10Part2AnswerTest()
        {
            int expected = 1008;
            int actual = Day10.GetDay10Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
