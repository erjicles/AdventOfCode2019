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
        public static PathResult<T> GetPath<T>(
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
            int startPointHScore = Heuristic(startPoint);
            var openSet = new SortedDictionary<int, HashSet<T>>();
            openSet.Add(startPointHScore, new HashSet<T>() { startPoint });
            var currentFScores = new Dictionary<T, int>() { { startPoint, startPointHScore } };
            var cameFrom = new Dictionary<T, T>();
            var gScore = new Dictionary<T, int>() { { startPoint, 0 } };

            while (openSet.Count > 0)
            {
                var lowestFScore = openSet.Keys.First();
                var current = openSet[lowestFScore].First();

                if (endPoint.Equals(current))
                {
                    var result = PathResult<T>.ReconstructPath(current, cameFrom, gScore);
                    return result;
                }

                openSet[lowestFScore].Remove(current);
                if (openSet[lowestFScore].Count == 0)
                    openSet.Remove(lowestFScore);
                currentFScores.Remove(current);

                var neighbors = GetNeighbors(current);
                foreach (var neighbor in neighbors)
                {
                    if (!gScore.ContainsKey(neighbor))
                        gScore.Add(neighbor, int.MaxValue);
                    var neighborGScore = gScore[current] + GetEdgeCost(current, neighbor);
                    if (neighborGScore < gScore[neighbor])
                    {
                        if (!cameFrom.ContainsKey(neighbor))
                            cameFrom.Add(neighbor, current);
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = neighborGScore;

                        var neighborFScore = gScore[neighbor] + Heuristic(neighbor);

                        if (currentFScores.ContainsKey(neighbor))
                        {
                            int currentNeighborFScore = currentFScores[neighbor];
                            if (currentNeighborFScore != neighborFScore)
                            {
                                openSet[currentNeighborFScore].Remove(neighbor);
                                if (openSet[currentNeighborFScore].Count == 0)
                                    openSet.Remove(currentNeighborFScore);
                            }
                            currentFScores[neighbor] = neighborFScore;
                        }
                        else
                        {
                            currentFScores.Add(neighbor, neighborFScore);
                        }

                        if (!openSet.ContainsKey(neighborFScore))
                            openSet.Add(neighborFScore, new HashSet<T>());
                        if (!openSet[neighborFScore].Contains(neighbor))
                            openSet[neighborFScore].Add(neighbor);
                        if (!currentFScores.ContainsKey(neighbor))
                            currentFScores.Add(neighbor, neighborFScore);
                    }
                }
            }

            return new PathResult<T>(new List<T>(), 0);
        }
    }
}
