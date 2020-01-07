using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public static class GridHelper
    {
        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString)
        {
            DrawGrid2D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
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
                prependText: "     ",
                invertY: invertY);
        }

        public static void DrawGrid2D(
            ICollection<GridPoint> gridPoints, 
            Func<GridPoint, string> GetPointString,
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
                    Console.Write(pointString);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


    }
}
