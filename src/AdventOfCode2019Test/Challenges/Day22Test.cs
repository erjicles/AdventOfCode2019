using AdventOfCode2019.Challenges.Day22;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace AdventOfCode2019Test.Challenges
{
    public class Day22Test
    {
        [Fact]
        public void DealIntoNewStackTest()
        {
            // Item 1: Deck size
            // Item 2: Start card index
            // Item 3: Expected end card index
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<int, int, int>>()
            {
                // To deal into new stack, create a new stack of cards by 
                // dealing the top card of the deck onto the top of the new 
                // stack repeatedly until you run out of cards:
                //Top Bottom
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //                      New stack
                //
                //  1 2 3 4 5 6 7 8 9   Your deck
                //                  0   New stack
                //
                //    2 3 4 5 6 7 8 9   Your deck
                //                1 0   New stack
                //
                //      3 4 5 6 7 8 9   Your deck
                //              2 1 0   New stack
                //
                //Several steps later...
                //
                //                  9   Your deck
                //  8 7 6 5 4 3 2 1 0   New stack
                //
                //                      Your deck
                //9 8 7 6 5 4 3 2 1 0   New stack
                Tuple.Create(10, 0, 9),
                Tuple.Create(10, 1, 8),
                Tuple.Create(10, 2, 7),
                Tuple.Create(10, 3, 6),
                Tuple.Create(10, 4, 5),
                Tuple.Create(10, 5, 4),
                Tuple.Create(10, 6, 3),
                Tuple.Create(10, 7, 2),
                Tuple.Create(10, 8, 1),
                Tuple.Create(10, 9, 0),
            };
            foreach (var testExample in testData)
            {
                var result = ShuffleHelper
                    .GetShuffleFunctionDealIntoNewStack(deckSize: testExample.Item1)
                    .Evaluate(testExample.Item2);
                Assert.Equal(testExample.Item3, result);
            }
        }

        [Fact]
        public void CutNCardsTest()
        {
            // Item 1: Deck size
            // Item 2: Cut size
            // Item 3: Start card index
            // Item 4: Expected end card index
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<int, int, int, int>>()
            {
                // To cut N cards, take the top N cards off the top of the 
                // deck and move them as a single unit to the bottom of the 
                // deck, retaining their order. For example, to cut 3:
                //Top          Bottom
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //
                //      3 4 5 6 7 8 9   Your deck
                //0 1 2                 Cut cards
                //
                //3 4 5 6 7 8 9         Your deck
                //              0 1 2   Cut cards
                //
                //3 4 5 6 7 8 9 0 1 2   Your deck
                Tuple.Create(10, 3, 0, 7),
                Tuple.Create(10, 3, 1, 8),
                Tuple.Create(10, 3, 2, 9),
                Tuple.Create(10, 3, 3, 0),
                Tuple.Create(10, 3, 4, 1),
                Tuple.Create(10, 3, 5, 2),
                Tuple.Create(10, 3, 6, 3),
                Tuple.Create(10, 3, 7, 4),
                Tuple.Create(10, 3, 8, 5),
                Tuple.Create(10, 3, 9, 6),
                // You've also been getting pretty good at a version of this 
                // technique where N is negative! In that case, cut (the 
                // absolute value of) N cards from the bottom of the deck onto 
                // the top. For example, to cut -4:
                //Top          Bottom
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //
                //0 1 2 3 4 5           Your deck
                //            6 7 8 9   Cut cards
                //
                //        0 1 2 3 4 5   Your deck
                //6 7 8 9               Cut cards
                //
                //6 7 8 9 0 1 2 3 4 5   Your deck
                Tuple.Create(10, -4, 0, 4),
                Tuple.Create(10, -4, 1, 5),
                Tuple.Create(10, -4, 2, 6),
                Tuple.Create(10, -4, 3, 7),
                Tuple.Create(10, -4, 4, 8),
                Tuple.Create(10, -4, 5, 9),
                Tuple.Create(10, -4, 6, 0),
                Tuple.Create(10, -4, 7, 1),
                Tuple.Create(10, -4, 8, 2),
                Tuple.Create(10, -4, 9, 3),
            };

            foreach (var testExample in testData)
            {
                var result = ShuffleHelper.GetShuffleFunctionCutNCards(
                    n: testExample.Item2,
                    deckSize: testExample.Item1)
                    .Evaluate(testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void CutNCardsBackwardsTest()
        {
            // Item 1: Deck size
            // Item 2: Cut size
            // Item 3: Start card index
            // Item 4: Expected end card index
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<int, int, int, int>>()
            {
                // To cut N cards, take the top N cards off the top of the 
                // deck and move them as a single unit to the bottom of the 
                // deck, retaining their order. For example, to cut 3:
                //Top          Bottom
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //
                //      3 4 5 6 7 8 9   Your deck
                //0 1 2                 Cut cards
                //
                //3 4 5 6 7 8 9         Your deck
                //              0 1 2   Cut cards
                //
                //3 4 5 6 7 8 9 0 1 2   Your deck
                Tuple.Create(10, 3, 0, 3),
                Tuple.Create(10, 3, 1, 4),
                Tuple.Create(10, 3, 2, 5),
                Tuple.Create(10, 3, 3, 6),
                Tuple.Create(10, 3, 4, 7),
                Tuple.Create(10, 3, 5, 8),
                Tuple.Create(10, 3, 6, 9),
                Tuple.Create(10, 3, 7, 0),
                Tuple.Create(10, 3, 8, 1),
                Tuple.Create(10, 3, 9, 2),
                // You've also been getting pretty good at a version of this 
                // technique where N is negative! In that case, cut (the 
                // absolute value of) N cards from the bottom of the deck onto 
                // the top. For example, to cut -4:
                //Top          Bottom
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //
                //0 1 2 3 4 5           Your deck
                //            6 7 8 9   Cut cards
                //
                //        0 1 2 3 4 5   Your deck
                //6 7 8 9               Cut cards
                //
                //6 7 8 9 0 1 2 3 4 5   Your deck
                Tuple.Create(10, -4, 0, 6),
                Tuple.Create(10, -4, 1, 7),
                Tuple.Create(10, -4, 2, 8),
                Tuple.Create(10, -4, 3, 9),
                Tuple.Create(10, -4, 4, 0),
                Tuple.Create(10, -4, 5, 1),
                Tuple.Create(10, -4, 6, 2),
                Tuple.Create(10, -4, 7, 3),
                Tuple.Create(10, -4, 8, 4),
                Tuple.Create(10, -4, 9, 5),
            };

            foreach (var testExample in testData)
            {
                var result = ShuffleHelper.GetShuffleFunctionCutNCards(
                    n: testExample.Item2,
                    deckSize: testExample.Item1)
                    .GetInverse()
                    .Evaluate(testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void DealWithIncrementNTest()
        {
            // Item 1: Deck size
            // Item 2: N
            // Item 3: Start card index
            // Item 4: Expected end card index
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<int, int, int, int>>()
            {
                // To deal with increment N, start by clearing enough space on 
                // your table to lay out all of the cards individually in a long 
                // line. Deal the top card into the leftmost position. Then, move 
                // N positions to the right and deal the next card there. If you 
                // would move into a position past the end of the space on your 
                // table, wrap around and keep counting from the leftmost card 
                // again.
                // Continue this process until you run out of cards.
                // For example, to deal with increment 3:
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //. . . . . . . . . .   Space on table
                //^                     Current position
                //
                //Deal the top card to the current position:
                //
                //  1 2 3 4 5 6 7 8 9   Your deck
                //0 . . . . . . . . .   Space on table
                //^                     Current position
                //
                //Move the current position right 3:
                //
                //  1 2 3 4 5 6 7 8 9   Your deck
                //0 . . . . . . . . .   Space on table
                //      ^               Current position
                //
                //Deal the top card:
                //
                //    2 3 4 5 6 7 8 9   Your deck
                //0 . . 1 . . . . . .   Space on table
                //      ^               Current position
                //
                //Move right 3 and deal:
                //
                //      3 4 5 6 7 8 9   Your deck
                //0 . . 1 . . 2 . . .   Space on table
                //            ^         Current position
                //
                //Move right 3 and deal:

                //        4 5 6 7 8 9   Your deck
                //0 . . 1 . . 2 . . 3   Space on table
                //                  ^   Current position
                //
                //Move right 3, wrapping around, and deal:
                //
                //          5 6 7 8 9   Your deck
                //0 . 4 1 . . 2 . . 3   Space on table
                //    ^                 Current position
                //
                //And so on:
                //
                //0 7 4 1 8 5 2 9 6 3   Space on table

                Tuple.Create(10, 3, 0, 0),
                Tuple.Create(10, 3, 1, 3),
                Tuple.Create(10, 3, 2, 6),
                Tuple.Create(10, 3, 3, 9),
                Tuple.Create(10, 3, 4, 2),
                Tuple.Create(10, 3, 5, 5),
                Tuple.Create(10, 3, 6, 8),
                Tuple.Create(10, 3, 7, 1),
                Tuple.Create(10, 3, 8, 4),
                Tuple.Create(10, 3, 9, 7),
            };

            foreach (var testExample in testData)
            {
                var result = ShuffleHelper.GetShuffleFunctionDealWithIncrementN(
                    n: testExample.Item2,
                    deckSize: testExample.Item1)
                    .Evaluate(testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void DealWithIncrementNBackwardsTest()
        {
            // Item 1: Deck size
            // Item 2: N
            // Item 3: Start card index
            // Item 4: Expected end card index
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<int, int, int, int>>()
            {
                // To deal with increment N, start by clearing enough space on 
                // your table to lay out all of the cards individually in a long 
                // line. Deal the top card into the leftmost position. Then, move 
                // N positions to the right and deal the next card there. If you 
                // would move into a position past the end of the space on your 
                // table, wrap around and keep counting from the leftmost card 
                // again.
                // Continue this process until you run out of cards.
                // For example, to deal with increment 3:
                //0 1 2 3 4 5 6 7 8 9   Your deck
                //. . . . . . . . . .   Space on table
                //^                     Current position
                //
                //Deal the top card to the current position:
                //
                //  1 2 3 4 5 6 7 8 9   Your deck
                //0 . . . . . . . . .   Space on table
                //^                     Current position
                //
                //Move the current position right 3:
                //
                //  1 2 3 4 5 6 7 8 9   Your deck
                //0 . . . . . . . . .   Space on table
                //      ^               Current position
                //
                //Deal the top card:
                //
                //    2 3 4 5 6 7 8 9   Your deck
                //0 . . 1 . . . . . .   Space on table
                //      ^               Current position
                //
                //Move right 3 and deal:
                //
                //      3 4 5 6 7 8 9   Your deck
                //0 . . 1 . . 2 . . .   Space on table
                //            ^         Current position
                //
                //Move right 3 and deal:

                //        4 5 6 7 8 9   Your deck
                //0 . . 1 . . 2 . . 3   Space on table
                //                  ^   Current position
                //
                //Move right 3, wrapping around, and deal:
                //
                //          5 6 7 8 9   Your deck
                //0 . 4 1 . . 2 . . 3   Space on table
                //    ^                 Current position
                //
                //And so on:
                //
                //0 7 4 1 8 5 2 9 6 3   Space on table
                Tuple.Create(10, 3, 0, 0),
                Tuple.Create(10, 3, 1, 7),
                Tuple.Create(10, 3, 2, 4),
                Tuple.Create(10, 3, 3, 1),
                Tuple.Create(10, 3, 4, 8),
                Tuple.Create(10, 3, 5, 5),
                Tuple.Create(10, 3, 6, 2),
                Tuple.Create(10, 3, 7, 9),
                Tuple.Create(10, 3, 8, 6),
                Tuple.Create(10, 3, 9, 3),

                // Deck size 10, deal with increment 7:
                // 00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69
                // 0                    1                    2                    3                    4                    5                    6                    7                    8                    9
                // =>
                // 00 01 02 03 04 05 06 07 08 09
                // 0                    1          
                // 10 11 12 13 14 15 16 17 18 19
                //             2                
                // 20 21 22 23 24 25 26 27 28 29
                //    3                    4    
                // 30 31 32 33 34 35 36 37 38 39
                //                5             
                // 40 41 42 43 44 45 46 47 48 49
                //       6                    7 
                // 50 51 52 53 54 55 56 57 58 59
                //                   8          
                // 60 61 62 63 64 65 66 67 68 69
                //          9                   
                // ->
                // 0  3  6  9  2  5  8  1  4  7
                Tuple.Create(10, 7, 0, 0),
                Tuple.Create(10, 7, 1, 3),
                Tuple.Create(10, 7, 2, 6),
                Tuple.Create(10, 7, 3, 9),
                Tuple.Create(10, 7, 4, 2),
                Tuple.Create(10, 7, 5, 5),
                Tuple.Create(10, 7, 6, 8),
                Tuple.Create(10, 7, 7, 1),
                Tuple.Create(10, 7, 8, 4),
                Tuple.Create(10, 7, 9, 7),
            };

            foreach (var testExample in testData)
            {
                var result = ShuffleHelper.GetShuffleFunctionDealWithIncrementN(
                    n: testExample.Item2,
                    deckSize: testExample.Item1)
                    .GetInverse()
                    .Evaluate(testExample.Item3);
                Assert.Equal(testExample.Item4, result);
            }
        }

        [Fact]
        public void ShuffleTest()
        {
            // Item 1: Shuffle instructions
            // Item 2: Deck size
            // Item 3: Final cards order
            // Test examples taken from here:
            // https://adventofcode.com/2019/day/22
            var testData = new List<Tuple<string[], int, int[]>>()
            {
                //  Here are some examples that combine techniques; they all start with a factory order deck of 10 cards:
                
                //deal with increment 7
                //deal into new stack
                //deal into new stack
                //Result: 0 3 6 9 2 5 8 1 4 7
                Tuple.Create(new string[]
                {
                    "deal with increment 7",
                    "deal into new stack",
                    "deal into new stack",
                }, 10, new int[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7, }),

                //cut 6
                //deal with increment 7
                //deal into new stack
                //Result: 3 0 7 4 1 8 5 2 9 6
                Tuple.Create(new string[]
                {
                    "cut 6",
                    "deal with increment 7",
                    "deal into new stack",
                }, 10, new int[] { 3, 0, 7, 4, 1, 8, 5, 2, 9, 6, }),

                //deal with increment 7
                //deal with increment 9
                //cut -2
                //Result: 6 3 0 7 4 1 8 5 2 9
                Tuple.Create(new string[]
                {
                    "deal with increment 7",
                    "deal with increment 9",
                    "cut -2",
                }, 10, new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2, 9, }),

                //deal into new stack
                //cut -2
                //deal with increment 7
                //cut 8
                //cut -4
                //deal with increment 7
                //cut 3
                //deal with increment 9
                //deal with increment 3
                //cut -1
                //Result: 9 2 5 8 1 4 7 0 3 6
                Tuple.Create(new string[]
                {
                    "deal into new stack",
                    "cut -2",
                    "deal with increment 7",
                    "cut 8",
                    "cut -4",
                    "deal with increment 7",
                    "cut 3",
                    "deal with increment 9",
                    "deal with increment 3",
                    "cut -1",
                }, 10, new int[] { 9, 2, 5, 8, 1, 4, 7, 0, 3, 6, }),

            };

            foreach (var testExample in testData)
            {
                var shuffleInstructions = ShuffleHelper.GetShuffleInstructions(testExample.Item1);
                var result = ShuffleHelper.ShuffleDeck(
                    deckSize: testExample.Item2,
                    shuffleInstructions: shuffleInstructions);
                Assert.Equal(testExample.Item3, result);
            }
        }

        [Fact]
        public void GetDay22Part1AnswerTest()
        {
            BigInteger expected = 2519;
            BigInteger actual = Day22.GetDay22Part1Answer();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDay22Part2AnswerTest()
        {
            BigInteger expected = BigInteger.Parse("58966729050483");
            BigInteger actual = Day22.GetDay22Part2Answer();
            Assert.Equal(expected, actual);
        }
    }
}
