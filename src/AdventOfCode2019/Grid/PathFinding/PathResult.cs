using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class PathResult<T>
    {
        public IList<T> Path { get; private set; }
        public int TotalPathCost { get; private set; }
        public PathResult(IList<T> path, int totalPathCost)
        {
            Path = path;
            TotalPathCost = totalPathCost;
        }

        public static PathResult<T> ReconstructPath(PathNode<T> endNode)
        {
            // Starting with the last node in the path, work backwards through
            // the parents to reconstruct the full path
            var currentNode = endNode;
            var path = new List<T>()
            {
                currentNode.Node
            };

            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                path.Insert(0, currentNode.Node);
            }

            var result = new PathResult<T>(path, endNode.GScore);
            return result;
        }

        public static PathResult<T> ReconstructPath(T endNode, Dictionary<T, T> cameFrom, Dictionary<T, int> gScores)
        {
            var currentNode = endNode;
            var path = new List<T>()
            {
                currentNode
            };
            while (cameFrom.ContainsKey(currentNode))
            {
                currentNode = cameFrom[currentNode];
                path.Insert(0, currentNode);
            }
            var result = new PathResult<T>(path, gScores[endNode]);
            return result;
        }

        public override string ToString()
        {
            return $"Path nodes: {Path.Count}, Total cost: {TotalPathCost}";
        }
    }
}
