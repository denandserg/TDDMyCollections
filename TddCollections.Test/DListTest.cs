using System;
using TddCollections.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TddCollections.Test
{
    [TestClass]
    public class DListTest
    {
        [TestMethod]
        public void DListCtorTest()
        {
            DList<int> list = new DList<int>();
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(false, list.IsReadOnly);

            DList<string> listA = new DList<string>();
            Assert.IsNotNull(listA);
            Assert.AreEqual(0, listA.Count);
            Assert.AreEqual(false, listA.IsReadOnly);
        }

        [TestMethod]
        public void DListAddTest()
        {
            DList<int> list = new DList<int>();
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
            list.Add(100);
            Assert.AreEqual(1, list.Count);
            for (int i = 0; i < 10000; ++i)
            {
                list.Add(i);
            }
            Assert.AreEqual(10001, list.Count);
        }

        [TestMethod]
        public void DListItemTest_Get()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);

            for (int i = 0; i < arr.Length; ++i)
            {
                Assert.AreEqual(arr[i], list[i]);
            }
        }
        [TestMethod]
        public void DListItemTest_Set()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            list[0] = 100;
            Assert.AreEqual(list[0], 100);
            list[9] = 100;
            Assert.AreEqual(list[9], 100);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DListItemTest_GetException_1()
        {
            DList<int> list = new DList<int>();
            Assert.AreEqual(list[0], 1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DListItemTest_GetException_2()
        {
            DList<int> list = new DList<int>();
            Assert.AreEqual(list[-1], 1);
        }


        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DListItemTest_GetException_3()
        {
            DList<int> list = new DList<int>();
            list[-1] = 100;
        }
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DListItemTest_GetException_4()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            list[10] = 100;
        }

        [TestMethod]
        public void DListClearTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr); ;
            Assert.AreEqual(arr.Length, list.Count);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void DListContainsTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(6));
            Assert.IsTrue(list.Contains(10));
            Assert.IsFalse(list.Contains(-1));
            Assert.IsFalse(list.Contains(25));
            Assert.IsFalse(list.Contains(Int32.MinValue));
            Assert.IsFalse(list.Contains(Int32.MaxValue));
        }

        [TestMethod]
        public void DListEnumeratorTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr); ;

            int index = 0;
            foreach (int val in list)
            {
                Assert.AreEqual(arr[index++], val);
            }
        }
        [TestMethod]
        public void DListCopyToTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr); ;

            int[] mas = new int[list.Count];
            list.CopyTo(mas, 0);
            for (int i = 0; i < arr.Length; ++i)
            {
                Assert.AreEqual(arr[i], mas[i]);
            }
        }

        [TestMethod]
        public void DListIndexOfTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);


            Assert.AreEqual(0, list.IndexOf(1));
            Assert.AreEqual(9, list.IndexOf(10));
            Assert.AreEqual(4, list.IndexOf(5));
            Assert.AreEqual(-1, list.IndexOf(0));
            Assert.AreEqual(-1, list.IndexOf(100));
            Assert.AreEqual(-1, list.IndexOf(Int32.MinValue));
            Assert.AreEqual(-1, list.IndexOf(Int32.MaxValue));
        }

        [TestMethod]
        public void DListRemoveAtTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            list.RemoveAt(0);
            Assert.AreEqual(9, list.Count);
            Assert.AreEqual(2, list[0]);
            list.RemoveAt(8);
            Assert.AreEqual(8, list.Count);
            Assert.AreEqual(9, list[7]);
        }
        [TestMethod]
        public void DListRemoveTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            list.Remove(1);
            Assert.AreEqual(9, list.Count);
            Assert.AreEqual(2, list[0]);
            list.Remove(10);
            Assert.AreEqual(8, list.Count);
            Assert.AreEqual(9, list[7]);
        }
        [TestMethod]
        public void DListInsertTest()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DList<int> list = new DList<int>(arr);
            list.Insert(0, 100);
            Assert.AreEqual(11, list.Count);
            Assert.AreEqual(100, list[0]);
            Assert.AreEqual(1, list[1]);
            list.Insert(10, 1000);
            Assert.AreEqual(12, list.Count);
            Assert.AreEqual(1000, list[10]);
            Assert.AreEqual(10, list[11]);
        }
    }
}
