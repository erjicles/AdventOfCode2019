using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class AStarNode<T> : PathNode<T>, IComparer, IComparer<AStarNode<T>>, IComparable, IComparable<AStarNode<T>>
    {
        /// <summary>
        /// Total estimated cost of path through node n (F = G + H)
        /// </summary>
        public int FScore { get { return GScore + HScore; } }
        /// <summary>
        /// Estimated cost from n to goal (heuristic function)
        /// </summary>
        public int HScore { get; set; }

        public AStarNode(T node, int gScore, int hScore)
            : base(node, gScore)
        {
            HScore = hScore;
        }

        public override int Compare(object x, object y)
        {
            return Compare((AStarNode<T>)x, (AStarNode<T>)y);
        }

        public int Compare(AStarNode<T> x, AStarNode<T> y)
        {
            return x.FScore - y.FScore;
        }

        public override int CompareTo(object obj)
        {
            return CompareTo((AStarNode<T>)obj);
        }

        public int CompareTo([AllowNull] AStarNode<T> other)
        {
            return Compare(this, other);
        }
        
    }
}
