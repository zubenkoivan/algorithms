namespace Algorithms.PatternMatching.AhoCorasickAlgorithm
{
    internal abstract class ReadOnlyTrieNode
    {
        public abstract bool Terminal { get; protected set; }
        public abstract ReadOnlyTrieNode RollbackTerminalNode { get; }
        public abstract int Level { get; }
        public abstract ReadOnlyTrieNode GetNext(char nextSymbol);
        public abstract ReadOnlyTrieNode FindRollbackNode(char nextSymbol);
    }
}