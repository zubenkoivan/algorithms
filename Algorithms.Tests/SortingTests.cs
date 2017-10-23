using System;
using System.Linq;
using Algorithms.Sorting;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SortingTests
    {
        [Fact]
        public void Radix_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            RadixSort.Sort(array, x => x);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Heap_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            HeapSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Insertion_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            InsertionSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Binary_Insertion_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            BinaryInsertionSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Shell_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            ShellSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Merge_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            MergeSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Counting_Sort()
        {
            int[] array = GenerateRandomArray(1000);
            int[] expected = array.OrderBy(x => x).ToArray();

            CountingSort.Sort(array);

            array.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        private static int[] GenerateRandomArray(int maxValue = 10000)
        {
            var array = new int[1000];
            var random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                array[i] = random.Next(maxValue);
            }

            return array;
        }
    }
}