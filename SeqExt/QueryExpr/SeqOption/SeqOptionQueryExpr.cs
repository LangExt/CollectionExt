using System;
using System.ComponentModel;
using LangExt;
using LangExt.Unsafe;

namespace SeqExt.QueryExpr.SeqOption
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SeqOptionQueryExpr
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<Seq<V>> SelectMany<T, U, V>(
            this Seq<Option<T>> self, Func<T, Option<U>> f, Func<T, U, V> g)
        {
            var res = new System.Collections.Generic.List<V>();
            foreach (var opt in self)
            {
                if (opt.IsNone)
                    return Option.None;
                var t = opt.GetValue(); // 上でチェック済みのため、安全
                var v = f(t).Bind(u => Option.Some(g(t, u)));
                if (v.IsNone)
                    return Option.None;
                res.Add(v.GetValue()); // 上でチェック済みのため、安全
            }
            return Option.Some(res.ToSeq());
        }
    }
}
