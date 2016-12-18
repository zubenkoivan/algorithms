using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Algorithms.Tests
{
    public class TestResult
    {
        public static TestResult Success => new TestResult();

        public bool IsSuccess { get; private set; } = true;
        public List<string> Errors { get; } = new List<string>();

        private TestResult()
        {
        }

        public void AddError(string error)
        {
            IsSuccess = false;
            Errors.Add(error);
        }

        public void AddError(string message, Exception exception)
        {
            IsSuccess = false;
            Errors.Add($"{message} exception '{exception.GetType()}' was thrown; {exception.Message}");
        }

        public void Combine(TestResult testResult)
        {
            IsSuccess &= testResult.IsSuccess;
            Errors.AddRange(testResult.Errors);
        }

        public static TestResult RunTest<TResult>(TResult expected, TResult actual, string message)
        {
            var testResult = new TestResult();

            try
            {
                if (!Equivalent(actual, expected))
                {
                    testResult.AddError($"{message}: expected {expected}, but was {actual}");
                }
            }
            catch (Exception e)
            {
                testResult.AddError($"{message}:", e);
            }

            return testResult;
        }

        private static bool Equivalent<T>(T actual, T expected)
        {
            try
            {
                actual.ShouldBeEquivalentTo(expected);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}