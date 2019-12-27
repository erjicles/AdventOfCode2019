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
        public void GetDay10Part1AnswerTest()
        {
            int expected = 282;
            int actual = Day10.GetDay10Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
