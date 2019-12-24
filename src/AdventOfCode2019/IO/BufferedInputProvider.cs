using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class BufferedInputProvider : IBufferedInputProvider
    {
        private IList<BigInteger> _values = new List<BigInteger>();
        private int _valueIndex = 0;

        public void AddInputValue(BigInteger value)
        {
            _values.Add(value);
        }

        public BigInteger GetInput()
        {
            if (_values == null || _values.Count == 0)
                throw new Exception("No values defined");
            if (_valueIndex >= _values.Count)
                throw new Exception("Not enough values in list");
            var currentValueIndex = _valueIndex;
            _valueIndex++;
            return _values[currentValueIndex];
        }
        public bool HasInput()
        {
            if (_values == null || _values.Count == 0)
                return false;
            if (_valueIndex < _values.Count)
                return true;
            return false;
        }
    }
}
