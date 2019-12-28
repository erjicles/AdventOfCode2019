using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day13
{
    public class GameGridCell : GridPoint
    {
        public GameCellType Type { get; set; }
        public GameGridCell(int x, int y, GameCellType type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public string DrawCell()
        {
            if (GameCellType.Ball.Equals(Type))
                return "O";
            if (GameCellType.Block.Equals(Type))
                return "#";
            if (GameCellType.HorizontalPaddle.Equals(Type))
                return "P";
            if (GameCellType.Wall.Equals(Type))
                return "W";
            return " ";
        }
    }
}
