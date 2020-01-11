using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleStreamRenderer
{
    public class ConsoleStreamRenderer
    {
        public IList<Frame> Frames { get; set; }
        public int RefreshRateMs { get; set; } = 150;

        public void Render()
        {
            foreach (var frame in Frames)
            {
                Console.Clear();
                frame.Render();
            }
        }
    }
}
