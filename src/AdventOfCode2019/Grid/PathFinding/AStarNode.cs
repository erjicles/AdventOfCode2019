using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class AStarNode<T>
    {
        public T Node { get; private set; }
        /// <summary>
        /// Total estimated cost of path through node n (F = G + H)
        /// </summary>
        public int FScore { get { return GScore + HScore; } }
        /// <summary>
        /// Cost so far to reach node n
        /// </summary>
        public int GScore { get; set; }
        /// <summary>
        /// Estimated cost from n to goal (heuristic function)
        /// </summary>
        public int HScore { get; set; }
        public AStarNode<T> Parent { get; set; }

        public AStarNode(T node, int gScore, int hScore)
        {
            Node = node;
            GScore = gScore;
            HScore = hScore;
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
