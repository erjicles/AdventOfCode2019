using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class GraphNode<T>
    {
        public T Node { get; private set; }
        public IList<GraphEdge<T>> Edges { get; private set; }
        public GraphNode(T node)
        {
            Node = node;
            Edges = new List<GraphEdge<T>>();
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
