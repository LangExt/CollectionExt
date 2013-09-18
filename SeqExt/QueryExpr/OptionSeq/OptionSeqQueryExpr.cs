using System;
using System.ComponentModel;
using LangExt;
using LangExt.Unsafe;

namespace SeqExt.QueryExpr.OptionSeq
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OptionSeqQueryExpr
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<Seq<V>> SelectMany<T, U, V>(
            this Option<Seq<T>> self, Func<T, Option<U>> f, Func<T, U, V> g)
        {
            if (self.IsNone)
                return Option.None;
            var xs = self.GetValue(); // 上でチェック済みのため、安全
            var res = new System.Collections.Generic.List<V>();
            foreach (var x in xs)
            {
                var v = f(x).Bind(u => Option.Some(g(x, u)));
                if (v.IsNone)
                    return Option.None;
                res.Add(v.GetValue()); // 上でチェック済みのため、安全
            }
            return Option.Some(res.ToSeq());
        }
    }
}
