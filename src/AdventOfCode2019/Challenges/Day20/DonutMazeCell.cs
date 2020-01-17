using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day20
{
    public class DonutMazeCell
    {
        public GridPoint Position { get; private set; }
        public DonutMazeCellType Type { get; private set; }
        public string PortalLetter { get; set; }
        public DonutMazeCell(GridPoint position, DonutMazeCellType type, string portalLetter)
        {
            Initialize(position, type, portalLetter);
        }

        private void Initialize(GridPoint position, DonutMazeCellType type, string portalLetter)
        {
            Position = position;
            Type = type;
            PortalLetter = portalLetter;
        }

        public static string GetCellString(DonutMazeCell cell)
        {
            if (DonutMazeCellType.Empty.Equals(cell.Type))
                return ".";
            if (DonutMazeCellType.Wall.Equals(cell.Type))
                return "#";
            if (DonutMazeCellType.Portal.Equals(cell.Type))
                return cell.PortalLetter;
            return " ";
        }

        public override string ToString()
        {
            return $"[({Position.X}, {Position.Y}) {Type}]";
        }
    }
}
