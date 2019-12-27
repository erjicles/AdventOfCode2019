using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day12
{
    public class Moon
    {
        public GridPoint3D Position { get; set; }
        public GridPoint3D Velocity { get; set; }

        public Moon()
        {
            Position = new GridPoint3D();
            Velocity = new GridPoint3D();
        }

        public Moon(int x, int y, int z)
        {
            Position = new GridPoint3D(x, y, z);
            Velocity = new GridPoint3D();
        }

        public Moon(GridPoint3D position)
        {
            Position = position.Move(new Tuple<int, int, int>(0, 0, 0));
            Velocity = new GridPoint3D();
        }

        public Moon(int x, int y, int z, int vX, int vY, int vZ)
        {
            Position = new GridPoint3D(x, y, z);
            Velocity = new GridPoint3D(vX, vY, vZ);
        }

        public Moon(GridPoint3D position, GridPoint3D velocity)
        {
            Position = position.Move(new Tuple<int, int, int>(0, 0, 0));
            Velocity = velocity.Move(new Tuple<int, int, int>(0, 0, 0));
        }

        public void UpdatePosition()
        {
            Position = Position.Move(Velocity);
        }

        public void UpdateVelocity(Tuple<int, int, int> acceleration)
        {
            Velocity = Velocity.Move(acceleration);
        }

        public int GetTotalEnergy()
        {
            // The total energy for a single moon is its potential energy 
            // multiplied by its kinetic energy.  
            return GetPotentialEnergy() * GetKineticEnergy();
        }

        public int GetPotentialEnergy()
        {
            // A moon's potential energy is the sum of the absolute values of 
            // its x, y, and z position coordinates.
            return Math.Abs(Position.X)
                + Math.Abs(Position.Y)
                + Math.Abs(Position.Z);
        }

        public int GetKineticEnergy()
        {
            // A moon's kinetic energy is the sum of the absolute values of 
            // its velocity coordinates.
            return Math.Abs(Velocity.X)
                + Math.Abs(Velocity.Y)
                + Math.Abs(Velocity.Z);
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Moon m = (Moon)obj;
                return Position.Equals(m.Position)
                    && Velocity.Equals(m.Velocity);
            }
        }

        public override int GetHashCode()
        {
            var tuple = new Tuple<int, int, int, int, int, int>(
                Position.X, Position.Y, Position.Z,
                Velocity.X, Velocity.Y, Velocity.Z);
            int hash = tuple.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return String.Format("Pos:<{0}, {1}, {2}>, Vel:<{3}, {4}, {5}>", 
                Position.X, Position.Y, Position.Z,
                Velocity.X, Velocity.Y, Velocity.Z);
        }

        public string GetCoordinateString(int coordinate)
        {
            if (coordinate == 0)
                return String.Format("Pos:<{0}>, Vel:<{1}", Position.X, Velocity.X);
            else if (coordinate == 1)
                return String.Format("Pos:<{0}>, Vel:<{1}", Position.Y, Velocity.Y);
            else if (coordinate == 2)
                return String.Format("Pos:<{0}>, Vel:<{1}", Position.Z, Velocity.Z);
            else
                throw new Exception($"Invalid coordinate {coordinate}");
        }
    }
}
