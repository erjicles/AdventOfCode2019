using AdventOfCode2019.Challenges.Day18;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day18Test
    {
        [Fact]
        public void GetShortestPathToCollectAllKeysTest()
        {
            // Test examples taken from here:
            //https://adventofcode.com/2019/day/18
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                // Collecting every key took a total of 8 steps:
                // #########
                // #b.A.@.a#
                // #########
                new Tuple<string[], int>(
                    new string[]
                    {
                        "#########",
                        "#b.A.@.a#",
                        "#########"
                    }, 8),

                // 86 steps:
                // ########################
                // #f.D.E.e.C.b.A.@.a.B.c.#
                // ######################.#
                // #d.....................#
                // ########################
                new Tuple<string[], int>(
                    new string[]
                    {
                        "########################",
                        "#f.D.E.e.C.b.A.@.a.B.c.#",
                        "######################.#",
                        "#d.....................#",
                        "########################"
                    }, 86),

                // 132 steps:
                // ########################
                // #...............b.C.D.f#
                // #.######################
                // #.....@.a.B.c.d.A.e.F.g#
                // ########################
                new Tuple<string[], int>(
                    new string[]
                    {
                        "########################",
                        "#...............b.C.D.f#",
                        "#.######################",
                        "#.....@.a.B.c.d.A.e.F.g#",
                        "########################"
                    }, 132),

                // 136 steps:
                // #################
                // #i.G..c...e..H.p#
                // ########.########
                // #j.A..b...f..D.o#
                // ########@########
                // #k.E..a...g..B.n#
                // ########.########
                // #l.F..d...h..C.m#
                // #################
                new Tuple<string[], int>(
                    new string[]
                    {
                        "#################",
                        "#i.G..c...e..H.p#",
                        "########.########",
                        "#j.A..b...f..D.o#",
                        "########@########",
                        "#k.E..a...g..B.n#",
                        "########.########",
                        "#l.F..d...h..C.m#",
                        "#################"
                    }, 136),

                // 81 steps:
                // ########################
                // #@..............ac.GI.b#
                // ###d#e#f################
                // ###A#B#C################
                // ###g#h#i################
                // ########################
                new Tuple<string[], int>(
                    new string[]
                    {
                        "########################",
                        "#@..............ac.GI.b#",
                        "###d#e#f################",
                        "###A#B#C################",
                        "###g#h#i################",
                        "########################"
                    }, 81),
            });
            foreach (var testExample in testData)
            {
                var maze = new Maze(testExample.Item1);
                var result = Day18.GetShortestPathToCollectAllKeys(maze);
                Assert.Equal(testExample.Item2, result.TotalPathCost);
            }
        }

        [Fact]
        public void GetShortestPathToCollectAllKeysWithQuadRobotsTest()
        {
            // Test examples taken from here:
            //https://adventofcode.com/2019/day/18
            var testData = new List<Tuple<string[], int>>(new Tuple<string[], int>[] {
                // #######       #######
                // #a.#Cd#       #a.#Cd#
                // ##...##       ##@#@##
                // ##.@.##  -->  #######
                // ##...##       ##@#@##
                // #cB#Ab#       #cB#Ab#
                // #######       #######
                new Tuple<string[], int>(
                    new string[]
                    {
                        "#######",
                        "#a.#Cd#",
                        "##...##",
                        "##.@.##",
                        "##...##",
                        "#cB#Ab#",
                        "#######"
                    }, 8),

                // 24 steps:
                // ###############
                // #d.ABC.#.....a#
                // ######...######
                // ######.@.######
                // ######...######
                // #b.....#.....c#
                // ###############
                new Tuple<string[], int>(
                    new string[]
                    {
                        "###############",
                        "#d.ABC.#.....a#",
                        "######...######",
                        "######.@.######",
                        "######...######",
                        "#b.....#.....c#",
                        "###############"
                    }, 24),

                // 32 steps:
                // #############
                // #DcBa.#.GhKl#
                // #.###...#I###
                // #e#d#.@.#j#k#
                // ###C#...###J#
                // #fEbA.#.FgHi#
                // #############
                new Tuple<string[], int>(
                    new string[]
                    {
                        "#############",
                        "#DcBa.#.GhKl#",
                        "#.###...#I###",
                        "#e#d#.@.#j#k#",
                        "###C#...###J#",
                        "#fEbA.#.FgHi#",
                        "#############"
                    }, 32),

                // 72 steps:
                // #############
                // #g#f.D#..h#l#
                // #F###e#E###.#
                // #dCba...BcIJ#
                // #####.@.#####
                // #nK.L...G...#
                // #M###N#H###.#
                // #o#m..#i#jk.#
                // #############
                new Tuple<string[], int>(
                    new string[]
                    {
                        "#############",
                        "#g#f.D#..h#l#",
                        "#F###e#E###.#",
                        "#dCba...BcIJ#",
                        "#####.@.#####",
                        "#nK.L...G...#",
                        "#M###N#H###.#",
                        "#o#m..#i#jk.#",
                        "#############"
                    }, 72),
            });
            foreach (var testExample in testData)
            {
                var maze = new Maze(testExample.Item1, true);
                var result = Day18.GetShortestPathToCollectAllKeys(maze);
                Assert.Equal(testExample.Item2, result.TotalPathCost);
            }
        }

        [Fact]
        public void GetDay18Part1AnswerTest()
        {
            int expected = 3216;
            int actual = Day18.GetDay18Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay18Part2AnswerTest()
        {
            int expected = 1538;
            int actual = Day18.GetDay18Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
