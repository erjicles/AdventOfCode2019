using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public static class MovementDirectionHelper
    {
        public static bool TryParse(
            string directionString, 
            bool invertY,
            out MovementDirection direction)
        {
            direction = MovementDirection.Down;
            bool result = false;
            switch (directionString.ToLower())
            {
                case "east":
                    direction = MovementDirection.Right;
                    result = true;
                    break;
                case "west":
                    direction = MovementDirection.Left;
                    result = true;
                    break;
                case "north":
                    direction = MovementDirection.Up;
                    if (invertY)
                        direction = MovementDirection.Down;
                    result = true;
                    break;
                case "south":
                    direction = MovementDirection.Down;
                    if (invertY)
                        direction = MovementDirection.Up;
                    result = true;
                    break;
            }
            return result;
        }
        public static MovementDirection GetOppositeDirection(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.Down => MovementDirection.Up,
                MovementDirection.Left => MovementDirection.Right,
                MovementDirection.Right => MovementDirection.Left,
                MovementDirection.Up => MovementDirection.Down,
                MovementDirection.In => MovementDirection.Out,
                MovementDirection.Out => MovementDirection.In,
                _ => throw new Exception($"Invalid movement direction {direction}"),
            };
        }
    }
}
