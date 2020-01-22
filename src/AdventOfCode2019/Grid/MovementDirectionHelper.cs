using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid
{
    public static class MovementDirectionHelper
    {
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
