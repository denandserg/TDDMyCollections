using System;
using System.Collections;
using System.Collections.Generic;

namespace TddCollections.Lib
{
    public class MyList<T> : IList<T>, IEnumerator<T>
    {
        private class ListItem
        {
            public T Value { get; set; }
            public ListItem Next { get; set; }
            public static ListItem operator ++(ListItem item) => item.Next;
        }

        private ListItem Head { get; set; }
        private ListItem Tail { get; set; }

        public T this[int index]
        {
            get => GetItem(index).Value;
            set => GetItem(index).Value = value;
        }

        public MyList()
        {
            Count = 0;
            Head = Tail = null;
        }
        public MyList(IEnumerable<T> arr)
        {
            using (IEnumerator<T> e = arr.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    Count = 1;
                    Head = Tail = new ListItem { Value = e.Current };

                    while (e.MoveNext())
                    {
                        Tail = Tail.Next = new ListItem { Value = e.Current };
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
            Tail = Tail.Next = new ListItem { Value = value };
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
        /// <summary>
        /// Метод вставки значения в середину списка
        /// </summary>
        /// <param name="index">индекс элемента, куда будет вставка</param>
        /// <param name="value">значение, которое будет вставлено</param>
        public void Insert(int index, T value)
        {
            //проверяем индекс на диапазон допустимых значений
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            //если индекс 0 - добавляем значение в голову 
            if (index == 0) AddHead(value);
            else
            {
                //получаем ссылку на предыдущий элемент
                ListItem prevItem = GetItem(index - 1);
                prevItem.Next = new ListItem { Value = value, Next = prevItem.Next };
                ++Count;
            }
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
            //если индекс 0 - удаляем значение с головы 
            if (index == 0)
            {
                Head = Head.Next;
                --Count;
            }
            else
            {
                //получаем ссылку на предыдущий элемент
                ListItem prevItem = GetItem(index - 1);
                if (prevItem.Next == Tail)
                {
                    prevItem.Next = null;
                    Tail = prevItem;
                }
                else
                {
                    prevItem.Next = prevItem.Next.Next;
                }               
                --Count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }
        #region IEnumerator

        ListItem currentItem=null;
        public T Current
        {
            get => currentItem.Value;
        }
        object IEnumerator.Current
        {
            get => Current;
        }
        public bool MoveNext()
        {
            if (currentItem == null)
            {
                currentItem = Head;
            }
            else
            {
                ++currentItem;
            }
            return currentItem != null;
        }
        public void Reset()
        {
            currentItem = null;
        }
        void IDisposable.Dispose()
        {
            Reset();
        }
        #endregion
    }
}
