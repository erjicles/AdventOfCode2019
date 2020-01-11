using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class Graph<T>
    {
        public Dictionary<T, GraphNode<T>> NodeMap { get; private set; }
        public Dictionary<Tuple<T, T>, GraphEdge<T>> EdgeMap { get; private set; }

        public Graph(IList<T> nodeList, Func<T, T, IList<T>> GetShortestPathBetweenNodes)
        {
            InitializeGraph(nodeList, GetShortestPathBetweenNodes);
        }

        private void InitializeGraph(IList<T> nodeList, Func<T, T, IList<T>> GetShortestPathBetweenNodes)
        {
            NodeMap = new Dictionary<T, GraphNode<T>>();
            EdgeMap = new Dictionary<Tuple<T, T>, GraphEdge<T>>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                var startNode = nodeList[i];
                if (!NodeMap.ContainsKey(startNode))
                    NodeMap.Add(startNode, new GraphNode<T>(startNode));
                var startGraphNode = NodeMap[startNode];

                for (int j = i+1; j < nodeList.Count; j++)
                {
                    var endNode = nodeList[j];
                    var shortestPath = GetShortestPathBetweenNodes(startNode, endNode);
                    if (shortestPath.Count > 0)
                    {
                        if (!NodeMap.ContainsKey(endNode))
                            NodeMap.Add(endNode, new GraphNode<T>(endNode));
                        var endGraphNode = NodeMap[endNode];
                        // -1 because the path includes the start point
                        var graphEdgeFromStartNode = new GraphEdge<T>(startGraphNode, endGraphNode, shortestPath.Count - 1);
                        var graphEdgeFromEndNode = new GraphEdge<T>(endGraphNode, startGraphNode, shortestPath.Count - 1);
                        startGraphNode.Edges.Add(graphEdgeFromStartNode);
                        endGraphNode.Edges.Add(graphEdgeFromEndNode);
                        EdgeMap.Add(new Tuple<T, T>(startNode, endNode), graphEdgeFromStartNode);
                        EdgeMap.Add(new Tuple<T, T>(endNode, startNode), graphEdgeFromEndNode);
                    }
                }
            }
        }

        public IList<GraphNode<T>> GetNodeNeighbors(
            GraphNode<T> node,
            Func<T, bool> GetCanEnterNode)
        {
            var result = new List<GraphNode<T>>();
            foreach (var edge in node.Edges)
            {
                if (GetCanEnterNode(edge.EndNode.Node))
                {
                    result.Add(edge.EndNode);
                }
            }
            return result;
        }

        public PathResult<GraphNode<T>> GetShortestPathViaNodes(
            T start, 
            T end, 
            Func<T, int> Heuristic,
            Func<T, bool> GetCanEnterNode)
        {
            if (!NodeMap.ContainsKey(start))
                throw new Exception("Start point not in graph");
            if (!NodeMap.ContainsKey(end))
                throw new Exception("End point not in graph");
            var startNode = NodeMap[start];
            var endNode = NodeMap[end];

            int NodeHeuristic(GraphNode<T> node)
            {
                return Heuristic(node.Node);
            }

            IList<GraphNode<T>> GetNeighbors(GraphNode<T> node)
            {
                return GetNodeNeighbors(node, GetCanEnterNode);
            }

            int GetEdgeCost(GraphNode<T> start, GraphNode<T> end)
            {
                var edgeKey = new Tuple<T, T>(start.Node, end.Node);
                return EdgeMap[edgeKey].EdgeCost;
            }

            var pathResult = AStar.GetPath(
                startPoint: startNode,
                endPoint: endNode,
                Heuristic: NodeHeuristic,
                GetNeighbors: GetNeighbors,
                GetEdgeCost: GetEdgeCost);

            return pathResult;
        }
    }
}
