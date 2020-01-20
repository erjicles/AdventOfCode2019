using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day22
{
    /// <summary>
    /// Solution to the Day 22 challenge:
    /// https://adventofcode.com/2019/day/22
    /// </summary>
    public class Day22
    {
        public const string FILE_NAME = "Day22Input.txt";
        public static BigInteger GetDay22Part1Answer()
        {
            // After shuffling your factory order deck of 10007 cards, what is 
            // the position of card 2019?
            // Answer: 2519
            var input = GetDay22Input();
            var shuffleInstructions = ShuffleHelper.GetShuffleInstructions(input);
            var endCardIndex = ShuffleHelper.ShuffleCard(
                startCardIndex: 2019,
                deckSize: 10007,
                numberOfShuffles: 1,
                shuffleInstructions: shuffleInstructions,
                runBackwards: false);
            return endCardIndex;
        }

        public static BigInteger GetDay22Part2Answer()
        {
            // When you get back, you discover that the 3D printers have 
            // combined their power to create for you a single, giant, brand 
            // new, factory order deck of 119315717514047 space cards.
            // Finally, a deck of cards worthy of shuffling!
            // You decide to apply your complete shuffle process(your puzzle 
            // input) to the deck 101741582076661 times in a row.
            // You'll need to be careful, though - one wrong move with this 
            // many cards and you might overflow your entire ship!
            // After shuffling your new, giant, factory order deck that many 
            // times, what number is on the card that ends up in position 2020?
            // Answer: 58966729050483
            var input = GetDay22Input();
            var shuffleInstructions = ShuffleHelper.GetShuffleInstructions(input);
            var endCardIndex = ShuffleHelper.ShuffleCard(
                startCardIndex: 2020,
                deckSize: BigInteger.Parse("119315717514047"),
                numberOfShuffles: BigInteger.Parse("101741582076661"),
                shuffleInstructions: shuffleInstructions,
                runBackwards: true);
            return endCardIndex;
        }

        public static IList<string> GetDay22Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
