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
            var renderingData = GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY);
            foreach (var renderingBlock in renderingData)
            {
                var blockColor = renderingBlock.Item2;
                var consoleColor = Console.ForegroundColor;
                if (!consoleColor.Equals(blockColor))
                {
                    Console.ForegroundColor = blockColor;
                }
                Console.Write(renderingBlock.Item1);
                if (!consoleColor.Equals(blockColor))
                {
                    Console.ForegroundColor = consoleColor;
                }
            }
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            string appendText)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string appendText)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            bool invertY)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint> gridPoints,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY)
        {
            var result = new List<Tuple<string, ConsoleColor>>();
            var builder = new StringBuilder();
            builder.Append(Environment.NewLine);

            int minX = gridPoints.Min(p => p.X);
            int maxX = gridPoints.Max(p => p.X);
            int minY = gridPoints.Min(p => p.Y);
            int maxY = gridPoints.Max(p => p.Y);

            var yDirection = invertY ? -1 : 1;
            var yStart = invertY ? maxY : minY;
            var yEnd = invertY ? minY : maxY;
            int yDiff = Math.Abs(yEnd - yStart);
            for (int yIndex = 0; yIndex <= yDiff; yIndex++)
            {
                int y = yStart + (yIndex * yDirection);
                builder.Append(prependText);
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new GridPoint(x, y);
                    var pointString = GetPointString(point);
                    var pointColor = GetPointColor(point);
                    if (!pointColor.Equals(Console.ForegroundColor))
                    {
                        result.Add(new Tuple<string, ConsoleColor>(builder.ToString(), Console.ForegroundColor));
                        builder.Clear();
                    }
                    builder.Append(pointString);
                    if (!pointColor.Equals(Console.ForegroundColor))
                    {
                        result.Add(new Tuple<string, ConsoleColor>(builder.ToString(), pointColor));
                        builder.Clear();
                    }
                }
                builder.Append(Environment.NewLine);
            }
            builder.Append(Environment.NewLine);
            result.Add(new Tuple<string, ConsoleColor>(builder.ToString(), Console.ForegroundColor));
            return result;
        }

    }
}
