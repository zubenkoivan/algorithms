namespace Algorithms.BitOperations
{
    public static class BitsSet
    {
        public static int Count(int value)
        {
            value = value - ((value >> 1) & 0x55555555);
            value = (value & 0x33333333) + ((value >> 2) & 0x33333333);

            int count = ((value + (value >> 4) & 0x0F0F0F0F) * 0x01010101) >> 24;

            return count;
        }
    }
}