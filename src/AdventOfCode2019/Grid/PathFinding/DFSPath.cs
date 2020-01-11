using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public static class DFSPath
    {
        /// <summary>
        /// Retrieves the first path found between two points using depth
        /// first search (DFS)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="GetNeighbors"></param>
        /// <param name="GetEdgeCost"></param>
        /// <returns></returns>
        public static PathResult<T> GetDFSPath<T>(
            T start,
            T end,
            Func<T, IList<T>> GetNeighbors,
            Func<T, T, int> GetEdgeCost)
        {
            // Each entry represents a path under construction
            // The first item is the head of the path
            // The second item is the set of all nodes already visited in the path
            var openPaths = new Stack<Tuple<PathNode<T>, HashSet<T>>>();

            var startNode = new PathNode<T>(start, 0);
            var rootPath = new Tuple<PathNode<T>, HashSet<T>>(startNode, new HashSet<T>() { start });
            openPaths.Push(rootPath);

            while (openPaths.Count > 0)
            {
                var path = openPaths.Pop();
                if (end.Equals(path.Item1.Node))
                {
                    var pathResult = PathResult<T>.ReconstructPath(path.Item1);
                    return pathResult;
                }

                var neighbors = GetNeighbors(path.Item1.Node);
                foreach (var neighbor in neighbors)
                {
                    if (path.Item2.Contains(neighbor))
                        continue;
                    int gScore = path.Item1.GScore + GetEdgeCost(path.Item1.Node, neighbor);
                    var neighborNode = new PathNode<T>(neighbor, gScore)
                    {
                        Parent = path.Item1
                    };
                    var alreadyVisited = path.Item2.ToHashSet();
                    alreadyVisited.Add(path.Item1.Node);
                    var newPath = new Tuple<PathNode<T>, HashSet<T>>(neighborNode, alreadyVisited);
                    openPaths.Push(newPath);
                }
            }

            return new PathResult<T>(new List<T>(), 0);

        }
    }
}
