using AdventOfCode2019.Grid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Challenges.Day18
{
    public class MazeCell
    {
        public GridPoint Position { get; private set; }
        public MazeCellType Type { get; private set; }
        public string Key { get; set; }
        //public bool ContainsKey { get { return !string.IsNullOrWhiteSpace(Key); } }
        //public string Door { get; set; }
        //public bool ContainsDoor { get { return !string.IsNullOrWhiteSpace(Door); } }
        //public bool IsCurrentPosition { get; private set; } = false;
        public MazeCell(GridPoint position, MazeCellType type)
        {
            Initialize(position, type, string.Empty);
        }
        //public MazeCell(GridPoint position, MazeCellType type, string item)
        //{
        //    Initialize(position, type, item);
        //}

        private void Initialize(GridPoint position, MazeCellType type, string item)
        {
            Position = position;
            Type = type;
            //if (Regex.IsMatch(item, @"^[a-z]$"))
            //{
            //    Key = item;
            //}
            //else if (Regex.IsMatch(item, @"^[A-Z]$"))
            //{
            //    Door = item;
            //}
            //else if (!string.IsNullOrWhiteSpace(item))
            //{
            //    throw new Exception($"Unrecognized cell item {item}");
            //}
        }

        //public static MazeCell CreateMazeCell(GridPoint point, char cellDefinition)
        //{
        //    var mazeCellType = MazeCellType.Empty;
        //    var cellItem = string.Empty;
        //    var isCurrentPosition = false;
        //    if ('#'.Equals(cellDefinition))
        //    {
        //        mazeCellType = MazeCellType.Wall;
        //    }
        //    else if ('@'.Equals(cellDefinition))
        //    {
        //        isCurrentPosition = true;
        //    }
        //    else if (!'.'.Equals(cellDefinition))
        //    {
        //        cellItem = cellDefinition.ToString();
        //    }
        //    var mazeCell = new MazeCell(point, mazeCellType, cellItem)
        //    {
        //        IsCurrentPosition = isCurrentPosition
        //    };
        //    return mazeCell;
        //}

        //public string SetIsCurrentPosition(bool isCurrentPosition)
        //{
        //    IsCurrentPosition = isCurrentPosition;
        //    if (!isCurrentPosition)
        //    {
        //        return string.Empty;
        //    }
        //    if (ContainsDoor)
        //    {
        //        Door = string.Empty;
        //    }
        //    if (ContainsKey)
        //    {
        //        var key = Key;
        //        Key = string.Empty;
        //        return key;
        //    }
        //    return string.Empty;
        //}

        //public void SetIsUnlocked()
        //{
        //    Door = string.Empty;
        //}

        //public bool GetCanEnterCell(HashSet<string> keysCollected)
        //{
        //    if (MazeCellType.Wall.Equals(Type))
        //        return false;
        //    if (ContainsDoor
        //        && !keysCollected.Contains(Door.ToLower()))
        //        return false;
        //    return true;
        //}

        //public string GetCellString()
        //{
        //    if (MazeCellType.Wall.Equals(Type))
        //        return "#";
        //    else if (ContainsKey)
        //        return Key;
        //    else if (ContainsDoor)
        //        return Door;
        //    else if (IsCurrentPosition)
        //        return "@";
        //    return ".";
        //}

        public override string ToString()
        {
            return $"[({Position.X}, {Position.Y}), {Type}]";
        }
    }
}
