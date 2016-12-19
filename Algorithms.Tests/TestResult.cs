using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Algorithms.Tests
{
    public class TestResult
    {
        public static TestResult Success => new TestResult();

        private bool IsSuccess { get; set; } = true;
        private List<string> Errors { get; } = new List<string>();

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

        public void AssertSuccess()
        {
            if (IsSuccess)
            {
                return;
            }

            throw new TestFailException($"Total errors count: {Errors.Count}\r\n" + string.Join("\r\n", Errors));
        }

        public static TestResult RunTest<TResult>(TResult expected, Func<TResult> actual, string message)
        {
            var testResult = new TestResult();

            try
            {
                TResult actualResult = actual();

                if (!Equivalent(actualResult, expected))
                {
                    testResult.AddError($"{message}: expected {expected}, but was {actualResult}");
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