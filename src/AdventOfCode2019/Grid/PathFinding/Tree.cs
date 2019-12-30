using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Grid.PathFinding
{
    public class Tree<T>
    {
        public Dictionary<T, TreeBranch<T>> Index { get; private set; } = 
            new Dictionary<T, TreeBranch<T>>();
        public TreeBranch<T> RootBranch { get; private set; }

        public Tree()
        {
            InitializeTree(default);
        }

        private void InitializeTree(T rootItem)
        {
            var rootBranch = new TreeBranch<T>(default, null);
            RootBranch = rootBranch;
        }

        /// <summary>
        /// Adds an item to the tree as a child to the given parent.
        /// If no parent is specified, then the item is added as a child to the
        /// root.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parentItem"></param>
        public void Add(T item, T parentItem)
        {
            if (parentItem != null
                && !Index.ContainsKey(parentItem))
                throw new Exception("Parent is not in the tree");
            if (Index.ContainsKey(item))
                throw new Exception("Item already in the tree");
            var parentBranch = RootBranch;
            if (parentItem != null)
                parentBranch = Index[parentItem];
            var itemBranch = new TreeBranch<T>(item, parentBranch);
            Index.Add(item, itemBranch);
            parentBranch.ChildBranches.Add(itemBranch);
        }

        public void Add(ICollection<T> items, T parentItem)
        {
            foreach (var item in items)
            {
                Add(item, parentItem);
            }
        }

        public void Remove(T item)
        {
            if (!Index.ContainsKey(item))
                throw new Exception("Item is not in the tree");
            var itemBranch = Index[item];
            Index.Remove(item);
            var parentBranch = itemBranch.ParentBranch;
            parentBranch.ChildBranches.Remove(itemBranch);
        }

        public bool Contains(T item)
        {
            return Index.ContainsKey(item);
        }

        public int Count { get { return Index.Count; } }

        public T Find()
        {
            return Find(default);
        }
        public T Find(T startPoint)
        {
            bool GetisValid(T item)
            {
                return true;
            }
            return FindAndTrim(startPoint, GetisValid);
        }

        /// <summary>
        /// Searches for a valid item meeting the given 
        /// <paramref name="GetIsValid"/> function.
        /// Starts by searching the <paramref name="startPoint"/>'s children,
        /// then searches among its siblings, and then backtracks up the
        /// branches of the tree. Returns null if none are found.
        /// Removes invalid child branches.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="GetIsValid"></param>
        /// <returns></returns>
        public T FindAndTrim(T startPoint, Func<T, bool> GetIsValid)
        {
            var alreadyChecked = new HashSet<T>();
            var currentItem = startPoint;
            while (Index.Count > 0)
            {
                if (currentItem != null
                    && !alreadyChecked.Contains(currentItem)
                    && GetIsValid(currentItem))
                    return currentItem;
                var branch = RootBranch;
                if (currentItem != null)
                {
                    alreadyChecked.Add(currentItem);
                    branch = Index[currentItem];
                }
                if (branch.ChildBranches.Count > 0)
                {
                    currentItem = branch.ChildBranches[0].Item;
                }
                else
                {
                    if (currentItem != null)
                        Remove(currentItem);
                    currentItem = branch.ParentBranch.Item;
                }
            }
            return default;
        }
    }
}
