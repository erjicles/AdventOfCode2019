using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day18
{
    public class MazeCell
    {
        public GridPoint Position { get; private set; }
        public MazeCellType Type { get; private set; }
        public string Key { get; set; }
        //public bool ContainsKey { get { return !string.IsNullOrWhiteSpace(Key); } }
        //public string Door { get; set; }
        //public bool ContainsDoor { get { return !string.IsNullOrWhiteSpace(Door); } }
        //public bool IsCurrentPosition { get; private set; } = false;
        public MazeCell(GridPoint position, MazeCellType type)
        {
            Initialize(position, type, string.Empty);
        }

        private void Initialize(GridPoint position, MazeCellType type, string item)
        {
            Position = position;
            Type = type;
        }

        public void SetType(MazeCellType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"[({Position.X}, {Position.Y}), {Type}]";
        }
    }
}
