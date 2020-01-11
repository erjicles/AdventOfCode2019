using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public static class GridHelper
    {
        private static ConsoleColor GetPointColorDefault(GridPoint point)
        {
            return Console.ForegroundColor;
        }
        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            string appendText)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string appendText)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            bool invertY)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints, 
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY)
        {
            int minX = gridPoints.Min(p => p.X);
            int maxX = gridPoints.Max(p => p.X);
            int minY = gridPoints.Min(p => p.Y);
            int maxY = gridPoints.Max(p => p.Y);
            Console.WriteLine();

            var yDirection = invertY ? -1 : 1;
            var yStart = invertY ? maxY : minY;
            var yEnd = invertY ? minY : maxY;
            int yDiff = Math.Abs(yEnd - yStart);
            for (int yIndex = 0; yIndex <= yDiff; yIndex++)
            {
                int y = yStart + (yIndex * yDirection);
                Console.Write(prependText);
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new GridPoint(x, y);
                    var pointString = GetPointString(point);
                    var oldColor = Console.ForegroundColor;
                    var color = GetPointColor(point);
                    if (!oldColor.Equals(color))
                    {
                        Console.ForegroundColor = color;
                    }
                    Console.Write(pointString);
                    if (!oldColor.Equals(color))
                    {
                        Console.ForegroundColor = oldColor;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


    }
}
