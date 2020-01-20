using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day22
{
    public class ShuffleInstruction
    {
        public ShuffleTechnique Technique { get; private set; }
        public BigInteger ShuffleParameter { get; private set; }
        public ShuffleInstruction(ShuffleTechnique technique, BigInteger shuffleParameter)
        {
            Technique = technique;
            ShuffleParameter = shuffleParameter;
        }
    }
}
