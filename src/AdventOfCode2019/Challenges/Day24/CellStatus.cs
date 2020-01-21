using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day24
{
    public class CellStatus
    {
        public int NumberOfEmptyNeighbors { get; private set; }
        public int NumberOfBugNeighbors { get; private set; }
        public CellStatus(int numberOfEmptyNeighbors, int numberOfBugNeighbors)
        {
            NumberOfEmptyNeighbors = numberOfEmptyNeighbors;
            NumberOfBugNeighbors = numberOfBugNeighbors;
        }
    }
}
