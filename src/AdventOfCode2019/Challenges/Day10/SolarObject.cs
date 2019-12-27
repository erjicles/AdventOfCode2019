using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Challenges.Day10
{
    public class SolarObject
    {
        private SolarGridPoint _gridPoint;
        private SolarObjectType _type;
        public SolarGridPoint GridPoint
        {
            get
            {
                return _gridPoint;
            }
        }
        public SolarObjectType Type { get; }

        public SolarObject(int x, int y, SolarObjectType type)
        {
            _gridPoint = new SolarGridPoint(x, y);
            _type = type;
        }

        public override string ToString()
        {
            return $"{Type} ({GridPoint})";
        }
    }
}
