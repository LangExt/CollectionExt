using System;
using LangExt;
using SeqExt;
using NUnit.Framework;

namespace SeqExt.Tests
{
    [TestFixture]
    public class ConsListTest
    {
        [Test]
        public void 空のConsListが作れる()
        {
            ConsList<int> nil = ConsList.Nil;
            Assert.That(nil, Is.Empty);
            Assert.That(ConsList<int>.Nil, Is.Empty);
            Assert.That(ConsList.Create<string>(), Is.Empty);
        }

        [Test]
        public void ConsでConsListが作れる()
        {
            Assert.That((1).Cons(ConsList<int>.Nil).ToArray(), Is.EqualTo(new[] { 1 }));
            Assert.That((1).Cons((2).Cons(ConsList<int>.Nil)).ToArray(), Is.EqualTo(new[] { 1, 2 }));
            Assert.That((1).Cons((2).Cons((3).Cons(ConsList<int>.Nil))).ToArray(), Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That((1).Cons((2).Cons((3).Cons((4).Cons(ConsList<int>.Nil)))).ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
            Assert.That((1).Cons((2).Cons((3).Cons((4).Cons((5).Cons(ConsList<int>.Nil))))).ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void CreateでConsListが作れる()
        {
            Assert.That(ConsList.Create(1).ToArray(), Is.EqualTo(new[] { 1 }));
            Assert.That(ConsList.Create(1, 2).ToArray(), Is.EqualTo(new[] { 1, 2 }));
            Assert.That(ConsList.Create(1, 2, 3).ToArray(), Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(ConsList.Create(1, 2, 3, 4).ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
            Assert.That(ConsList.Create(1, 2, 3, 4, 5).ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [TestCase(new int[0], new int[0], new int[0])]
        [TestCase(new int[0], new[] { 1 }, new[] { 1 })]
        [TestCase(new int[0], new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 1 }, new int[0], new[] { 1 })]
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 1, 2, 3, 4 }, new[] { 1, 1, 2, 3, 4 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new int[0], new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1 }, new[] { 1, 2, 3, 4, 1 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4, 1, 2, 3, 4 })]
        public void ふたつのConsListを連結できる(int[] xs, int[] ys, int[] expected)
        {
            var lst1 = ConsList.Create(xs);
            var lst2 = ConsList.Create(ys);
            var expectedLst = ConsList.Create(expected);
            Assert.That(lst1.Append(lst2), Is.EqualTo(expectedLst));
        }

        [TestCase(new int[0], new int[0])]
        [TestCase(new int[0], new[] { 1 })]
        [TestCase(new int[0], new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 1 }, new int[0])]
        [TestCase(new[] { 1 }, new[] { 2 })]
        [TestCase(new[] { 1 }, new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new int[0])]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
        public void Appendしても元のリストは変わらない(int[] xs, int[] ys)
        {
            var lst1 = ConsList.Create(xs);
            var lst2 = ConsList.Create(ys);
            lst1.Append(lst2);
            Assert.That(lst1, Is.EqualTo(ConsList.Create(xs)));
            Assert.That(lst2, Is.EqualTo(ConsList.Create(ys)));
        }

        [TestCase(new int[0], "Nil")]
        [TestCase(new[] { 1 }, "ConsList(1)")]
        [TestCase(new[] { 1, 2, 3 }, "ConsList(1, 2, 3)")]
        public void ConsListを文字列化できる(int[] xs, string expected)
        {
            Assert.That(ConsList.Create(xs).ToString(), Is.EqualTo(expected));
        }

        [TestCase(new int[0], new int[0], true)]
        [TestCase(new int[0], new[] { 1 }, false)]
        [TestCase(new[] { 1 }, new int[0], false)]
        [TestCase(new[] { 1 }, new[] { 1 }, true)]
        [TestCase(new[] { 1 }, new[] { 2 }, false)]
        [TestCase(new[] { 1 }, new[] { 1, 0 }, false)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2 }, false)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 0 }, false)]
        public void ConsListどうしの比較ができる(int[] xs, int[] ys, bool expected)
        {
            var cons1 = ConsList.Create(xs);
            var cons2 = ConsList.Create(ys);
            Assert.That(cons1.Equals(cons2), Is.EqualTo(expected));
        }
    }
}
