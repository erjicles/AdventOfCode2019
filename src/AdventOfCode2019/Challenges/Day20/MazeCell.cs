using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day20
{
    public class MazeCell
    {
        public GridPoint Position { get; private set; }
        public MazeCellType Type { get; private set; }
        public string PortalLetter { get; set; }
        public MazeCell(GridPoint position, MazeCellType type, string portalLetter)
        {
            Initialize(position, type, portalLetter);
        }

        private void Initialize(GridPoint position, MazeCellType type, string portalLetter)
        {
            Position = position;
            Type = type;
            PortalLetter = portalLetter;
        }

        public static string GetCellString(MazeCell cell)
        {
            if (MazeCellType.Empty.Equals(cell.Type))
                return ".";
            if (MazeCellType.Wall.Equals(cell.Type))
                return "#";
            if (MazeCellType.Portal.Equals(cell.Type))
                return cell.PortalLetter;
            return " ";
        }

        public override string ToString()
        {
            return $"[({Position.X}, {Position.Y}), {Type}]";
        }
    }
}
