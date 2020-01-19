using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day22
{
    public class Deck
    {
        public IList<SpaceCard> Cards { get; private set; }
        public Deck(int numberOfCards)
        {
            Cards = new List<SpaceCard>(numberOfCards);
            for (int i = 0; i < numberOfCards; i++)
            {
                Cards.Add(new SpaceCard(i));
            }
        }

        public static int[] Shuffle(
            int deckSize, 
            IList<Tuple<ShuffleTechnique, int>> shuffleInstructions)
        {
            var result = new int[deckSize];
            for (int startCardIndex = 0; startCardIndex < deckSize; startCardIndex++)
            {
                int endCardIndex = Shuffle(
                    startCardIndex: startCardIndex,
                    deckSize: deckSize,
                    shuffleInstructions: shuffleInstructions);
                result[endCardIndex] = startCardIndex;
            }
            return result;
        }

        public static int Shuffle(
            int startCardIndex, 
            int deckSize,
            IList<Tuple<ShuffleTechnique, int>> shuffleInstructions)
        {
            int endCardIndex = startCardIndex;
            foreach (var shuffleInstruction in shuffleInstructions)
            {
                endCardIndex = PerformShuffleInstruction(
                    startCardIndex: endCardIndex,
                    deckSize: deckSize,
                    shuffleInstruction: shuffleInstruction);
            }
            return endCardIndex;
        }

        public static int PerformShuffleInstruction(
            int startCardIndex,
            int deckSize,
            Tuple<ShuffleTechnique, int> shuffleInstruction)
        {
            return shuffleInstruction.Item1 switch
            {
                ShuffleTechnique.DealIntoNewStack => DealIntoNewStack(startCardIndex, deckSize),
                ShuffleTechnique.CutNCards => CutNCards(startCardIndex, shuffleInstruction.Item2, deckSize),
                ShuffleTechnique.DealWithIncrementN => DealWithIncrementN(startCardIndex, shuffleInstruction.Item2, deckSize),
                _ => throw new Exception($"Invalid shuffle instruction {shuffleInstruction.Item1}"),
            };
        }

        public static int DealIntoNewStack(int startCardIndex, int deckSize)
        {
            // To deal into new stack, create a new stack of cards by 
            // dealing the top card of the deck onto the top of the new 
            // stack repeatedly until you run out of cards
            int endCardIndex = deckSize - 1 - startCardIndex;
            return endCardIndex;
        }

        public static int CutNCards(int startCardIndex, int n, int deckSize)
        {
            // To cut N cards, take the top N cards off the top of the 
            // deck and move them as a single unit to the bottom of the 
            // deck, retaining their order. 
            // You've also been getting pretty good at a version of this 
            // technique where N is negative! In that case, cut (the 
            // absolute value of) N cards from the bottom of the deck onto 
            // the top
            int cutCount = Math.Abs(n);
            if (cutCount > deckSize)
                throw new Exception($"{nameof(n)} greater than {nameof(deckSize)}");
            if (startCardIndex < 0 || startCardIndex >= deckSize)
                throw new ArgumentException(nameof(startCardIndex));
            int endCardIndex;
            if (n >= 0)
            {
                int cutIndex = deckSize - n;
                if (startCardIndex >= n)
                {
                    endCardIndex = startCardIndex - n;
                }
                else
                {
                    endCardIndex = cutIndex + startCardIndex;
                }
            }
            else
            {
                int cutIndex = deckSize - cutCount;
                if (startCardIndex < cutIndex)
                {
                    endCardIndex = startCardIndex + cutCount;
                }
                else
                {
                    endCardIndex = startCardIndex - cutIndex;
                }
            }
            return endCardIndex;
        }

        public static int DealWithIncrementN(int startCardIndex, int n, int deckSize)
        {
            // To deal with increment N, start by clearing enough space on 
            // your table to lay out all of the cards individually in a long 
            // line. Deal the top card into the leftmost position. Then, move 
            // N positions to the right and deal the next card there. If you 
            // would move into a position past the end of the space on your 
            // table, wrap around and keep counting from the leftmost card 
            // again.
            // Continue this process until you run out of cards.
            if (n > deckSize)
                throw new Exception($"{nameof(n)} greater than {nameof(deckSize)}");
            if (startCardIndex < 0 || startCardIndex >= deckSize)
                throw new ArgumentException(nameof(startCardIndex));
            int endCardIndexRaw = startCardIndex * n;
            int endCardIndex = endCardIndexRaw % deckSize;
            return endCardIndex;
        }
    }
}
