using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using InterviewQuestions;

namespace InterviewQuestionTests
{
    [TestClass]
    public class ArrayTests
    {
        [TestMethod]
        public void TestFindMostFrequent()
        {
            var array = new[] { 1, 2, 4, 3, 6, 6, 8 };
            Assert.AreEqual(array.MostFrequent(), 6);
        }

        [TestMethod]
        public void TestBubbleSortDescending()
        {
            var unsorted = new[] { 1, 5, 2, 9, 8, 7, 3, 10, 4, 6 };
            var sorted = unsorted.BubbleSort(SortOrder.Descending);
            
            Assert.IsTrue(sorted.SequenceEqual(new [] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }));
        }

        [TestMethod]
        public void TestBubbleSortAscending()
        {
            var unsorted = new[] { 1, 5, 2, 9, 8, 7, 3, 10, 4, 6 };
            var sorted = unsorted.BubbleSort(SortOrder.Ascending);

            Assert.IsTrue(sorted.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
        }

        [TestMethod]
        public void TestBinarySearch()
        {
            var letters = new[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

            Assert.AreEqual(letters.BinarySearch("C"), 2);
            Assert.AreEqual(letters.BinarySearch("T"), 19);
            Assert.AreEqual(letters.BinarySearch("F"), 5);
        }
    }
}
