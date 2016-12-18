namespace Algorithms.BitOperations
{
    public static class LogBase2
    {
        private static readonly int[] MultiplyDeBruijnBitPosition =
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
            8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31
        };

        public static int Find(int arg)
        {
            arg |= arg >> 1;
            arg |= arg >> 2;
            arg |= arg >> 4;
            arg |= arg >> 8;
            arg |= arg >> 16;

            return MultiplyDeBruijnBitPosition[(uint) (arg * 0x07C4ACDDU) >> 27];
        }
    }
}
