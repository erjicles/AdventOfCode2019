using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day25
{
    public static class RobotOutputHelper
    {
        public static RobotOutputResult ProcessRobotOutput(string robotOutput)
        {
            // == Warp Drive Maintenance ==
            // It appears to be working normally.
            //
            // Doors here lead:
            // - north
            // - east
            // - west
            //
            // Items here:
            // - semiconductor
            //
            // Command?
            if (Regex.IsMatch(robotOutput, @"You can't go that way", RegexOptions.IgnoreCase))
            {
                return new RobotOutputResult(robotOutput, RobotOutputType.Blocked);
            }
            if (Regex.IsMatch(robotOutput, @"Unrecognized command", RegexOptions.IgnoreCase))
            {
                return new RobotOutputResult(robotOutput, RobotOutputType.InvalidCommand);
            }
            if (Regex.IsMatch(robotOutput, @"You take the", RegexOptions.IgnoreCase))
            {
                return new RobotOutputResult(robotOutput, RobotOutputType.TookItem);
            }
            if (Regex.IsMatch(robotOutput, @"You drop the", RegexOptions.IgnoreCase))
            {
                return new RobotOutputResult(robotOutput, RobotOutputType.DroppedItem);
            }
            bool isEjectedBack = false;
            if (Regex.IsMatch(robotOutput, @"ejected back to the checkpoint"))
            {
                isEjectedBack = true;
            }

            var patternRoomName = @"==([^=]+)==\s+(.+)\s+Doors here lead\:";
            var patternDirections = @"Doors here lead\:(\s+- (north|east|south|west))+";
            var patternItems = @"Items here\:(\s+-([^-]+))+Command\?";
            var matchRoomName = Regex.Match(robotOutput, patternRoomName);
            var matchDirections = Regex.Match(robotOutput, patternDirections);
            var matchItems = Regex.Match(robotOutput, patternItems);
            if (!matchRoomName.Success)
                throw new Exception("Missing room name");
            if (!matchDirections.Success)
                throw new Exception("Missing directions");
            var roomName = matchRoomName.Groups[1].Value.Trim();
            var roomDescription = matchRoomName.Groups[2].Value.Trim();
            var directions = new HashSet<string>();
            foreach (Capture capture in matchDirections.Groups[2].Captures)
            {
                directions.Add(capture.Value.Trim());
            }
            var items = new HashSet<string>();
            if (matchItems.Success)
            {
                foreach (Capture capture in matchItems.Groups[2].Captures)
                {
                    items.Add(capture.Value.Trim());
                }
            }
            var shipSectionInfo = new ShipSectionInfo(
                name: roomName,
                description: roomDescription,
                directions: directions,
                items: items);
            var type = RobotOutputType.MovedToPoint;
            if (isEjectedBack)
                type = RobotOutputType.EjectedBack;
            return new RobotOutputResult(robotOutput, type, shipSectionInfo);
        }
    }
}
