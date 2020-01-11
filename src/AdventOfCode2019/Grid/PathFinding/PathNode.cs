using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class PathNode<T> : IComparer, IComparer<PathNode<T>>, IComparable, IComparable<PathNode<T>>
    {
        public T Node { get; private set; }
        /// <summary>
        /// Cost so far to reach node n
        /// </summary>
        public int GScore { get; set; }
        public PathNode<T> Parent { get; set; }

        /// <summary>
        /// gScore is the total cost so far to reach this node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="gScore"></param>
        public PathNode(T node, int gScore)
        {
            Node = node;
            GScore = gScore;
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }

        public override string ToString()
        {
            return Node.ToString();
        }

        public virtual int Compare(object x, object y)
        {
            return Compare((PathNode<T>)x, (PathNode<T>)y);
        }

        public int Compare(PathNode<T> x, PathNode<T> y)
        {
            return x.GScore - y.GScore;
        }

        public virtual int CompareTo(object obj)
        {
            return CompareTo((PathNode<T>)obj);
        }

        public int CompareTo(PathNode<T> other)
        {
            return Compare(this, other);
        }

    }
}
