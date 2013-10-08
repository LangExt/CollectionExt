using System;
using LangExt;
using SeqExt.Mutable;
using NUnit.Framework;

namespace SeqExt.Tests.Mutable
{
    [TestFixture]
    public class VarArrayTest
    {
        [Test]
        public void 空のVarArrayが作れる()
        {
            Assert.That(new VarArray<int>(), Is.Empty);
        }

        [TestCase(new int[0], new int[0])]
        [TestCase(new[] { 1 }, new[] { 1 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
        public void シーケンスからVarArrayが作れる(int[] xs, int[] expected)
        {
            Assert.That(new VarArray<int>(xs.ToSeq()).ToArray(), Is.EqualTo(expected));
        }

        [Test]
        public void コレクション初期化子が使える()
        {
            Assert.That(new VarArray<int> { 1, 2, 3, 4 }.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [TestCase(new int[0], 0)]
        [TestCase(new[] { 1 }, 1)]
        [TestCase(new[] { 1, 2, 3, 4 }, 4)]
        public void Sizeが取れる(int[] xs, int expected)
        {
            Assert.That(new VarArray<int>(xs.ToSeq()).Size, Is.EqualTo(expected));
        }

        [Test]
        public void 要素がAdd出来る()
        {
            var xs = new VarArray<int>();
            xs.Add(1);
            xs.Add(2);
            Assert.That(xs.ToArray(), Is.EqualTo(new[] { 1, 2 }));
        }

        [Test]
        public void シーケンスがAddAll出来る()
        {
            var xs = new VarArray<int>();
            xs.Add(1);
            xs.AddAll(Seq.Init(3, x => x + 2));
            Assert.That(xs.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void 空のシーケンスをAddAllしても変わらない()
        {
            var xs = new VarArray<int> { 1, 2, 3 };
            xs.AddAll(Seq.Empty<int>());
            Assert.That(xs.ToArray(), Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [TestCase(new int[0], 0, 1, new[] { 1 })]
        [TestCase(new int[0], 1, 1, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new int[0], -1, 1, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1, 2, 3 }, 0, 0, new[] { 0, 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 2, 0, new[] { 1, 2, 0, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 3, 0, new[] { 1, 2, 3, 0 })]
        [TestCase(new[] { 1, 2, 3 }, 4, 0, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1, 2, 3 }, -1, 0, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        public void 要素がInsert出来る　(int[] xs, int index, int item, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            array.Insert(index, item);
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], 0, new int[0], new int[0])]
        [TestCase(new int[0], 0, new[] { 1 }, new[] { 1 })]
        [TestCase(new int[0], 0, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(new int[0], 1, new[] { 1 }, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new int[0], -1, new[] { 1 }, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1, 2, 3 }, 0, new int[0], new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 0, new[] { 0, 0 }, new[] { 0, 0, 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 2, new int[0], new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 2, new[] { 0, 0 }, new[] { 1, 2, 0, 0, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 3, new int[0], new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 3, new[] { 0, 0 }, new[] { 1, 2, 3, 0, 0 })]
        [TestCase(new[] { 1, 2, 3 }, 4, new[] { 0, 0 }, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1, 2, 3 }, -1, new[] { 0, 0 }, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        public void シーケンスがInsertAll出来る(int[] xs, int index, int[] items, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            array.InsertAll(index, items.ToSeq());
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], 0, new int[0])]
        [TestCase(new[] { 1 }, 0, new[] { 1 })]
        [TestCase(new[] { 1 }, 1, new int[0])]
        [TestCase(new[] { 1, 2, 3, 4 }, 3, new[] { 1, 2, 4 })]
        [TestCase(new[] { 1, 2, 3, 4, 3 }, 3, new[] { 1, 2, 4, 3 })]
        [TestCase(new[] { 1, 2, 3, 4, 3 }, 5, new[] { 1, 2, 3, 4, 3 })]
        public void 要素がRemoveFirst出来る(int[] xs, int target, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            Assert.That(array.RemoveFirst(target), Is.EqualTo(xs.Length != expected.Length));
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], 0, new int[0])]
        [TestCase(new[] { 1 }, 0, new[] { 1 })]
        [TestCase(new[] { 1 }, 1, new int[0])]
        [TestCase(new[] { 1, 2, 3, 4 }, 3, new[] { 1, 2, 4 })]
        [TestCase(new[] { 1, 2, 3, 4, 3 }, 3, new[] { 1, 2, 4 })]
        [TestCase(new[] { 1, 2, 3, 4, 3 }, 5, new[] { 1, 2, 3, 4, 3 })]
        public void 要素がRemoveAll出来る(int[] xs, int target, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            Assert.That(array.RemoveAll(n => n == target), Is.EqualTo(xs.Length - expected.Length));
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], 0, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1 }, 0, new int[0])]
        [TestCase(new[] { 1 }, 1, null, ExpectedException=typeof(ArgumentOutOfRangeException))]
        [TestCase(new[] { 1, 2, 3, 4 }, 1, new[] { 1, 3, 4 })]
        public void 指定位置の要素が削除できる(int[] xs, int index, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            array.RemoveAt(index);
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], 0, 0, new int[0])]
        [TestCase(new int[0], 0, 1, null, ExpectedException=typeof(ArgumentException))]
        [TestCase(new[] { 1 }, 0, 0, new[] { 1 })]
        [TestCase(new[] { 1 }, 10, 10, new[] { 1 })]
        [TestCase(new[] { 1 }, 0, 1, new int[0])]
        [TestCase(new[] { 1 }, 0, 2, null, ExpectedException=typeof(ArgumentException))]
        [TestCase(new[] { 1, 2, 3, 4 }, 1, 3, new[] { 1, 4 })]
        [TestCase(new[] { 1, 2, 3, 4 }, 3, 1, new[] { 1, 2 })]
        [TestCase(new[] { 1, 2, 3, 4 }, 0, 4, new int[0])]
        public void 指定範囲の要素が削除できる(int[] xs, int from, int until, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            array.RemoveRange(Range.FromUntil(from, until));
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }

        [Test]
        public void 要素を全削除できる()
        {
            var array = new VarArray<int>(Seq.Init(10, Func.Id));
            array.Clear();
            Assert.That(array.ToArray(), Is.Empty);
        }

        [TestCase(new int[0], 0, null)]
        [TestCase(new[] { 1 }, 0, 1)]
        [TestCase(new[] { 1 }, 1, null)]
        [TestCase(new[] { 1 }, -1, null)]
        [TestCase(new[] { 1, 2, 3, 4 }, 2, 3)]
        [TestCase(new[] { 1, 2, 3, 4 }, 4, null)]
        [TestCase(new[] { 1, 2, 3, 4 }, -1, null)]
        public void 指定要素を取得できる(int[] xs, int index, int? expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            var res = array.TryGet(index);
            if (expected == null)
                Assert.That(res.IsNone, Is.True);
            else
                Assert.That(res.GetOr(int.MinValue), Is.EqualTo(expected.Value));
        }

        [TestCase(new int[0], new int[0])]
        [TestCase(new[] { 1 }, new[] { 1 })]
        [TestCase(new[] { 3, 5, 2, 1, 4 }, new[] { 1, 2, 3, 4, 5 })]
        public void 要素をソートできる(int[] xs, int[] expected)
        {
            var origin = new int[xs.Length];
            xs.CopyTo(origin, 0);
            var array = new VarArray<int>(xs.ToSeq());
            array.SortThis();
            Assert.That(array.ToArray(), Is.EqualTo(expected));
            Assert.That(xs, Is.EqualTo(origin));
        }

        [TestCase(new int[0], new int[0])]
        [TestCase(new[] { 1 }, new[] { 1 })]
        [TestCase(new[] { 3, 5, 2, 1, 4 }, new[] { 5, 4, 3, 2, 1 })]
        public void 要素を指定した順番でソートできる(int[] xs, int[] expected)
        {
            var origin = new int[xs.Length];
            xs.CopyTo(origin, 0);
            var array = new VarArray<int>(xs.ToSeq());
            array.SortThisWith((a, b) => b - a);
            Assert.That(array.ToArray(), Is.EqualTo(expected));
            Assert.That(xs, Is.EqualTo(origin));

            array = new VarArray<int>(xs.ToSeq());
            array.SortThisWith(Comparer.Create<int>((a, b) => b - a));
            Assert.That(array.ToArray(), Is.EqualTo(expected));
            Assert.That(xs, Is.EqualTo(origin));
        }

        [TestCase(new int[0], new int[0])]
        [TestCase(new[] { 1 }, new[] { 1 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2, 1 })]
        public void 要素を反転できる(int[] xs, int[] expected)
        {
            var array = new VarArray<int>(xs.ToSeq());
            array.ReverseThis();
            Assert.That(array.ToArray(), Is.EqualTo(expected));
        }
    }
}
