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

        /// <summary>
        /// Adds <paramref name="value"/> to the input buffer.
        /// </summary>
        /// <param name="value"></param>
        public void AddInputValue(BigInteger value)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to its constituent ASCII values
        /// and adds each of those to the input buffer.
        /// </summary>
        /// <param name="value"></param>
        public void AddInputValue(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                int asciiValue = char.ConvertToUtf32(value, i);
                AddInputValue(asciiValue);
            }
        }

        /// <summary>
        /// Gets the next value in the input buffer and increments the pointer
        /// to the next value in the buffer.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if there are values in the input buffer that haven't
        /// yet been read. Otherwise returns false.
        /// </summary>
        /// <returns></returns>
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
