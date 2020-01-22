using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public static class GridHelper3D
    {
        private static ConsoleColor GetPointColorDefault(GridPoint3D point)
        {
            return Console.ForegroundColor;
        }
        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString)
        {
            DrawGrid3D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString)
        {
            DrawGrid3D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor)
        {
            DrawGrid3D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor)
        {
            DrawGrid3D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            string appendText)
        {
            DrawGrid3D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            string appendText)
        {
            DrawGrid3D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string appendText)
        {
            DrawGrid3D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string appendText)
        {
            DrawGrid3D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            bool invertY)
        {
            DrawGrid3D(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY,
                invertZ: false);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            bool invertY)
        {
            DrawGrid3D(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY,
                invertZ: false);
        }

        public static void DrawGrid3D(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            DrawGrid3D(
                minX: gridPoints.Min(p => p.X),
                maxX: gridPoints.Max(p => p.X),
                minY: gridPoints.Min(p => p.Y),
                maxY: gridPoints.Max(p => p.Y),
                minZ: gridPoints.Min(p => p.Z),
                maxZ: gridPoints.Max(p => p.Z),
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY,
                invertZ: invertZ);
        }

        public static void DrawGrid3D(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            var renderingData = GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY,
                invertZ: invertZ);
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
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: "     ",
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            string appendText)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            string appendText)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string appendText)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string appendText)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: appendText,
                invertY: false,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            bool invertY)
        {
            return GetGridRenderingData(
                gridPoints: gridPoints,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            bool invertY)
        {
            return GetGridRenderingData(
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                minZ: minZ,
                maxZ: maxZ,
                GetPointString: GetPointString,
                GetPointColor: GetPointColorDefault,
                prependText: "     ",
                invertY: invertY,
                invertZ: false);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            return GetGridRenderingData(
                minX: gridPoints.Min(p => p.X),
                maxX: gridPoints.Max(p => p.X),
                minY: gridPoints.Min(p => p.Y),
                maxY: gridPoints.Max(p => p.Y),
                minZ: gridPoints.Min(p => p.Z),
                maxZ: gridPoints.Max(p => p.Z),
                GetPointString: GetPointString,
                GetPointColor: GetPointColor,
                prependText: prependText,
                invertY: invertY,
                invertZ: invertZ);
        }

        public static IList<Tuple<string, ConsoleColor>> GetGridRenderingData(
            int minX,
            int maxX,
            int minY,
            int maxY,
            int minZ,
            int maxZ,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            var result = new List<Tuple<string, ConsoleColor>>();
            var builder = new StringBuilder();

            var yDirection = invertY ? -1 : 1;
            var yStart = invertY ? maxY : minY;
            var yEnd = invertY ? minY : maxY;
            int yDiff = Math.Abs(yEnd - yStart);

            var zDirection = invertZ ? -1 : 1;
            var zStart = invertZ ? maxZ : minZ;
            var zEnd = invertZ ? minZ : maxZ;
            int zDiff = Math.Abs(zEnd - zStart);

            for (int zIndex = 0; zIndex <= zDiff; zIndex++)
            {
                int z = zStart + (zIndex * zDirection);
                builder.Append(Environment.NewLine);
                builder.Append(prependText + $"Z: {z}");
                builder.Append(Environment.NewLine);

                for (int yIndex = 0; yIndex <= yDiff; yIndex++)
                {
                    int y = yStart + (yIndex * yDirection);
                    builder.Append(prependText);
                    for (int x = minX; x <= maxX; x++)
                    {
                        var point = new GridPoint3D(x, y, z);
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
            }

            builder.Append(Environment.NewLine);
            result.Add(new Tuple<string, ConsoleColor>(builder.ToString(), Console.ForegroundColor));
            return result;
        }

        /// <summary>
        /// Gets all points along an edge.
        /// <paramref name="edgeValues"/> should contain 4 values:
        /// <paramref name="edgeValues"/>[i] should be the constant value
        /// corresponding to the <paramref name="coordinateOrder"/>[i]th
        /// coordinate, for i in {0, 1}
        /// <paramref name="edgeValues"/>[2] and <paramref name="edgeValues"/>[3]
        /// should be the min and max values of the <paramref name="coordinateOrder"/>[2]nd
        /// coordinate.
        /// </summary>
        /// <param name="edgeValues"></param>
        /// <param name="coordinateOrder"></param>
        /// <returns></returns>
        public static IList<GridPoint3D> GetPointsAlongEdge(
            int[] edgeValues,
            string coordinateOrder)
        {
            if (edgeValues.Length != 4)
                throw new ArgumentException(nameof(edgeValues));
            if (coordinateOrder.Length != 3)
                throw new ArgumentException(nameof(coordinateOrder));
            coordinateOrder = coordinateOrder.ToUpper();
            if (!coordinateOrder.Contains("X"))
                throw new ArgumentException(nameof(coordinateOrder));
            if (!coordinateOrder.Contains("Y"))
                throw new ArgumentException(nameof(coordinateOrder));
            if (!coordinateOrder.Contains("Z"))
                throw new ArgumentException(nameof(coordinateOrder));

            var result = new List<GridPoint3D>();
            for (int rangeCoordinate = edgeValues[2]; rangeCoordinate <= edgeValues[3]; rangeCoordinate++)
            {
                var pointValues = new int[3];
                for (int pointCoordinateIndex = 0; pointCoordinateIndex < 3; pointCoordinateIndex++)
                {
                    var coordinate = "XYZ"[pointCoordinateIndex];
                    var pointCoordinateOrderIndex = coordinateOrder.IndexOf(coordinate);
                    int value;
                    if (pointCoordinateOrderIndex == 2)
                        value = rangeCoordinate;
                    else
                        value = edgeValues[pointCoordinateOrderIndex];
                    pointValues[pointCoordinateIndex] = value;
                }
                var point = new GridPoint3D(pointValues[0], pointValues[1], pointValues[2]);
                result.Add(point);
            }
            return result;
        }
    }
}
