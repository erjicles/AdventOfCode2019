using AdventOfCode2019.MathHelpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day22
{
    public static class ShuffleHelper
    {

        public static int[] ShuffleDeck(
            int deckSize,
            IList<ShuffleInstruction> shuffleInstructions)
        {
            var result = new int[deckSize];
            for (int startCardIndex = 0; startCardIndex < deckSize; startCardIndex++)
            {
                var endCardIndex = (int)ShuffleCard(
                    startCardIndex: startCardIndex,
                    deckSize: deckSize,
                    numberOfShuffles: 1,
                    shuffleInstructions: shuffleInstructions,
                    runBackwards: false);
                result[endCardIndex] = startCardIndex;
            }
            return result;
        }

        public static BigInteger ShuffleCard(
            BigInteger startCardIndex,
            BigInteger deckSize,
            BigInteger numberOfShuffles,
            IList<ShuffleInstruction> shuffleInstructions,
            bool runBackwards)
        {
            var shuffleFunction = GetShuffleFunction(deckSize, shuffleInstructions);
            // Compose the shuffle function into itself once for each shuffle pass
            shuffleFunction = ExponentiationHelper.GetExponentiationByPowers(
                x: shuffleFunction,
                exponent: numberOfShuffles,
                MultiplyFunction: LCG.Compose,
                identity: LCG.GetIdentity(deckSize));
            // If we are running backwards, then use the inverse of the 
            // shuffle function
            if (runBackwards)
                shuffleFunction = shuffleFunction.GetInverse();
            var endCardIndex = shuffleFunction.Evaluate(startCardIndex);
            return endCardIndex;
        }

        public static LCG GetShuffleFunction(
            BigInteger deckSize,
            IList<ShuffleInstruction> shuffleInstructions)
        {
            // Combine all shuffle instructions into one LCG
            var result = LCG.GetIdentity(deckSize);
            var dealIntoNewStack = GetShuffleFunctionDealIntoNewStack(deckSize);
            foreach (var shuffleInstruction in shuffleInstructions)
            {
                if (ShuffleTechnique.DealIntoNewStack.Equals(shuffleInstruction.Technique))
                {
                    result = LCG.Compose(result, dealIntoNewStack);
                }
                else if (ShuffleTechnique.CutNCards.Equals(shuffleInstruction.Technique))
                {
                    var cutNCards = GetShuffleFunctionCutNCards(shuffleInstruction.ShuffleParameter, deckSize);
                    result = LCG.Compose(result, cutNCards);
                }
                else if (ShuffleTechnique.DealWithIncrementN.Equals(shuffleInstruction.Technique))
                {
                    var dealWithIncrementN = GetShuffleFunctionDealWithIncrementN(shuffleInstruction.ShuffleParameter, deckSize);
                    result = LCG.Compose(result, dealWithIncrementN);
                }
            }
            return result;
        }

        public static LCG GetShuffleFunctionDealIntoNewStack(BigInteger deckSize)
        {
            // To deal into new stack, create a new stack of cards by 
            // dealing the top card of the deck onto the top of the new 
            // stack repeatedly until you run out of cards
            // This is equivalent to the following LCG:
            // For all card indexes x,
            // DealIntoNewStack(x) = -x - 1 (mod deckSize)
            return new LCG(-1, -1, deckSize);
        }

        public static LCG GetShuffleFunctionCutNCards(BigInteger n, BigInteger deckSize)
        {
            // To cut N cards, take the top N cards off the top of the 
            // deck and move them as a single unit to the bottom of the 
            // deck, retaining their order. 
            // You've also been getting pretty good at a version of this 
            // technique where N is negative! In that case, cut (the 
            // absolute value of) N cards from the bottom of the deck onto 
            // the top
            // This is equivalent to the following LCG:
            // For all card indexes x,
            // CutNCards(x) = x - n (mod m)
            return new LCG(1, -n, deckSize);
        }

        public static LCG GetShuffleFunctionDealWithIncrementN(BigInteger n, BigInteger deckSize)
        {
            // To deal with increment N, start by clearing enough space on 
            // your table to lay out all of the cards individually in a long 
            // line. Deal the top card into the leftmost position. Then, move 
            // N positions to the right and deal the next card there. If you 
            // would move into a position past the end of the space on your 
            // table, wrap around and keep counting from the leftmost card 
            // again.
            // Continue this process until you run out of cards.
            // This is equivalent to the following LCG:
            // For all card indexes x,
            // DealWithIncrementN(x) = n*X (mod m)
            return new LCG(n, 0, deckSize);
        }

        public static IList<ShuffleInstruction> GetShuffleInstructions(IList<string> shuffleInstructions)
        {
            var result = new List<ShuffleInstruction>();
            foreach (var instruction in shuffleInstructions)
            {
                if (string.IsNullOrWhiteSpace(instruction))
                    continue;
                var matchDealIntoNewStack = Regex.Match(instruction, @"^deal into new stack$");
                var matchCutNCards = Regex.Match(instruction, @"^cut (-?[0-9]+)$");
                var matchDealWithIncrement = Regex.Match(instruction, @"^deal with increment ([0-9]+)$");
                if (matchDealIntoNewStack.Success)
                {
                    result.Add(new ShuffleInstruction(ShuffleTechnique.DealIntoNewStack, BigInteger.Zero));
                }
                else if (matchCutNCards.Success)
                {
                    BigInteger n = BigInteger.Parse(matchCutNCards.Groups[1].Value);
                    result.Add(new ShuffleInstruction(ShuffleTechnique.CutNCards, n));
                }
                else if (matchDealWithIncrement.Success)
                {
                    BigInteger n = BigInteger.Parse(matchDealWithIncrement.Groups[1].Value);
                    result.Add(new ShuffleInstruction(ShuffleTechnique.DealWithIncrementN, n));
                }
                else
                {
                    throw new Exception($"Invalid shuffle instruction: {instruction}");
                }
            }
            return result;
        }
    }
}
