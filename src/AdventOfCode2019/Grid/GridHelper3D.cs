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
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            var renderingData = GetGridRenderingData(
                gridPoints: gridPoints,
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
            ICollection<GridPoint3D> gridPoints,
            Func<GridPoint3D, string> GetPointString,
            Func<GridPoint3D, ConsoleColor> GetPointColor,
            string prependText,
            bool invertY,
            bool invertZ)
        {
            var result = new List<Tuple<string, ConsoleColor>>();
            var builder = new StringBuilder();

            int minX = gridPoints.Min(p => p.X);
            int maxX = gridPoints.Max(p => p.X);
            int minY = gridPoints.Min(p => p.Y);
            int maxY = gridPoints.Max(p => p.Y);
            int minZ = gridPoints.Min(p => p.Z);
            int maxZ = gridPoints.Max(p => p.Z);

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
    }
}
