using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TddCollections.Lib
{
    public class DList<T> : IList<T>
    {
        private class ListItem
        {
            public T Value { get; set; }
            public ListItem Next { get; set; }
            public ListItem Prev { get; set; }
            public static ListItem operator ++(ListItem item) => item.Next;
        }

        private ListItem Head { get; set; }
        private ListItem Tail { get; set; }

        public DList()
        {
            Count = 0;
            Head = Tail = null;
        }
        public DList(IEnumerable<T> arr)
        {
            using (IEnumerator<T> e = arr.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    Count = 1;
                    Head = Tail = new ListItem { Value = e.Current };

                    while (e.MoveNext())
                    {
                        Tail = Tail.Next = new ListItem { Value = e.Current, Prev=Tail };
                        ++Count;
                    }
                }
                else
                {
                    Count = 0;
                    Head = Tail = null;
                    return;
                }
            }
        }

        public T this[int index]
        {
            get => GetItem(index).Value;
            set => GetItem(index).Value = value;
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get => false; }

        private ListItem GetItem(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException("Index Out Of Range");
            }
            ListItem item = Head;
            while (index-- > 0)
            {
                item = item.Next;
            }
            return item;
        }

        public void Add(T value)
        {
            if (Head == null && Tail == null)
            {
                Head = Tail = new ListItem { Value = value };
                Count = 1;
                return;
            }
            Tail = Tail.Next = new ListItem { Value = value, Prev=Tail };
            ++Count;
        }
        public void AddHead(T value)
        {
            if (Head == null && Tail == null)
            {
                Head = Tail = new ListItem { Value = value };
                Count = 1;
                return;
            }
            Head = new ListItem { Value = value, Next = Head };
            ++Count;
        }

        public void Clear()
        {
            Tail = Head = null;
            Count = 0;
        }

        public bool Contains(T value)
        {
            using (IEnumerator<T> e = GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (e.Current.Equals(value)) return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (IEnumerator<T> e = GetEnumerator())
            {
                while (e.MoveNext())
                {
                    array[arrayIndex++] = e.Current;
                }
            }
        }

        public int IndexOf(T value)
        {
            using (IEnumerator<T> e = GetEnumerator())
            {
                for (int i = 0; e.MoveNext(); ++i)
                {
                    if (e.Current.Equals(value))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, T value)
        {
            //проверяем индекс на диапазон допустимых значений
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            if (index == 0)
            {
                AddHead(value);
                return;
            }

            ListItem item = GetItem(index);

            item.Prev = item.Prev.Next =  new ListItem
            {
                Value = value,
                Next=item,
                Prev=item.Prev
            };
            ++Count;
        }

        public bool Remove(T value)
        {
            int index = IndexOf(value);
            if (index < 0) return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            //проверяем индекс на диапазон допустимых значений
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
          
            ListItem item = GetItem(index);

            if (item == Head)
                Head = item.Next;                
            else
                item.Prev.Next = item.Next;
            
            if(item==Tail)
                Tail = item.Prev;
            else
                item.Next.Prev = item.Prev;

            --Count;           
        }

        private IEnumerator<ListItem> GetListItemEnumerator()
        {
            for(ListItem item=Head; item!=null; ++item)
            {
                yield return item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            using(IEnumerator<ListItem> e = GetListItemEnumerator())
            {
                while (e.MoveNext())
                {
                    yield return e.Current.Value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
