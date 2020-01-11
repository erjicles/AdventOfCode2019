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
        public void GetDay18Part1AnswerTest()
        {
            int expected = 3216;
            int actual = Day18.GetDay18Part1Answer();
            Assert.Equal(expected, actual);
        }
    }
}
