using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day14
{
    /// <summary>
    /// Solution to the Day 14 challenge:
    /// https://adventofcode.com/2019/day/14
    /// </summary>
    public class Day14
    {
        public const string FILE_NAME = "Day14Input.txt";
        public const string OreName = "ORE";
        public const string FuelName = "FUEL";

        public static int GetDay14Part1Answer()
        {
            // Given the list of reactions in your puzzle input, what is the 
            // minimum amount of ORE required to produce exactly 1 FUEL?
            // Answer: 873899
            IList<string> input = GetDay14Input();
            int result = GetQuantityOfComponentRequiredToGenerateOutput(
                input,
                OreName,
                FuelName,
                1);
            return result;
        }

        public static int GetQuantityOfComponentRequiredToGenerateOutput(
            IList<string> reactionDefinitions,
            string component,
            string output,
            int outputQuantity)
        {
            var reactions = ParseReactions(reactionDefinitions);
            var reactionOutputDictionary = GetReactionOutputDictionary(reactions);
            if (!reactionOutputDictionary.ContainsKey(output))
                throw new Exception($"{output} missing from reaction definitions");
            if (reactions.Where(r => r.Inputs.Any(i => component.Equals(i.Item1))).Count() == 0)
                throw new Exception($"{component} missing from input reaction definitions");
            var inputDependents = GetInputDependents(reactions);
            
            int result = 0;

            // Starting with the desired output, work backwards through the 
            // chain until all inputs have been processed, and total up the
            // quantity of the requested component
            var requirements = new Dictionary<string, int>
            {
                { output, outputQuantity }
            };
            // Keep looping until the only remaining requirement is just the requested component
            while (requirements.Where(kvp => component.Equals(kvp.Key)).Count() != requirements.Count)
            {
                // Locate a requirement to process:
                // Only process a requirement where none of the other requirements
                // are dependent on it
                var requirement = GetRequirementWhereNoneOfTheOtherRequirementsAreDependentOnIt(
                    requirements.Select(kvp => kvp.Key), inputDependents);
                var requirementQuantity = requirements[requirement];
                var reactionOutputtingRequirement = reactionOutputDictionary[requirement];
                requirements.Remove(requirement);

                // Determine how many repeated reactions are needed to produce
                // the required output quantity
                int numberOfReactionsNeeded = (int)Math.Ceiling(
                    (decimal)requirementQuantity / reactionOutputtingRequirement.OutputQuantity);

                // Add each reaction input as a new requirement, along with the
                // quantity needed for the requiremed number of reactions
                foreach (var input in reactionOutputtingRequirement.Inputs)
                {
                    var amountOfInputRequiredForReactions =
                        input.Item2 * numberOfReactionsNeeded;
                    if (!requirements.ContainsKey(input.Item1))
                        requirements.Add(input.Item1, 0);
                    requirements[input.Item1] += amountOfInputRequiredForReactions;
                }

            }

            result = requirements[component];
            return result;
        }

        public static string GetRequirementWhereNoneOfTheOtherRequirementsAreDependentOnIt(
            IEnumerable<string> requirements, 
            Dictionary<string, HashSet<string>> inputDependents)
        {
            foreach (var requirement in requirements)
            {
                bool hasDependentRequirement = false;
                foreach (var otherRequirement in requirements.Where(r => !requirement.Equals(r)))
                {
                    if (inputDependents[requirement].Contains(otherRequirement))
                    {
                        hasDependentRequirement = true;
                        break;
                    }
                }
                if (!hasDependentRequirement)
                    return requirement;
            }
            throw new Exception("No independent requirements found");
        }

        /// <summary>
        /// For each input, create a set of all outputs that are dependent
        /// on the input.
        /// </summary>
        /// <param name="reactions"></param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> GetInputDependents(IList<Reaction> reactions)
        {
            var result = new Dictionary<string, HashSet<string>>();
            var uniqueInputs = reactions
                .SelectMany(r => r.Inputs.Select(i => i.Item1))
                .Distinct();
            foreach (var input in uniqueInputs)
            {
                var outputsDependentOnInput = GetOutputsDependentOnInput(input, reactions);
                result.Add(input, outputsDependentOnInput);
            }
            return result;
        }

        public static HashSet<string> GetOutputsDependentOnInput(string input, IList<Reaction> reactions)
        {
            var result = new HashSet<string>();
            var outputsAlreadyAdded = new HashSet<string>();
            var outputsDependentOnInput = reactions
                .Where(r => r.Inputs.Any(i => input.Equals(i.Item1)))
                .Select(r => r.Output)
                .ToHashSet();
            while (outputsDependentOnInput.Count > 0)
            {
                foreach (var output in outputsDependentOnInput.ToList())
                {
                    outputsDependentOnInput.Remove(output);
                    if (outputsAlreadyAdded.Contains(output))
                        continue;
                    result.Add(output);
                    outputsAlreadyAdded.Add(output);
                    var outputsDependentOnThisOutput = reactions
                        .Where(r => r.Inputs.Any(i => output.Equals(i.Item1)))
                        .Select(r => r.Output)
                        .ToHashSet();
                    foreach (var dependentOutput in outputsDependentOnThisOutput)
                    {
                        if (!outputsAlreadyAdded.Contains(dependentOutput)
                            && !outputsDependentOnInput.Contains(dependentOutput))
                        {
                            outputsDependentOnInput.Add(dependentOutput);
                        }
                    }
                }
            }
            return result;
        }

        public static Dictionary<string, Reaction> GetReactionOutputDictionary(IList<Reaction> reactions)
        {
            var result = new Dictionary<string, Reaction>();
            foreach (var reaction in reactions)
            {
                result.Add(reaction.Output, reaction);
            }
            return result;
        }
        public static IList<Reaction> ParseReactions(IList<string> reactionDefinitions)
        {
            var result = new List<Reaction>();
            foreach (var reactionDefinition in reactionDefinitions)
            {
                var reaction = Reaction.ParseReaction(reactionDefinition);
                result.Add(reaction);
            }
            return result;
        }

        public static IList<string> GetDay14Input()
        {
            return FileHelper.ReadInputFileLines(FILE_NAME);
        }
    }
}
