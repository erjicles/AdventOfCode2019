using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day25
{
    public static class RobotMovementHelper
    {
        public const string DELIMITER = " -> ";

        public static string GetMovementCommandForNeighbors(
            string startPoint,
            string endPoint)
        {
            if (startPoint.Length < endPoint.Length)
            {
                var result = endPoint.Split(DELIMITER).Last();
                return result;
            }
            else
            {
                var lastMovementCommand = startPoint.Split(DELIMITER).Last();
                var result = GetOppositeMovementCommand(lastMovementCommand);
                return result;
            }
        }

        public static bool GetAreAdjacent(string p1, string p2)
        {
            var longerPath = p1.Length > p2.Length ? p1 : p2;
            var shorterPath = p1.Length < p2.Length ? p1 : p2;
            var lengthDifference = longerPath.Length - shorterPath.Length;
            if (lengthDifference == 0)
                return false;
            if (!longerPath.StartsWith(shorterPath))
                return false;
            var additionalPointsOnLongerPath = longerPath.Substring(shorterPath.Length, longerPath.Length - shorterPath.Length);
            if (additionalPointsOnLongerPath.Split(DELIMITER).Length != 2)
                return false;
            return true;
        }

        public static string Move(string point, string movementCommand)
        {
            // Get the last movement in the point
            var pointMovements = point.Split(DELIMITER);
            var lastPointMovement = pointMovements.Last();
            if (GetIsOppositeMovement(movementCommand, lastPointMovement))
            {
                var result = string.Join(DELIMITER, pointMovements, 0, pointMovements.Length - 1);
                return result;
            }
            return point + DELIMITER + movementCommand;
        }

        public static string GetOppositeMovementCommand(string movementCommand)
        {
            return movementCommand switch
            {
                "west" => "east",
                "east" => "west",
                "north" => "south",
                "south" => "north",
                _ => throw new Exception($"Invalid movement command {movementCommand}")
            };
        }

        public static bool GetIsOppositeMovement(string command1, string command2)
        {
            if (command1.Equals(command2))
                return false;
            var eastWest = new string[2] { "east", "west" };
            if (eastWest.Contains(command1) && eastWest.Contains(command2))
                return true;
            var northSouth = new string[2] { "north", "south" };
            if (northSouth.Contains(command1) && northSouth.Contains(command2))
                return true;
            return false;
        }

        public static bool GetIsValidMovementCommand(string movementCommand)
        {
            return movementCommand switch
            {
                "west" => true,
                "north" => true,
                "south" => true,
                "east" => true,
                _ => false,
            };
        }
        
    }
    
}
