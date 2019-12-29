using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class BufferedInputProvider : IBufferedInputProvider
    {
        public List<BigInteger> Values { get; } = new List<BigInteger>();
        private int _valueIndex = 0;

        public void AddInputValue(BigInteger value)
        {
            Values.Add(value);
        }

        public BigInteger GetInput()
        {
            if (Values == null || Values.Count == 0)
                throw new Exception("No values defined");
            if (_valueIndex >= Values.Count)
                throw new Exception("Not enough values in list");
            var currentValueIndex = _valueIndex;
            _valueIndex++;
            return Values[currentValueIndex];
        }
        public bool HasInput()
        {
            if (Values == null || Values.Count == 0)
                return false;
            if (_valueIndex < Values.Count)
                return true;
            return false;
        }
    }
}
