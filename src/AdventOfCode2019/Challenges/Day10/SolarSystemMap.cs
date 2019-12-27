using Radicals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace AdventOfCode2019.Challenges.Day10
{
    public class SolarSystemMap
    {
        private int _width;
        private int _height;
        private Dictionary<SolarGridPoint, SolarObject> _solarObjectDictionary;

        public SolarSystemMap(string[] mapDefinition)
        {
            if (mapDefinition == null)
                throw new Exception("Map definition is null");
            _height = mapDefinition.Length;
            if (mapDefinition.Length > 0)
            {
                _width = mapDefinition[0].Length;
            }
            for (int i = 0; i < mapDefinition.Length; i++)
            {
                if (mapDefinition[i].Length != _width)
                    throw new Exception($"Row of length {mapDefinition[i].Length} different from map width {_width}");
            }
            _solarObjectDictionary = new Dictionary<SolarGridPoint, SolarObject>();
            CreateMapFromDefinition(mapDefinition);
        }

        private void CreateMapFromDefinition(string[] mapDefinition)
        {
            for (int rowIndex = 0; rowIndex < _height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _width; columnIndex++)
                {
                    var currentPointDefinition = mapDefinition[rowIndex][columnIndex];
                    if (currentPointDefinition == '#')
                    {
                        var asteroid = new SolarObject(columnIndex, rowIndex, SolarObjectType.Asteroid);
                        _solarObjectDictionary.Add(asteroid.GridPoint, asteroid);
                    }
                }
            }
        }

        public Tuple<SolarObject, int> GetObjectThatSeesMostOtherObjects()
        {
            int mostObjectsSeen = 0;
            SolarObject bestObjectForVisibility = null;

            // Start multithreading block -- process each object in parallel
            // Initialize dictionary of results for each object
            var resultList = new List<Tuple<SolarObject, int>>();
            int processCount = _solarObjectDictionary.Count;
            var doneEvent = new ManualResetEvent(false);
            foreach (var solarObjectKVP in _solarObjectDictionary)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    var kvp = (KeyValuePair<SolarGridPoint, SolarObject>)obj;
                    // Get the total seen from this object
                    var point = kvp.Key;
                    var solarObject = kvp.Value;
                    var objectsVisibleFromThisObject = GetObjectsVisibleFromPoint(point);

                    // Add the result to the result dictionary
                    lock (resultList)
                    {
                        var resultTuple = new Tuple<SolarObject, int>(
                            solarObject,
                            objectsVisibleFromThisObject.Count);
                        resultList.Add(resultTuple);
                    }

                    if (Interlocked.Decrement(ref processCount) == 0)
                    {
                        doneEvent.Set();
                    }
                }), solarObjectKVP);
            }
            doneEvent.WaitOne();

            // Find the object with the most visible
            var result = resultList
                .OrderByDescending(r => r.Item2)
                .FirstOrDefault();

            return result;
        }

        public IList<SolarObject> GetObjectsVisibleFromPoint(SolarGridPoint point)
        {
            // Starting with the given central point, get all objects lying
            // on the enclosing box of all points adjacent to the central point.
            // For each object on that box, do the following:
            // 1) Mark the object as seen
            // 2) Get the ray extending from the central point through the
            //    object's point, and mark any other objects on that ray as
            //    blocked.
            // Once each object within that enclosing box is checked, repeat 
            // the process by expanding the box to enclose the first box.
            // Continue the process until every object on the map has been
            // checked.
            IList<SolarObject> result = new List<SolarObject>();
            var pointsRemainingToBeChecked = _solarObjectDictionary
                .Select(kvp => kvp.Key)
                .ToHashSet();
            pointsRemainingToBeChecked.Remove(point);
            var pointsAlreadyChecked = new HashSet<SolarGridPoint>();
            int boxNumber = 1;
            while (GetIsNthBoxFromPointPartiallyWithinMap(point, boxNumber))
            {
                IList<SolarObject> objectsInBox = GetSolarObjectsInNthBoxFromPoint(point, boxNumber);
                foreach (var objectInBox in objectsInBox)
                {
                    if (pointsRemainingToBeChecked.Contains(objectInBox.GridPoint))
                        pointsRemainingToBeChecked.Remove(objectInBox.GridPoint);
                    if (pointsAlreadyChecked.Contains(objectInBox.GridPoint))
                        continue;
                    pointsAlreadyChecked.Add(objectInBox.GridPoint);
                    result.Add(objectInBox);

                    // Mark all objects lying on the ray from the central
                    // point through this object's point as checked
                    // (including the current object)
                    // 1) Compute the difference vector from the central point
                    //    to this point
                    // 2a) For each point not already checked, get the difference
                    //    vector from the central point to that point
                    // 2b) Compute the unit vector for both points
                    // Compute the dot product of the two difference vectors;
                    //    If one, then they are parallel.
                    // 2c) If they are parallel, check the signs of the difference
                    //     vector to see if they're in the same direction from
                    //     the central point. If they are, then mark the point
                    //     as blocked.
                    var differenceVector = SolarGridPoint.GetDifferenceVector(point, objectInBox.GridPoint);
                    var differenceUnitVector = VectorHelper.GetUnitVector(differenceVector);
                    var potentiallyBlockedPoints = pointsRemainingToBeChecked.ToHashSet();
                    foreach (var otherPoint in potentiallyBlockedPoints)
                    {
                        var otherDifferenceVector = SolarGridPoint.GetDifferenceVector(point, otherPoint);
                        var pointsLieOnSameRay = VectorHelper.GetAreParallelAndUnidirectional(
                            differenceUnitVector, 
                            new Tuple<Radical, Radical>(otherDifferenceVector.Item1, otherDifferenceVector.Item2));
                        if (pointsLieOnSameRay)
                        {
                            pointsRemainingToBeChecked.Remove(otherPoint);
                            pointsAlreadyChecked.Add(otherPoint);
                        }
                    }
                }
                boxNumber++;
            }
            return result;
        }

        public bool GetIsNthBoxFromPointPartiallyWithinMap(SolarGridPoint point, int N)
        {
            var left = point.X - N;
            var right = point.X + N;
            var top = point.Y - N;
            var bottom = point.Y + N;
            bool isCompletelyOffMap = 
                left < 0 
                && right >= _width
                && top < 0 
                && bottom >= _height;
            return !isCompletelyOffMap;
        }

        public IList<SolarObject> GetSolarObjectsInNthBoxFromPoint(SolarGridPoint point, int N)
        {
            var result = new List<SolarObject>();
            if (N <= 0)
                return result;
            
            // Left side
            var x = point.X - N;
            int y;
            if (x >= 0)
            {
                for (y = point.Y - N; y <= point.Y + N; y++)
                {
                    var coordinate = new SolarGridPoint(x, y);
                    if (_solarObjectDictionary.ContainsKey(coordinate))
                    {
                        result.Add(_solarObjectDictionary[coordinate]);
                    }
                }
            }
            
            // Bottom side
            y = point.Y + N;
            if (y < _height)
            {
                for (x = point.X - N + 1; x <= point.X + N; x++)
                {
                    var coordinate = new SolarGridPoint(x, y);
                    if (_solarObjectDictionary.ContainsKey(coordinate))
                    {
                        result.Add(_solarObjectDictionary[coordinate]);
                    }
                }
            }
            
            // Right side
            x = point.X + N;
            if (x < _width)
            {
                for (y = point.Y + N - 1; y >= point.Y - N; y--)
                {
                    var coordinate = new SolarGridPoint(x, y);
                    if (_solarObjectDictionary.ContainsKey(coordinate))
                    {
                        result.Add(_solarObjectDictionary[coordinate]);
                    }
                }
            }
            
            // Top side
            y = point.Y - N;
            if (y >= 0)
            {
                for (x = point.X + N - 1; x >= point.X - N + 1; x--)
                {
                    var coordinate = new SolarGridPoint(x, y);
                    if (_solarObjectDictionary.ContainsKey(coordinate))
                    {
                        result.Add(_solarObjectDictionary[coordinate]);
                    }
                }
            }
            
            return result;
        }

        public bool GetIsCoordinateInGrid(SolarGridPoint point)
        {
            return GetIsCoordinateInGrid(point.X, point.Y);
        }

        public bool GetIsCoordinateInGrid(int x, int y)
        {
            return x >= 0 && x < _width
                && y >= 0 && y < _height;
        }
    }
}
