using System;

namespace Algorithms.Tests
{
    public class TestFailException : Exception
    {
        public TestFailException(string message) : base(message)
        {
        }
    }
}