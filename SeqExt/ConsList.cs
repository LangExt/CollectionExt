using System;
using System.ComponentModel;
using LangExt;

namespace SeqExt
{
    /// <summary>
    /// ConsListに対する関数を提供します。
    /// </summary>
    public static class ConsList
    {
        /// <summary>
        /// 任意のConsListに変換可能な空のConsListです。
        /// </summary>
        public static readonly ConsList<Placeholder> Nil = ConsList<Placeholder>.Nil;

        /// <summary>
        /// 先頭要素と末尾リストからConsListを構築します。
        /// </summary>
        public static ConsList<T> Cons<T>(this T self, ConsList<T> rest)
        {
            return new ConsList<T>.Cons(self, rest);
        }

        /// <summary>空のConsListを作ります。</summary>
        public static ConsList<T> Empty<T>() { return ConsList<T>.Nil; }

        /// <summary>空のConsListを作ります。これの代わりに、Emptyを使った方がいいでしょう。</summary>
        public static ConsList<T> Create<T>() { return ConsList<T>.Nil; }
        /// <summary>要素が1つ含まれたConsListを作ります。これの代わりに、Singletonを使うことを考慮しましょう。</summary>
        public static ConsList<T> Create<T>(T x1) { return Cons(x1, ConsList<T>.Nil); }
        /// <summary>要素が2つ含まれたConsListを作ります。</summary>
        public static ConsList<T> Create<T>(T x1, T x2) { return Cons(x1, Cons(x2, ConsList<T>.Nil)); }
        /// <summary>要素が3つ含まれたConsListを作ります。</summary>
        public static ConsList<T> Create<T>(T x1, T x2, T x3) { return Cons(x1, Cons(x2, Cons(x3, ConsList<T>.Nil))); }
        /// <summary>要素が4つ含まれたConsListを作ります。</summary>
        public static ConsList<T> Create<T>(T x1, T x2, T x3, T x4) { return Cons(x1, Cons(x2, Cons(x3, Cons(x4, ConsList<T>.Nil)))); }

        /// <summary>
        /// 引数に指定された要素が含まれたConsListを作ります。
        /// </summary>
        public static ConsList<T> Create<T>(params T[] values)
        {
            var res = ConsList<T>.Nil;
            var len = values.Length;
            for (int i = 0; i < len; i++)
            {
                var idx = len - i - 1;
                res = new ConsList<T>.Cons(values[idx], res);
            }
            return res;
        }

        /// <summary>
        /// fを元にn要素のシーケンスを生成します。
        /// nが大きい順に(リストの後ろから)呼び出されるため、副作用のある関数を渡す場合は注意してください。
        /// </summary>
        public static ConsList<T> Init<T>(int n, Func<int, T> f)
        {
            if (n < 0) throw new ArgumentOutOfRangeException();
            var res = ConsList<T>.Nil;
            for (int i = 0; i < n; i++)
            {
                var m = n - i - 1;
                res = new ConsList<T>.Cons(f(m), res);
            }
            return res;
        }

        /// <summary>
        /// 指定した要素を含むn要素のConsListを生成します。
        /// </summary>
        public static ConsList<T> Repeat<T>(int n, T t)
        {
            if (n < 0) throw new ArgumentOutOfRangeException();
            var res = ConsList<T>.Nil;
            for (int i = 0; i < n; i++)
                res = new ConsList<T>.Cons(t, res);
            return res;
        }

        /// <summary>
        /// 指定した要素のみを含む1要素のConsListを生成します。
        /// </summary>
        public static ConsList<T> Singleton<T>(T t)
        {
            return new ConsList<T>.Cons(t, ConsList<T>.Nil);
        }

        /// <summary>
        /// 配列をConsListに変換します。
        /// </summary>
        public static ConsList<T> OfArray<T>(T[] array)
        {
            return Create(array);
        }

        /// <summary>
        /// シーケンスをConsListに変換します。
        /// 無限のシーケンスは扱えません。
        /// </summary>
        public static ConsList<T> OfSeq<T>(Seq<T> seq)
        {
            return Create(seq.ToArray());
        }

        /// <summary>
        /// シーケンスをConsListに変換します。
        /// 無限のシーケンスは扱えません。
        /// </summary>
        public static ConsList<T> ToConsList<T>(this Seq<T> self)
        {
            return Create(self.ToArray());
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b) { return object.Equals(a, b); }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object a, object b) { return object.ReferenceEquals(a, b); }
    }

    /// <summary>
    /// 任意の要素を格納可能な単方向の連結リストです。
    /// このシーケンスはイミュータブルです。
    /// </summary>
    public abstract class ConsList<T> : Seq<T>, IEquatable<ConsList<T>>
    {
        ConsList() { }
        /// <summary>
        /// 空のConsListです。
        /// </summary>
        public static readonly ConsList<T> Nil = new NilType();

        /// <summary>
        /// Placeholder型の空のConsListを、任意の型のConsListに暗黙変換します。
        /// 空ではないリストを渡した場合は、例外が発生します。
        /// </summary>
        public static implicit operator ConsList<T>(ConsList<Placeholder> nil)
        {
            if (nil.IsNotEmpty())
                throw new InvalidOperationException();
            return ConsList<T>.Nil;
        }

        /// <summary>
        /// 現在のオブジェクトにConsListを連結したConsListを生成します。
        /// </summary>
        public abstract ConsList<T> Append(ConsList<T> other);

        /// <summary>
        /// 現在のオブジェクトが空かどうかを返します。 
        /// </summary>
        public abstract bool IsEmpty { get; }
        /// <summary>
        /// 現在のオブジェクトを分解して、
        /// 要素を持つ場合はその要素と残りのリストを使った処理を、
        /// 要素がない場合はその場合の処理を実行します。
        /// 1つしか要素がなかった場合、残りのリストにはNilが渡されます。
        /// </summary>
        /// <param name="Cons">リストに要素が含まれる場合の処理</param>
        /// <param name="Nil">リストに要素が含まれない場合の処理</param>
        /// <returns>処理の結果</returns>
        public abstract U Match<U>(Func<T, ConsList<T>, U> Cons, Func<U> Nil);

        /// <summary>
        /// 現在のオブジェクトが空でないかどうかを返します。
        /// </summary>
        public bool IsNotEmpty { get { return this.IsEmpty == false; } }
        /// <summary>
        /// 現在のオブジェクトを分解して、
        /// 要素を持つ場合はその要素と残りのリストを使った処理を、
        /// 要素がない場合はその場合の処理を実行します。
        /// 1つしか要素がなかった場合、残りのリストにはNilが渡されます。
        /// </summary>
        /// <param name="Cons">リストに要素が含まれる場合の処理</param>
        /// <param name="Nil">リストに要素が含まれない場合の処理</param>
        public void Match(Action<T, ConsList<T>> Cons, Action Nil)
        {
            Match((x, xs) => { Cons(x, xs); return Unit._; }, () => { Nil(); return Unit._; });
        }

        /// <summary>
        /// 現在のオブジェクトを走査するためのEnumeratorを取得します。
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal class Cons : ConsList<T>
        {
            internal readonly T Head;
            internal /* readonly */ ConsList<T> Tail;
            internal Cons(T head, ConsList<T> tail)
            {
                this.Head = head;
                this.Tail = tail;
            }

            static Cons Singleton(T value) { return new Cons(value, ConsList<T>.Nil); }

            public override ConsList<T> Append(ConsList<T> other)
            {
                //return this.Head.Cons(this.Tail.Append(other));
                var first = Singleton(this.Head);
                var crnt = first;
                foreach (var x in this.Tail)
                {
                    var newTail = Singleton(x);
                    crnt.Tail = newTail;
                    crnt = newTail;
                }
                crnt.Tail = other;
                return first;
            }

            public override bool IsEmpty { get { return false; } }

            public override U Match<U>(Func<T, ConsList<T>, U> Cons, Func<U> Nil)
            {
                return Cons(this.Head, this.Tail);
            }
        }

        class NilType : ConsList<T>
        {
            public override ConsList<T> Append(ConsList<T> other) { return other; }

            public override bool IsEmpty { get { return true; } }

            public override U Match<U>(Func<T, ConsList<T>, U> Cons, Func<U> Nil)
            {
                return Nil();
            }
        }

        class Enumerator : IEnumerator<T>
        {
            ConsList<T> next;
            internal Enumerator(ConsList<T> target) { this.next = target; }

            public bool MoveNext()
            {
                return this.next.Match(
                    (x, xs) =>
                    {
                        this.crnt = x;
                        this.next = xs;
                        return true;
                    },
                    () => false);
            }

            T crnt;
            public T Current { get { return this.crnt; } }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose() { }
        }

        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを判定します。 
        /// ConsList.Nilで返された値と、型付きのNilを比較した場合にfalseが返される点に注意してください。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するConsList</param>
        /// <returns>現在のオブジェクトがobjで</returns>
        public bool Equals(ConsList<T> other)
        {
            if (this.IsEmpty)
                return other.IsEmpty;
            if (other.IsEmpty)
                return false;

            var itor1 = this.GetEnumerator();
            var itor2 = other.GetEnumerator();
            while (itor1.MoveNext())
            {
                if (itor2.MoveNext() == false)
                    return false;
                var crnt1 = itor1.Current;
                var crnt2 = itor2.Current;
                if (object.Equals(crnt1, crnt2) == false)
                    return false;
            }
            if (itor2.MoveNext())
                return false;
            return true;
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            var other = obj as ConsList<T>;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            var hash = 31;
            foreach (var x in this)
                hash ^= x == null ? 0 : x.GetHashCode();
            return hash;
        }

        /// <summary>
        /// このオブジェクトの文字列表現を取得します。
        /// </summary>
        public override string ToString()
        {
            if (this.IsEmpty)
                return "Nil";
            var buf = new System.Text.StringBuilder("ConsList(");
            foreach (var x in this)
                buf.Append(x.ToStr()).Append(", ");
            buf.Remove(buf.Length - 2, 2);
            return buf.Append(")").ToString();
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b) { return object.Equals(a, b); }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object a, object b) { return object.ReferenceEquals(a, b); }
    }
}
