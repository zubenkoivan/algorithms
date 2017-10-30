using System;
using System.Linq;
using System.Text;

namespace Algorithms.Compression
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

        private static string ResultToString(int[] freqs, Code[] codeTable, byte[] compressedData)
        {
            var result = new StringBuilder();

            for (int i = 0; i < codeTable.Length; i++)
            {
                if (codeTable[i].Length > 0)
                {
                    result.AppendLine($"{(char) i}: {codeTable[i]}");
                }
            }

            result.AppendLine();
            result.AppendLine(string.Join("", freqs
                .Select((x, i) => new {Freq = x, Char = (char) i})
                .Where(x => x.Freq > 0)
                .Select(x => $"{x.Char};{x.Freq}\n")));

            int size = codeTable
                .Select((x, i) => new {Freq = freqs[i], x.Length})
                .Sum(x => x.Freq * x.Length);

            result.AppendLine();
            result.AppendLine($"Compressed data length: {size} bits");
            result.AppendLine();

            for (int i = 0; i < compressedData.Length; i++)
            {
                result.Append(Convert.ToString(compressedData[i], 2).PadLeft(8, '0'));
            }

            return result.ToString();
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

                if (code.Length > 24)
                {
                    WriteLongCode(code, compressedData, codeBitLength);
                }
                else
                {
                    WriteShortCode(code, compressedData, codeBitLength);
                }

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

        private static void WriteLongCode(Code code, byte[] data, int currDataBitLength)
        {
            int shift = currDataBitLength & 7;
            int currDataIndex = currDataBitLength - 1 >> 3;

            for (int i = 0; i < code.Bytes.Length; i++)
            {
                int currCode = code.Bytes[i] << (8 - shift);
                data[currDataIndex] |= (byte) (currCode >> 8);
                currDataIndex++;

                if (currDataIndex < data.Length)
                {
                    data[currDataIndex] = (byte) currCode;
                }
            }
        }

        private static void WriteShortCode(Code code, byte[] data, int currDataBitLength)
        {
            int shift = currDataBitLength & 7;
            int currDataIndex = currDataBitLength >> 3;
            int bytesCount = ((code.Length - 1) >> 3) + 1;
            int bytes = code.Byte1 | (code.Byte2 << 8) | (code.Byte3 << 16);

            for (int i = 0; i < bytesCount; i++)
            {
                int currCode = (bytes & 0xFF) << (8 - shift);
                data[currDataIndex] |= (byte) (currCode >> 8);
                currDataIndex++;

                if (currDataIndex < data.Length)
                {
                    data[currDataIndex] = (byte) currCode;
                }

                bytes >>= 8;
            }
        }

        private struct TreeNode
        {
            public readonly int Frequency;
            public readonly int Left;
            public readonly int Right;
            public readonly byte Value;

            public TreeNode(byte value, int frequency)
            {
                Value = value;
                Frequency = frequency;
                Left = -1;
                Right = -1;
            }

            public TreeNode(int frequency, int left, int right)
            {
                Value = 0;
                Frequency = frequency;
                Left = left;
                Right = right;
            }

            public bool IsLeaf => Left == -1;

            public override string ToString()
            {
                return IsLeaf ? $"{(char) Value}: {Frequency}" : $"{Left},{Right}: {Frequency}";
            }
        }

        private struct Code
        {
            public readonly byte Length;
            public readonly byte Byte1;
            public readonly byte Byte2;
            public readonly byte Byte3;
            public readonly byte[] Bytes;

            public Code(int codeSize, byte[] code)
            {
                Length = (byte) codeSize;
                Byte1 = code[0];
                Byte2 = code[1];
                Byte3 = code[2];

                if (codeSize > 24)
                {
                    Bytes = new byte[((codeSize - 1) >> 3) + 1];
                    Array.Copy(code, Bytes, Bytes.Length);
                }
                else
                {
                    Bytes = null;
                }
            }

            public override string ToString()
            {
                var result = new StringBuilder();

                if (Length > 24)
                {
                    for (int i = 0; i < Bytes.Length; i++)
                    {
                        result.Append(Convert.ToString(Bytes[i], 2).PadLeft(8, '0'));
                    }
                }
                else
                {
                    int bytes = (Byte1 << 16) | (Byte2 << 8) | Byte3;
                    result.AppendFormat(Convert.ToString(bytes, 2).PadLeft(24, '0'));
                }

                return result.ToString(0, Length);
            }
        }
    }
}