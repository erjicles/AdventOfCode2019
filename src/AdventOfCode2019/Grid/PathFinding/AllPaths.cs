using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode2019.Grid.PathFinding
{
    public static class AllPaths
    {
        /// <summary>
        /// Gets all paths between two points. WARNING: The number of paths
        /// between two points grows exponentially with the graph size. Use
        /// at your own risk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="GetNeighbors"></param>
        /// <param name="GetEdgeCost"></param>
        /// <returns></returns>
        public static IList<PathResult<T>> GetAllPaths<T>(
            T start, 
            T end, 
            Func<T, IList<T>> GetNeighbors,
            Func<T, T, int> GetEdgeCost)
        {
            // Each entry represents a path
            // The first item is the head of the path
            // The second item is the set of all nodes already visited in the path
            var result = new List<PathResult<T>>();
            var foundPaths = new List<PathNode<T>>();
            var openPaths = new Queue<Tuple<PathNode<T>, HashSet<T>>>();

            var startNode = new PathNode<T>(start, 0);
            var rootPath = new Tuple<PathNode<T>, HashSet<T>>(startNode, new HashSet<T>() { start });
            openPaths.Enqueue(rootPath);
            while (openPaths.Count > 0)
            {
                var pathsToAddToOpenPath = new Queue<Tuple<PathNode<T>, HashSet<T>>>();
                int processCount = openPaths.Count;
                var doneEvent = new ManualResetEvent(false);
                while (openPaths.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                    {
                        var path = (Tuple<PathNode<T>, HashSet<T>>)obj;
                        if (end.Equals(path.Item1.Node))
                        {
                            lock (foundPaths)
                            {
                                foundPaths.Add(path.Item1);
                            }
                            if (Interlocked.Decrement(ref processCount) == 0)
                            {
                                doneEvent.Set();
                            }
                            return;
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
                            lock (pathsToAddToOpenPath)
                            {
                                pathsToAddToOpenPath.Enqueue(newPath);
                            }
                        }

                        if (Interlocked.Decrement(ref processCount) == 0)
                        {
                            doneEvent.Set();
                        }
                    }), openPaths.Dequeue());
                }
                doneEvent.WaitOne();

                openPaths = pathsToAddToOpenPath;
            }

            foreach (var foundPath in foundPaths)
            {
                var pathResult = PathResult<T>.ReconstructPath(foundPath);
                result.Add(pathResult);
            }
            return result;

        }
    }
}
