using System.Collections.Generic;

namespace Algorithms.TextProcessing.PatternMatching.AhoCorasickAlgorithm
{
    internal class TrieNode
    {
        public readonly int Level;
        private Dictionary<char, TrieNode> nextNodes;
        private TrieNode rollbackNode;

        public TrieNode RollbackTerminalNode { get; private set; }

        public bool IsTerminal { get; private set; }

        public TrieNode(int level)
        {
            Level = level;
        }

        public TrieNode AddNext(char nextSymbol, int patternLength)
        {
            nextNodes = nextNodes ?? new Dictionary<char, TrieNode>();
            TrieNode nextNode = GetNext(nextSymbol) ?? AddNew(nextSymbol);
            nextNode.IsTerminal |= nextNode.Level == patternLength;

            return nextNode;
        }

        private TrieNode AddNew(char nextSymbol)
        {
            TrieNode nextRollbackNode = FindRollbackNode(nextSymbol);
            var nextNode = new TrieNode(Level + 1)
            {
                rollbackNode = nextRollbackNode,
                RollbackTerminalNode = nextRollbackNode.IsTerminal
                    ? nextRollbackNode
                    : nextRollbackNode.RollbackTerminalNode
            };
            nextNodes.Add(nextSymbol, nextNode);

            return nextNode;
        }

        public TrieNode GetNext(char nextSymbol)
        {
            if (nextNodes == null)
            {
                return null;
            }

            TrieNode node;
            return nextNodes.TryGetValue(nextSymbol, out node) ? node : null;
        }

        public TrieNode FindRollbackNode(char nextSymbol)
        {
            TrieNode currentNode = this;

            while (currentNode.rollbackNode != null)
            {
                TrieNode currentRollbackNode = currentNode.rollbackNode;
                TrieNode nextNode = currentRollbackNode.GetNext(nextSymbol);

                if (nextNode != null)
                {
                    return nextNode;
                }

                currentNode = currentNode.rollbackNode;
            }

            return currentNode;
        }
    }
}