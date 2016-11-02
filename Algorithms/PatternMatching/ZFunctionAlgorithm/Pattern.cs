using System.Collections.Generic;

namespace Algorithms.PatternMatching.ZFunctionAlgorithm
{
    public sealed class Pattern : PatternBase
    {
        private readonly ZFunction zFunction;

        public Pattern(string pattern) : base(pattern)
        {
            zFunction = new ZFunction(pattern);
        }

        protected override IEnumerable<int> IndexesInImpl(string text)
        {
            int textIndex = 0;

            foreach (int matchLength in zFunction.Calculate(text))
            {
                if (matchLength == PatternLength)
                {
                    yield return textIndex;
                }

                ++textIndex;
            }
        }
    }
}
