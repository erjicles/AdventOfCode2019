using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Challenges.Day25
{
    public class RobotOutputResult
    {
        public RobotOutputType Type { get; private set; }
        public ShipSectionInfo ShipSectionInfo { get; private set; }
        public string FullText { get; private set; }
        public RobotOutputResult(string fullText, RobotOutputType type)
        {
            InitializeRobotOutputResult(fullText, type, null);
        }
        public RobotOutputResult(
            string fullText,
            RobotOutputType type, 
            ShipSectionInfo shipSectionInfo)
        {
            InitializeRobotOutputResult(fullText, type, shipSectionInfo);
        }
        private void InitializeRobotOutputResult(
            string fullText,
            RobotOutputType type, 
            ShipSectionInfo shipSectionInfo)
        {
            FullText = fullText;
            Type = type;
            ShipSectionInfo = shipSectionInfo;
        }
    }
}
