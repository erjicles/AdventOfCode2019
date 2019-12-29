using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day14
{
    public class Reaction
    {
        public string Output { get; private set; }
        public int OutputQuantity { get; private set; }
        public IList<Tuple<string, int>> Inputs { get; private set; }
        public Reaction(
            string output, 
            int outputQuantity,
            IList<Tuple<string, int>> inputs)
        {
            Output = output;
            OutputQuantity = outputQuantity;
            Inputs = inputs;
        }

        /// <summary>
        /// Takes a strong formatted as "QUANTITY CHEMICAL" and returns a tuple
        /// of the format (CHEMICAL, QUANTITY)
        /// </summary>
        /// <param name="componentDefinition"></param>
        /// <returns></returns>
        public static Tuple<string, int> ParseReactionComponent(string componentDefinition)
        {
            var match = Regex.Match(componentDefinition, @"^\s*(\d+)\s+(\w+)\s*$");
            if (!match.Success)
                throw new Exception($"Badly formatted component definition: {componentDefinition}");
            var result = new Tuple<string, int>(
                match.Groups[2].Value,
                int.Parse(match.Groups[1].Value));
            return result;
        }

        public static Reaction ParseReaction(string reactionDefinition)
        {
            // Reactions are formatted like this:
            // num1 ABC, num2 DEF, ..., numN XYZ => outputQuantity OUTPUT
            var definitionSplit = reactionDefinition.Split("=>");
            if (definitionSplit.Length != 2)
                throw new Exception($"Badly formatted reaction definition: {reactionDefinition}");

            IList<Tuple<string, int>> inputs = new List<Tuple<string, int>>();
            var inputSplit = definitionSplit[0].Split(",");
            foreach (var input in inputSplit)
            {
                var inputComponent = ParseReactionComponent(input);
                inputs.Add(inputComponent);
            }
            var output = ParseReactionComponent(definitionSplit[1]);
            var result = new Reaction(output.Item1, output.Item2, inputs);
            return result;
        }
    }
}
