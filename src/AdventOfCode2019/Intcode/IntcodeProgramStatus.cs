using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Intcode
{
    public enum IntcodeProgramStatus
    {
        Running = 0,
        AwaitingInput = 3,
        Completed = 99
    }
}
