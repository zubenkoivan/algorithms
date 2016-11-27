using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.TextProcessing.PatternMatching.AhoCorasickAlgorithm
{
    public class Trie
    {
        private readonly TrieNode root;

        public Trie(params string[] patterns)
        {
            if (patterns.Length == 0)
            {
                throw new ArgumentException("is empty", nameof(patterns));
            }

            if (patterns.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException("one of patterns is empty", nameof(patterns));
            }

            root = new TrieNode(0);
            BuildTrie(patterns);
        }

        private void BuildTrie(string[] patterns)
        {
            TrieNode[] previousLevelNodes = CreateNodesArray(patterns.Length, root);
            int s = 0;

            for (bool isBuilt = false; !isBuilt;)
            {
                isBuilt = true;

                for (int i = 0; i < patterns.Length; i++)
                {
                    string pattern = patterns[i];

                    if (pattern.Length <= s)
                    {
                        continue;
                    }

                    isBuilt = false;
                    TrieNode nextNode = previousLevelNodes[i].AddNext(pattern[s], pattern.Length);
                    previousLevelNodes[i] = nextNode;
                }

                ++s;
            }
        }

        private static TrieNode[] CreateNodesArray(int length, TrieNode node)
        {
            var nodes = new TrieNode[length];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = node;
            }

            return nodes;
        }

        public IEnumerable<PatternLocation> LocationsIn(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("is empty", nameof(text));
            }

            TrieNode currentNode = root;

            for (int i = 0; i < text.Length; i++)
            {
                TrieNode nextNode = currentNode.GetNext(text[i]);

                if (nextNode == null)
                {
                    currentNode = currentNode.FindRollbackNode(text[i]);
                    continue;
                }

                if (nextNode.IsTerminal)
                {
                    yield return PatternLocation(i, nextNode);
                }
                else if (nextNode.RollbackTerminalNode != null)
                {
                    yield return PatternLocation(i, nextNode.RollbackTerminalNode);
                }

                currentNode = nextNode;
            }
        }

        private static PatternLocation PatternLocation(int textIndex, TrieNode node)
        {
            return new PatternLocation(textIndex - node.Level + 1, node.Level);
        }
    }
}