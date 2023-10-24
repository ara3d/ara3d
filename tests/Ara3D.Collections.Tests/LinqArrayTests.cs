using NUnit.Framework;
using System;

namespace Ara3D.Collections.Tests
{
    public class LinqArrayTests
    {
        public static int[] ArrayToTen = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static IArray<int> RangeToTen = 10.Range();
        public static IArray<int> BuildToTen = LinqArray.Build(0, x => x + 1, x => x < 10);

        public static object[] TensData = { ArrayToTen.ToIArray(), RangeToTen, BuildToTen };

        [TestCaseSource(nameof(TensData))]
        public void CheckTens(IArray<int> tens)
        {
            Assert.IsTrue(tens.SequenceEquals(ArrayToTen.ToIArray()));
            Assert.AreEqual(0, tens.First());
            Assert.AreEqual(9, tens.Last());
            Assert.AreEqual(45, tens.Aggregate(0, (a, b) => a + b));
            Assert.AreEqual(10, tens.Count);
            Assert.AreEqual(5, tens[5]);
            Assert.AreEqual(5, tens.ElementAt(5));

            var ones = 1.Repeat(9);
            var diffs = tens.ZipEachWithNext((x, y) => y - x);
            Assert.IsTrue(ones.SequenceEquals(diffs));
            Assert.IsFalse(ones.SequenceEquals(tens));

            var indices = tens.Indices();
            Assert.IsTrue(tens.SequenceEquals(indices));
            Assert.IsTrue(tens.SequenceEquals(tens.SelectByIndex(indices)));
            Assert.IsTrue(tens.Reverse().SequenceEquals(tens.SelectByIndex(indices.Reverse())));

            var sum = 0;
            foreach (var x in tens.ToEnumerable())
            {
                sum += x;
            }
            foreach (var x in tens.ToEnumerable())
            {
                Console.WriteLine(x.ToString());
            }
            Assert.AreEqual(45, sum);
            Assert.AreEqual(0, tens.First());
            Assert.True(tens.All(x => x < 10));
            Assert.True(tens.Any(x => x < 5));
            Assert.AreEqual(5, tens.CountWhere(x => x % 2 == 0));
            Assert.AreEqual(0, tens.Reverse().Last());
            Assert.AreEqual(0, tens.Reverse().Reverse().First());
            var split = tens.Split(LinqArray.Create(3, 6));
            Assert.AreEqual(3, split.Count);

            var batch = tens.SubArrays(3);
            Assert.AreEqual(4, batch.Count);
            Assert.True(batch[0].SequenceEquals(LinqArray.Create(0, 1, 2)));
            Assert.True(batch[3].SequenceEquals(LinqArray.Create(9)));

            var batch2 = tens.Take(9).SubArrays(3);
            Assert.AreEqual(3, batch2.Count);

            var counts = split.Select(x => x.Count);
            Assert.True(counts.SequenceEquals(LinqArray.Create(3, 3, 4)));
            var indices2 = counts.Accumulate((x, y) => x + y);
            Assert.True(indices2.SequenceEquals(LinqArray.Create(3, 6, 10)));
            var indices3 = counts.PostAccumulate((x, y) => x + y);
            Assert.True(indices3.SequenceEquals(LinqArray.Create(0, 3, 6, 10)));
            var flattened = split.Flatten();
            Assert.True(flattened.SequenceEquals(tens));
        }
    }
}
