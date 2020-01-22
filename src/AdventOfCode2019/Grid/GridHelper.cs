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
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString)
        {
            DrawGrid2D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor)
        {
            DrawGrid2D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString,
            string appendText)
        {
            DrawGrid2D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string appendText)
        {
            DrawGrid2D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString,
            bool invertY)
        {
            DrawGrid2D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            DrawGrid2D(
                minX: gridPoints.Min(p => p.X),
                maxX: gridPoints.Max(p => p.X),
                minY: gridPoints.Min(p => p.Y),
                maxY: gridPoints.Max(p => p.Y),
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY);
        }

        public static void DrawGrid2D(
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY)
        {
            var renderingData = GetGridRenderingData(
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY,
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY);
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
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString,
            string appendText)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string appendText)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            int minX,
            int minY,
            int maxX,
            int maxY,
            Func<GridPoint, string> GetPointString,
            bool invertY)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
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
            return GetGridRenderingData(
                minX: gridPoints.Min(p => p.X),
                maxX: gridPoints.Max(p => p.X),
                minY: gridPoints.Min(p => p.Y),
                maxY: gridPoints.Max(p => p.Y),
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            Func<GridPoint, string> GetPointString,
            Func<GridPoint, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY)
        {
            var result = new List<Tuple<string, ConsoleColor>>();
            var builder = new StringBuilder();
            builder.Append(Environment.NewLine);

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

        /// <summary>
        /// Gets all points along an edge.
        /// <paramref name="edgeValues"/> should contain 3 values:
        /// <paramref name="edgeValues"/>[i] should be the constant value
        /// corresponding to the <paramref name="coordinateOrder"/>[i]th
        /// coordinate, for i in {0}
        /// <paramref name="edgeValues"/>[1] and <paramref name="edgeValues"/>[2]
        /// should be the min and max values of the <paramref name="coordinateOrder"/>[1]st
        /// coordinate.
        /// </summary>
        /// <param name="edgeValues"></param>
        /// <param name="coordinateOrder"></param>
        /// <returns></returns>
        public static IList<GridPoint> GetPointsAlongEdge(
            int[] edgeValues,
            string coordinateOrder)
        {
            if (edgeValues.Length != 3)
                throw new ArgumentException(nameof(edgeValues));
            if (coordinateOrder.Length != 2)
                throw new ArgumentException(nameof(coordinateOrder));
            coordinateOrder = coordinateOrder.ToUpper();
            if (!coordinateOrder.Contains("X"))
                throw new ArgumentException(nameof(coordinateOrder));
            if (!coordinateOrder.Contains("Y"))
                throw new ArgumentException(nameof(coordinateOrder));

            var result = new List<GridPoint>();
            for (int rangeCoordinate = edgeValues[1]; rangeCoordinate <= edgeValues[2]; rangeCoordinate++)
            {
                var pointValues = new int[2];
                for (int pointCoordinateIndex = 0; pointCoordinateIndex < 2; pointCoordinateIndex++)
                {
                    var coordinate = "XY"[pointCoordinateIndex];
                    var pointCoordinateOrderIndex = coordinateOrder.IndexOf(coordinate);
                    int value;
                    if (pointCoordinateOrderIndex == 1)
                        value = rangeCoordinate;
                    else
                        value = edgeValues[pointCoordinateOrderIndex];
                    pointValues[pointCoordinateIndex] = value;
                }
                var point = new GridPoint(pointValues[0], pointValues[1]);
                result.Add(point);
            }
            return result;
        }

    }
}
