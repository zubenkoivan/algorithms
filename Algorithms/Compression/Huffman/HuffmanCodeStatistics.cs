using System;
using System.Linq;
using System.Text;

namespace Algorithms.Compression.Huffman
{
    internal static class HuffmanCodeStatistics
    {
        public static string Get(int[] freqs, Code[] codeTable, byte[] compressedData)
        {
            var result = new StringBuilder();

            for (int i = 0; i < codeTable.Length; i++)
            {
                if (codeTable[i].Length > 0)
                {
                    result.AppendLine($"{(char)i}: {codeTable[i]}");
                }
            }

            result.AppendLine();
            result.AppendLine(string.Join("", freqs
                .Select((x, i) => new { Freq = x, Char = (char)i })
                .Where(x => x.Freq > 0)
                .Select(x => $"{x.Char};{x.Freq}\n")));

            int size = codeTable
                .Select((x, i) => new { Freq = freqs[i], x.Length })
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
    }
}