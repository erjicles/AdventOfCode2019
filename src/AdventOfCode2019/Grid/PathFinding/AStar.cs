using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public static class AStar
    {
        /// <summary>
        /// Uses the A* pathfinding algorithm to find a path between the
        /// <paramref name="startPoint"/> and <paramref name="endPoint"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        public static IList<T> GetPath<T>(
            T startPoint, 
            T endPoint, 
            Func<T, int> Heuristic,
            Func<T, IList<T>> GetNeighbors,
            Func<T, T, int> GetEdgeCost)
        {
            // Pseudocode found here:
            // https://brilliant.org/wiki/a-star-search/
            //  make an openlist containing only the starting node
            //  make an empty closed list
            //  while (the destination node has not been reached):
            //      consider the node with the lowest f score in the open list
            //      if (this node is our destination node) :
            //          we are finished
            //      if not:
            //          put the current node in the closed list and look at all of its neighbors
            //          for (each neighbor of the current node):
            //              if (neighbor has lower g value than current and is in the closed list) :
            //                  replace the neighbor with the new, lower, g value
            //                  current node is now the neighbor's parent            
            //              else if (current g value is lower and this neighbor is in the open list ) :
            //                  replace the neighbor with the new, lower, g value
            //                  change the neighbor's parent to our current node
            //
            //              else if this neighbor is not in both lists:
            //                  add it to the open list and set its g

            var startNode = new AStarNode<T>(startPoint, 0, Heuristic(startPoint));
            var openSet = new Dictionary<T, AStarNode<T>>()
            {
                { startPoint, startNode }
            };
            var closedSet = new Dictionary<T, AStarNode<T>>();
            while (openSet.Count > 0)
            {
                var currentNode = openSet
                    .OrderBy(kvp => kvp.Value.FScore)
                    .Select(kvp => kvp.Value)
                    .FirstOrDefault();

                if (endPoint.Equals(currentNode.Node))
                {
                    return ReconstructPath(currentNode);
                }

                openSet.Remove(currentNode.Node);
                if (!closedSet.ContainsKey(currentNode.Node))
                    closedSet.Add(currentNode.Node, currentNode);

                var neighborValues = GetNeighbors(currentNode.Node);
                foreach (var neighborValue in neighborValues)
                {
                    int neighborGScore = currentNode.GScore + GetEdgeCost(currentNode.Node, neighborValue);
                    int neighborHScore = Heuristic(neighborValue);
                    bool isInOpenSet = openSet.ContainsKey(neighborValue);
                    bool isInClosedSet = closedSet.ContainsKey(neighborValue);

                    // if (neighbor has lower g value than current and is in the closed list) :
                    //      replace the neighbor with the new, lower, g value
                    //      current node is now the neighbor's parent       
                    if (neighborGScore < currentNode.GScore
                        && isInClosedSet)
                    {
                        var neighborNode = closedSet[neighborValue];
                        neighborNode.GScore = neighborGScore;
                        neighborNode.Parent = currentNode;
                    }
                    // else if (current g value is lower and this neighbor is in the open list ) :
                    //      replace the neighbor with the new, lower, g value
                    //      change the neighbor's parent to our current node
                    else if (currentNode.GScore < neighborGScore
                        && isInOpenSet)
                    {
                        var neighborNode = openSet[neighborValue];
                        neighborNode.GScore = neighborGScore;
                        neighborNode.Parent = currentNode;
                    }
                    // else if this neighbor is not in both lists:
                    //      add it to the open list and set its g
                    else if (!isInOpenSet && !isInClosedSet)
                    {
                        var neighborNode = new AStarNode<T>(neighborValue, neighborGScore, neighborHScore)
                        {
                            Parent = currentNode
                        };
                        openSet.Add(neighborValue, neighborNode);
                    }
                }
            }

            return new List<T>();
        }

        public static IList<T> ReconstructPath<T>(AStarNode<T> endNode)
        {
            // Starting with the last node in the path, work backwards through
            // the parents to reconstruct the full path
            var currentNode = endNode;
            var result = new List<T>()
            {
                currentNode.Node
            };

            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                result.Insert(0, currentNode.Node);
            }
            return result;
        }
    }
}
