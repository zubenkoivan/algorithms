namespace Algorithms.Compression.Huffman
{
    public static class HuffmanCode
    {
        public static byte[] Compress(byte[] data)
        {
            int[] freqs = CountFrequencies(data);
            TreeNode[] tree = BuildTree(freqs);
            Code[] codeTable = BuildCodeTable(tree);
            byte[] compressedData = Compress(data, freqs, codeTable);

            return compressedData;
        }

        private static int[] CountFrequencies(byte[] data)
        {
            var freqs = new int[256];

            for (int i = 0; i < data.Length; i++)
            {
                freqs[data[i]]++;
            }

            return freqs;
        }

        private static TreeNode[] BuildTree(int[] freqs)
        {
            TreeNode[] heap = CreateHeap(freqs, out int heapEnd);

            for (int i = heap.Length - 2; i > 0; i -= 2)
            {
                TreeNode left = heap[0];
                heap[0] = heap[heapEnd];
                heapEnd--;
                Heapify(heap, heapEnd, 0);
                TreeNode right = heap[0];
                heap[i] = left;
                heap[i + 1] = right;
                heap[0] = new TreeNode(left.Frequency + right.Frequency, i, i + 1);
                Heapify(heap, heapEnd, 0);
            }

            return heap;
        }

        private static TreeNode[] CreateHeap(int[] freqs, out int heapEnd)
        {
            int uniqueBytes = UniqueBytesCount(freqs);
            var heap = new TreeNode[2 * uniqueBytes - 1];
            heapEnd = uniqueBytes - 1;
            int heapifyStart = (heapEnd - 1) >> 1;

            for (int i = 0, j = uniqueBytes; i < freqs.Length; i++)
            {
                if (freqs[i] > 0)
                {
                    j--;
                    heap[j] = new TreeNode((byte) i, freqs[i]);

                    if (j <= heapifyStart)
                    {
                        Heapify(heap, heapEnd, j);
                    }
                }
            }

            return heap;
        }

        private static int UniqueBytesCount(int[] freqs)
        {
            int uniqueBytes = 0;

            for (int i = 0; i < freqs.Length; i++)
            {
                if (freqs[i] != 0) uniqueBytes++;
            }

            return uniqueBytes;
        }

        private static void Heapify(TreeNode[] heap, int end, int nodeIndex)
        {
            while (true)
            {
                int smallest = nodeIndex;
                int left = 2 * nodeIndex + 1;
                int right = 2 * nodeIndex + 2;

                if (left <= end && heap[left].Frequency < heap[smallest].Frequency)
                {
                    smallest = left;
                }

                if (right <= end && heap[right].Frequency < heap[smallest].Frequency)
                {
                    smallest = right;
                }

                if (smallest == nodeIndex)
                {
                    break;
                }

                TreeNode tmp = heap[smallest];
                heap[smallest] = heap[nodeIndex];
                heap[nodeIndex] = tmp;
                nodeIndex = smallest;
            }
        }

        private static Code[] BuildCodeTable(TreeNode[] tree)
        {
            var codeTable = new Code[256];
            var code = new byte[32];
            BuildCodeTable(codeTable, tree, 0, 0, code);
            return codeTable;
        }

        private static void BuildCodeTable(Code[] codeTable, TreeNode[] tree, int node, int codeSize, byte[] code)
        {
            if (tree[node].IsLeaf)
            {
                codeTable[tree[node].Value] = new Code(codeSize, code);
                return;
            }

            BuildCodeTable(codeTable, tree, tree[node].Left, codeSize + 1, code);
            SetBit(code, codeSize);
            BuildCodeTable(codeTable, tree, tree[node].Right, codeSize + 1, code);
            ResetBit(code, codeSize);
        }

        private static void SetBit(byte[] code, int bit)
        {
            code[bit >> 3] |= (byte) (0b1000_0000 >> (bit & 7));
        }

        private static void ResetBit(byte[] code, int bit)
        {
            code[bit >> 3] &= (byte) ~(0b1000_0000 >> (bit & 7));
        }

        private static byte[] Compress(byte[] data, int[] freqs, Code[] codeTable)
        {
            var compressedData = new byte[CompressedDataLength(freqs, codeTable)];
            int codeBitLength = 0;

            for (int i = 0; i < data.Length; i++)
            {
                Code code = codeTable[data[i]];

                code.WriteCode(compressedData, codeBitLength);
                codeBitLength += code.Length;
            }

            return compressedData;
        }

        private static int CompressedDataLength(int[] freqs, Code[] codeTable)
        {
            int bitLength = 0;

            for (int i = 0; i < freqs.Length; i++)
            {
                bitLength += freqs[i] * codeTable[i].Length;
            }

            return ((bitLength - 1) >> 3) + 1;
        }
    }
}