using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleStreamRenderer
{
    public class Frame
    {
        public IList<Tuple<string, ConsoleColor>> Blocks { get; set; }
        public ConsoleColor DefaultColor { get; set; } = Console.ForegroundColor;

        public Frame(IList<Tuple<string, ConsoleColor>> blocks)
        {
            Blocks = blocks;
        }

        public void Render()
        {
            foreach (var block in Blocks)
            {
                Console.ForegroundColor = block.Item2;
                Console.Write(block.Item1);
                Console.ForegroundColor = DefaultColor;
            }
        }
    }
}
