using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day22
{
    public static class ShuffleHelper
    {
        public static IList<Tuple<ShuffleTechnique, int>> GetShuffleInstructions(IList<string> shuffleInstructions)
        {
            var result = new List<Tuple<ShuffleTechnique, int>>();
            foreach (var instruction in shuffleInstructions)
            {
                if (string.IsNullOrWhiteSpace(instruction))
                    continue;
                var matchDealIntoNewStack = Regex.Match(instruction, @"^deal into new stack$");
                var matchCutNCards = Regex.Match(instruction, @"^cut (-?[0-9]+)$");
                var matchDealWithIncrement = Regex.Match(instruction, @"^deal with increment ([0-9]+)$");
                if (matchDealIntoNewStack.Success)
                {
                    result.Add(Tuple.Create(ShuffleTechnique.DealIntoNewStack, 0));
                }
                else if (matchCutNCards.Success)
                {
                    int n = int.Parse(matchCutNCards.Groups[1].Value);
                    result.Add(Tuple.Create(ShuffleTechnique.CutNCards, n));
                }
                else if (matchDealWithIncrement.Success)
                {
                    int n = int.Parse(matchDealWithIncrement.Groups[1].Value);
                    result.Add(Tuple.Create(ShuffleTechnique.DealWithIncrementN, n));
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
