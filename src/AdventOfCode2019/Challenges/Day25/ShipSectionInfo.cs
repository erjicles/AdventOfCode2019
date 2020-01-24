using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day25
{
    public class ShipSectionInfo
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public HashSet<string> Directions { get; private set; } = new HashSet<string>();
        public HashSet<string> Items { get; private set; } = new HashSet<string>();
        public bool IsBlocked { get; private set; } = false;
        public ShipSectionInfo(bool isBlocked)
        {
            IsBlocked = isBlocked;
        }
        public ShipSectionInfo(
            string name, 
            string description, 
            HashSet<string> directions)
        {
            InitializeShipSection(
                name: name,
                description: description,
                directions: directions,
                items: new HashSet<string>());
        }

        public ShipSectionInfo(
            string name,
            string description,
            HashSet<string> directions,
            HashSet<string> items)
        {
            InitializeShipSection(
                name: name,
                description: description,
                directions: directions,
                items: items);
        }

        private void InitializeShipSection(
            string name,
            string description,
            HashSet<string> directions,
            HashSet<string> items)
        {
            Name = name;
            Description = description;
            Directions = directions;
            Items = items;
        }

        public void SetDirections(HashSet<string> directions)
        {
            Directions = directions;
        }

        public void SetItems(HashSet<string> items)
        {
            Items = items;
        }

        public override string ToString()
        {
            var itemString = string.Empty;
            if (Items.Count > 0)
            {
                itemString = "; Items: " + string.Join(",", Items);
            }
            return $"{Name}: {Description}{itemString}";
        }
    }
}
