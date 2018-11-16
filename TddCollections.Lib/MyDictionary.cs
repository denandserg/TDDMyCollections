using System;
using System.Collections;
using System.Collections.Generic;

namespace TddCollections.Lib
{
    public class MyDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private class TreeItem
        {
            public KeyValuePair<TKey, TValue> Pair { get; set; }
            public TreeItem Parent { get; set; }
            public TreeItem Left { get; set; }
            public TreeItem Right { get; set; }
        }

        private TreeItem root = null;
        public bool IsRepeatKeys { get; private set; }

        private TreeItem GetItem(TKey key)
        {
            using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
            {
                while (en.MoveNext())
                {
                    if (en.Current.Pair.Key.CompareTo(key) == 0)
                    {
                        return en.Current;
                    }
                }
                return null;
            }
        }

        public MyDictionary(bool isRepeatKeys = false)
        {
            IsRepeatKeys = isRepeatKeys;
        }

        public void Add(TKey key, TValue val)
        {
            Add(new KeyValuePair<TKey, TValue>(key, val));
        }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            if (root == null)
            {
                root = new TreeItem { Pair = pair };
            }
            else
            {
                Add(pair, root);
            }
        }
        private void Add(KeyValuePair<TKey, TValue> pair, TreeItem item)
        {
            ++Count;

            if (!IsRepeatKeys && item.Pair.Key.CompareTo(pair.Key) == 0)
            {
                item.Pair = pair;
                return;
            }

            if (item.Pair.Key.CompareTo(pair.Key) > 0)
            {
                if (item.Left == null)
                    item.Left = new TreeItem { Pair = pair, Parent = item };
                else
                    Add(pair, item.Left);
            }
            else
            {
                if (item.Right == null)
                    item.Right = new TreeItem { Pair = pair, Parent = item };
                else
                    Add(pair, item.Right);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TreeItem item = GetItem(key);
                if (item == null) throw new IndexOutOfRangeException();
                return item.Pair.Value;
            }
            set
            {
                TreeItem item = GetItem(key);
                if (item == null)
                    Add(key, value);
                else
                    item.Pair = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>(Count);
                using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
                {
                    while (en.MoveNext())
                    {
                        keys.Add(en.Current.Pair.Key);
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>(Count);
                using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
                {
                    while (en.MoveNext())
                    {
                        values.Add(en.Current.Pair.Value);
                    }
                }
                return values;
            }
        }

        public int Count
        {
            get;
            private set;
        } = 0;

        public bool IsReadOnly { get => false; }

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            TreeItem item = GetItem(pair.Key);
            return item != null && item.Pair.Value.Equals(pair.Value);
        }

        public bool ContainsKey(TKey key)
        {
            TreeItem item = GetItem(key);
            return item != null;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
            {
                while (en.MoveNext())
                {
                    array[arrayIndex++] = en.Current.Pair;
                }
            }
        }

        public bool Remove(TKey key)
        {
            TreeItem item = GetItem(key);
            if (item == null) return false;
            RemoveItem(item);
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            TreeItem item = GetItem(pair.Key);
            if (item == null || !item.Pair.Value.Equals(pair.Value))
                return false;
            RemoveItem(item);
            return true;
        }

        private void RemoveItem(TreeItem item)
        {
            //Case 0: Item nas not children
            if (item.Left == null && item.Right == null)
                RemoveItemWithoutChildren(item);
            //Case 1: Two children
            else if (item.Left != null && item.Right != null)
                RemoveItemWithTwoChildren(item);
            else
                // Case 2: One child;
                RemoveItemWithOneChild(item);

            --Count;
        }

        private void RemoveItemWithOneChild(TreeItem item)
        {
            if (item.Left == null)
            {
                //Special case: item is root;
                if (item == root)
                {
                    root = item.Right;
                    return;
                }
                //if item is right child
                if (item.Parent.Left == item)
                    item.Parent.Left = item.Right;
                //if item is left child
                else
                    item.Parent.Right = item.Right;
            }
            else
            {
                //Special case: item is root;
                if (item == root)
                {
                    root = item.Left;
                    return;
                }
                //if item is right child
                if (item.Parent.Left == item)
                    item.Parent.Left = item.Left;
                //if item is left child
                else
                    item.Parent.Right = item.Left;
            }
        }

        private void RemoveItemWithTwoChildren(TreeItem item)
        {
            /* =========================================================
              Step 1: Find the successor node in the RIGHT subtree of p
              ========================================================= */

            TreeItem succ = item.Right;    // Starting point: right subtree
            
            while (succ.Left != null)
            {               
                succ = succ.Left;   // Always go left to find min. value
            }
            
            /* ===================================================
               Step 2: replace content of p with successor node
               =================================================== */
            item.Pair = new KeyValuePair<TKey, TValue>(succ.Pair.Key, succ.Pair.Value);

            /* ===================================================
               Step 3: remove successor node
               =================================================== */

            succ.Parent = succ.Right;
        }

        private void RemoveItemWithoutChildren(TreeItem item)
        {
            //Special case: item is root;
            if (item == root)
            {
                root = null;
                return;
            }
            if (item.Parent.Left == item)
            {
                item.Parent.Left = null;
            }
            else
            {
                item.Parent.Right = null;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            TreeItem item = GetItem(key);
            if (item == null)
            {
                value = default(TValue);
                return false;
            }
            value = item.Pair.Value;
            return true;
        }

        private IEnumerator<TreeItem> GetTreeItemEnumerator(TreeItem item)
        {
            Stack<TreeItem> items = new Stack<TreeItem>();
            while (item != null || items.Count != 0)
            {
                if (items.Count != 0)
                {
                    item = items.Pop();
                    yield return item;
                    if (item.Right != null)
                        item = item.Right;
                    else
                        item = null;
                }
                while (item != null)
                {
                    items.Push(item);
                    item = item.Left;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
            {
                while (en.MoveNext())
                {
                    yield return en.Current.Pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
