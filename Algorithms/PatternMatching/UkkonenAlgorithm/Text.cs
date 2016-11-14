namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    public class Text
    {
        private readonly SuffixTree suffixTree;

        public Text(string text)
        {
            suffixTree = new SuffixTree(text);
        }

        public bool HasPattern(string pattern)
        {
            return suffixTree.HasPattern(pattern);
        }
    }
}