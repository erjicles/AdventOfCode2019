using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class GraphEdge<T>
    {
        public GraphNode<T> StartNode { get; private set; }
        public GraphNode<T> EndNode { get; private set; }
        public int EdgeCost { get; private set; }
        public GraphEdge(GraphNode<T> startNode, GraphNode<T> endNode, int edgeCost)
        {
            StartNode = startNode;
            EndNode = endNode;
            EdgeCost = edgeCost;
        }

        public override int GetHashCode()
        {
            var edgeTuple = new Tuple<GraphNode<T>, GraphNode<T>>(StartNode, EndNode);
            return edgeTuple.GetHashCode();
        }

        public override string ToString()
        {
            return $"<({StartNode} -> {EndNode}), Cost: {EdgeCost}>";
        }

    }
}
