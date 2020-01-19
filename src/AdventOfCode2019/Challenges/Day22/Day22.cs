using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
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
        public static int GetDay22Part1Answer()
        {
            // After shuffling your factory order deck of 10007 cards, what is 
            // the position of card 2019?
            // Answer: 2519
            var input = GetDay22Input();
            var shuffleInstructions = ShuffleHelper.GetShuffleInstructions(input);
            var endCardIndex = Deck.Shuffle(
                startCardIndex: 2019,
                deckSize: 10007,
                shuffleInstructions: shuffleInstructions);
            return endCardIndex;
        }

        public static IList<string> GetDay22Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
