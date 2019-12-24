using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.IO
{
    public interface IInputProvider
    {
        bool HasInput();
        int GetInput();
    }
}
