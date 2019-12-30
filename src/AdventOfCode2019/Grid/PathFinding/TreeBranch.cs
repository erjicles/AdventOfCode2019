using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class TreeBranch<T>
    {
        public T Item { get; set; }
        public TreeBranch<T> ParentBranch { get; set; }
        public IList<TreeBranch<T>> ChildBranches { get; set; } = 
            new List<TreeBranch<T>>();
    
        public TreeBranch(T item, TreeBranch<T> parentBranch)
        {
            Item = item;
            ParentBranch = parentBranch;
        }
    }
}
