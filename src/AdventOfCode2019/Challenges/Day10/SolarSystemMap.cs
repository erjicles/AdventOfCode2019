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

        // Assumes the station starts rotating clockwise from vertical
        public SolarObject GetNthAsteroidVaporized(
            SolarGridPoint centerPoint,
            int n)
        {
            if (n < 1)
                throw new ArgumentOutOfRangeException(nameof(n));
            // Initialize the set of objects remaining
            var pointsRemaining = _solarObjectDictionary
                .Select(kvp => kvp.Key)
                .ToHashSet();
            if (pointsRemaining.Contains(centerPoint))
                pointsRemaining.Remove(centerPoint);

            // Initialize the ordered list of objects vaporized
            var objectsVaporized = new List<SolarObject>();

            // On each rotation, get the list of objects vaporized, ordered
            // clockwise from the starting angle
            int currentRotation = 1;
            var startingLaserVector = new Tuple<Radical, Radical>(0, -1);
            while (pointsRemaining.Count > 0)
            {
                // Get the list of objects seen (and therefore vaporized)
                // on this rotation
                var objectsVaporizedOnRotation =
                    GetObjectsVisibleFromPoint(centerPoint, pointsRemaining);

                // Order by clockwise angle
                var objectsVaporizedOnRotationOrdered = objectsVaporizedOnRotation
                    .Select(o => new Tuple<double, SolarObject>(
                        VectorHelper.GetClockwiseAngleBetweenVectors(
                            startingLaserVector,
                            SolarGridPoint.GetDifferenceVector(centerPoint, o.GridPoint).ToRadicalVector()),
                        o))
                    .OrderBy(o => o.Item1)
                    .ToList();

                // Remove the objects vaporized from the remaining list
                foreach (var vaporizedObject in objectsVaporizedOnRotation)
                {
                    if (pointsRemaining.Contains(vaporizedObject.GridPoint))
                    {
                        pointsRemaining.Remove(vaporizedObject.GridPoint);
                    }
                }

                // Add ordered list of objects to
                objectsVaporized.AddRange(objectsVaporizedOnRotationOrdered.Select(o => o.Item2));

                currentRotation++;
            }

            // Get the nth item
            if (n-1 >= objectsVaporized.Count)
                return null;
            return objectsVaporized[n-1];
        }

        public Tuple<SolarObject, int> GetObjectThatSeesMostOtherObjects()
        {
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
                    var pointsToCheck = _solarObjectDictionary
                        .Select(kvp => kvp.Key)
                        .ToHashSet();
                    pointsToCheck.Remove(kvp.Key);
                    var objectsVisibleFromThisObject = 
                        GetObjectsVisibleFromPoint(
                            point,
                            pointsToCheck);

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

        public IList<SolarObject> GetObjectsVisibleFromPoint(
            SolarGridPoint point,
            HashSet<SolarGridPoint> pointsToCheck)
        {
            // Define a "jump vector" as a displacement from the given
            // central point to another point.
            // 1) For each integer n starting with 1, find all jump vectors where
            //    |x| + |y| = n. 
            // 2a) For each jump vector for a given n, move outwards from the
            //     central point in multiples of the given jump vector.
            // 2b) If a point is encountered containing an object that hasn't
            //     been seen before, then mark that object as seen. Afterwards,
            //     mark any subsequent objects encountered as blocked.
            IList<SolarObject> objectsSeen = new List<SolarObject>();
            var pointsChecked = new HashSet<SolarGridPoint>();
            var displacementVectorsChecked = new HashSet<Tuple<int, int>>();
            var numberToCheck = pointsToCheck.Count;
            int jumpNumber = 1;
            while (pointsChecked.Count < numberToCheck)
            {
                // Get all jump vectors for this n
                var jumpVectors = VectorHelper.GetJumpVectors(jumpNumber);

                foreach (var jumpVector in jumpVectors)
                {
                    int jumpStep = 1;
                    var displacementVector = VectorHelper.MultiplyVector(jumpVector, jumpStep);
                    if (displacementVectorsChecked.Contains(displacementVector))
                        continue;
                    displacementVectorsChecked.Add(displacementVector);
                    var currentPoint = SolarGridPoint.GetPointAtRayVector(point, displacementVector);
                    bool encounteredObjectAlongRay = false;
                    while (GetIsCoordinateInGrid(currentPoint))
                    {
                        if (pointsChecked.Contains(currentPoint))
                            continue;

                        // Check if the point contains an object
                        if (pointsToCheck.Contains(currentPoint))
                        {
                            if (!encounteredObjectAlongRay)
                            {
                                objectsSeen.Add(_solarObjectDictionary[currentPoint]);
                                encounteredObjectAlongRay = true;
                            }
                            pointsChecked.Add(currentPoint);
                        }

                        jumpStep++;
                        displacementVector = VectorHelper.MultiplyVector(jumpVector, jumpStep);
                        displacementVectorsChecked.Add(displacementVector);
                        currentPoint = SolarGridPoint.GetPointAtRayVector(point, displacementVector);
                    }
                }

                jumpNumber++;
            }
            return objectsSeen;
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
