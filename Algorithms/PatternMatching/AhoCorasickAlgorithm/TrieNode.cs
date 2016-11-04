using System.Collections.Generic;

namespace Algorithms.PatternMatching.AhoCorasickAlgorithm
{
    internal class TrieNode
    {
        public readonly int Level;
        private Dictionary<char, TrieNode> nextNodes;
        private TrieNode rollbackNode;

        public TrieNode RollbackTerminalNode { get; private set; }

        public bool Terminal { get; private set; }

        public TrieNode(int level)
        {
            Level = level;
        }

        public TrieNode AddNext(char nextSymbol, bool isTerminal)
        {
            nextNodes = nextNodes ?? new Dictionary<char, TrieNode>();

            TrieNode nextNode = GetNext(nextSymbol);

            if (nextNode == null)
            {
                nextNode = new TrieNode(Level + 1)
                {
                    rollbackNode = FindRollbackNode(nextSymbol)
                };
                nextNodes.Add(nextSymbol, nextNode);
                SetRollbackNodes(nextNode, nextSymbol);
            }

            nextNode.Terminal |= isTerminal;

            return nextNode;
        }

        private void SetRollbackNodes(TrieNode nextNode, char nextSymbol)
        {
            TrieNode currentNode = this;

            while (currentNode.rollbackNode != null)
            {
                TrieNode currentRollbackNode = currentNode.rollbackNode;
                TrieNode nextRollbackNode = currentRollbackNode.GetNext(nextSymbol);

                if (nextRollbackNode != null)
                {
                    nextNode.rollbackNode = nextRollbackNode;
                    nextNode.RollbackTerminalNode = nextRollbackNode.Terminal
                        ? nextRollbackNode
                        : nextRollbackNode.RollbackTerminalNode;

                    return;
                }

                currentNode = currentRollbackNode;
            }

            nextNode.rollbackNode = currentNode;
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