using Beispiel;
using System;
using System.Collections.Generic;
using System.Linq;
using NumberStack = Beispiel.Stack<int>;

namespace Beispiel_To.Test
{
    [TestClass]
    public class StackTest
    {
        // Nicht verwenden
        public int SpecialCounter { get; set; }

        [TestMethod]
        public void Test_IsEmpty_ReturnsTrue()
        {
            // Arrange
            var stack = new NumberStack();

            // Act
            var expected = stack.IsEmpty;

            // Assert
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void Test_Push_AfterPushStackIsNotEmpty()
        {
            // Arrange
            var stack = new NumberStack();

            // Act
            stack.Push(3);

            // Assert
            Assert.IsFalse(stack.IsEmpty);
        }
        

        [TestMethod]
        public void Test_PopEmptyStack_ThrowsInvalidOperationException()
        {
            // Arrange
            var stack = new NumberStack();

            // Act
            var act = stack.Pop;

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => act);
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(-1)]
        [DataRow(int.MaxValue)]
        public void Test_PushAndPop_ReturnsPushedValue(int expected)
        {
            // Arrange
            var stack = new NumberStack();

            // Act
            stack.Push(expected);
            var result = stack.Pop();

            // Assert
            Assert.IsTrue(stack.IsEmpty, "Expected stack to be empty");
            Assert.AreEqual(expected, result);
        }

        public static IEnumerable<object[]> ArrayOfNumbers 
        {
            get
            {
                yield return new object[] { new int[] { 1, 2, 3 } };
                yield return new object[] { new int[] { } };
            }
        }

        [TestMethod]
        [TestCategory("Meine Lieblingstests")]
        [DynamicData(nameof(ArrayOfNumbers))]
        public void Test_PushRangeOfNumbers_RangeIsPushed(int[] expected)
        {
            // Arrange
            var stack = new NumberStack();

            // Act
            foreach (var n in expected)
            {
                stack.Push(n);
            }

            // Assert
            Assert.AreEqual(expected.Length < 1, stack.IsEmpty);

            // Act
            var result = Enumerable.Range(0, expected.Length).Select(i => stack.Pop()).ToArray();

            // Assert
            CollectionAssert.AreEquivalent(expected, result);

            // CollectionAssert auf Objekte funktioniert nicht, aber mit Extension Methods und Reflection ist das einfach realisierbar
            //CollectionAssert.AreEquivalent(new { a, b, c, }, new { a, b c, });
        }
    }
}