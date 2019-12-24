using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class StaticValueInputProvider : IInputProvider
    {
        private readonly BigInteger[] _values = new BigInteger[] { };
        private int _valueIndex = 0;
        public StaticValueInputProvider(BigInteger value)
        {
            _values = new BigInteger[] { value };
        }
        public StaticValueInputProvider(BigInteger[] values)
        {
            _values = new BigInteger[values.Length];
            Array.Copy(values, _values, values.Length);
        }

        public BigInteger GetInput()
        {
            if (_values == null || _values.Length == 0)
                throw new Exception("No values defined");
            if (_valueIndex >= _values.Length)
                throw new Exception("Not enough values in list");
            var currentValueIndex = _valueIndex;
            _valueIndex++;
            return _values[currentValueIndex];
        }
        public bool HasInput()
        {
            if (_values == null || _values.Length == 0)
                return false;
            if (_valueIndex < _values.Length)
                return true;
            return false;
        }
    }
}
