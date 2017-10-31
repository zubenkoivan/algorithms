using System;
using System.Text;

namespace Algorithms.Compression.Huffman
{
    internal struct Code
    {
        public readonly byte Length;
        private readonly byte byte1;
        private readonly byte byte2;
        private readonly byte byte3;
        private readonly byte[] bytes;

        public Code(int codeSize, byte[] code)
        {
            Length = (byte)codeSize;
            byte1 = code[0];
            byte2 = code[1];
            byte3 = code[2];

            if (codeSize > 24)
            {
                bytes = new byte[((codeSize - 1) >> 3) + 1];
                Array.Copy(code, bytes, bytes.Length);
            }
            else
            {
                bytes = null;
            }
        }

        public void WriteCode(byte[] data, int currDataBitLength)
        {
            if (Length > 24)
            {
                WriteLongCode(data, currDataBitLength);
            }
            else
            {
                WriteShortCode(data, currDataBitLength);
            }
        }

        private void WriteLongCode(byte[] data, int currDataBitLength)
        {
            int shift = currDataBitLength & 7;
            int currDataIndex = currDataBitLength - 1 >> 3;

            for (int i = 0; i < bytes.Length; i++)
            {
                int currCode = bytes[i] << (8 - shift);
                data[currDataIndex] |= (byte)(currCode >> 8);
                currDataIndex++;

                if (currDataIndex < data.Length)
                {
                    data[currDataIndex] = (byte)currCode;
                }
            }
        }

        private void WriteShortCode(byte[] data, int currDataBitLength)
        {
            int shift = currDataBitLength & 7;
            int currDataIndex = currDataBitLength >> 3;
            int bytesCount = ((Length - 1) >> 3) + 1;
            int bytes = byte1 | (byte2 << 8) | (byte3 << 16);

            for (int i = 0; i < bytesCount; i++)
            {
                int currCode = (bytes & 0xFF) << (8 - shift);
                data[currDataIndex] |= (byte)(currCode >> 8);
                currDataIndex++;

                if (currDataIndex < data.Length)
                {
                    data[currDataIndex] = (byte)currCode;
                }

                bytes >>= 8;
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            if (Length > 24)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    result.Append(Convert.ToString(bytes[i], 2).PadLeft(8, '0'));
                }
            }
            else
            {
                int bytes = (byte1 << 16) | (byte2 << 8) | byte3;
                result.AppendFormat(Convert.ToString(bytes, 2).PadLeft(24, '0'));
            }

            return result.ToString(0, Length);
        }
    }
}