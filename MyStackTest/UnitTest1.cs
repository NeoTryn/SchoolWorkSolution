using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyStackTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAll()
        {
            MyStack.Stack<int> stack = new MyStack.Stack<int>(5);
            stack.push(1);
            stack.push(2);
            stack.push(3);
            stack.push(4);
            stack.push(5);
            Console.WriteLine(stack.List);
            Assert.AreEqual(5, stack.Size);
            Assert.AreEqual(5, stack.peek());
            stack.pop();
            Assert.AreEqual(4, stack.peek());
        }
    }
}