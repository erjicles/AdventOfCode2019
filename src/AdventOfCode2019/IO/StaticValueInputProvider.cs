using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public class StaticValueInputProvider : IInputProvider
    {
        private readonly int _value;
        public StaticValueInputProvider(int value)
        {
            _value = value;
        }

        public int GetUserInput()
        {
            return _value;
        }
    }
}
