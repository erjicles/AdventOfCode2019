using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleStreamRenderer
{
    public class ConsoleStreamRenderer
    {
        public int RefreshRateMs { get; set; } = 150;

        public void Render(Frame frame)
        {
            Console.Clear();
            frame.Render();
        }
    }
}
