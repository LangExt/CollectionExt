using System;
using LangExt;
using NUnit.Framework;
using SeqExt.QueryExpr.SeqOption;

namespace SeqExt.Tests
{
    [TestFixture]
    public class SeqOptionQueryExprTest
    {
        [Test]
        public void クエリ式が使える()
        {
            var xs = Range.FromTo(1, 5).ToSeq().Map(Option.Some);
            var res =
                from x in xs
                from y in Option.Some("hoge")
                select y + x;
            Assert.That(res.GetOr(null).ToArray(), Is.EqualTo(new[] { "hoge1", "hoge2", "hoge3", "hoge4", "hoge5" }));
        }
    }
}
