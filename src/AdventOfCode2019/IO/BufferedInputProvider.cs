using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.IO
{
    public class BufferedInputProvider : IBufferedInputProvider
    {
        private IList<int> _values = new List<int>();
        private int _valueIndex = 0;

        public void AddInputValue(int value)
        {
            _values.Add(value);
        }

        public int GetInput()
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
