using System;
using LangExt;

namespace SeqExt
{
    public static class ConsList
    {
        public static readonly ConsList<Placeholder> Nil = ConsList<Placeholder>.Nil;

        public static ConsList<T> Cons<T>(this T self, ConsList<T> rest)
        {
            return new ConsList<T>.Cons(self, rest);
        }

        public static ConsList<T> Create<T>() { return ConsList<T>.Nil; }
        public static ConsList<T> Create<T>(T x1) { return Cons(x1, ConsList<T>.Nil); }
        public static ConsList<T> Create<T>(T x1, T x2) { return Cons(x1, Cons(x2, ConsList<T>.Nil)); }
        public static ConsList<T> Create<T>(T x1, T x2, T x3) { return Cons(x1, Cons(x2, Cons(x3, ConsList<T>.Nil))); }
        public static ConsList<T> Create<T>(T x1, T x2, T x3, T x4) { return Cons(x1, Cons(x2, Cons(x3, Cons(x4, ConsList<T>.Nil)))); }

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
    }

    public abstract class ConsList<T> : Seq<T>, IEquatable<ConsList<T>>
    {
        ConsList() { }
        public static readonly ConsList<T> Nil = new NilType();

        public static implicit operator ConsList<T>(ConsList<Placeholder> nil)
        {
            if (nil.IsNotEmpty())
                throw new InvalidOperationException();
            return ConsList<T>.Nil;
        }

        public abstract ConsList<T> Append(ConsList<T> other);

        public abstract bool IsEmpty { get; }
        public abstract U Match<U>(Func<T, ConsList<T>, U> Cons, Func<U> Nil);

        public bool IsNotEmpty { get { return this.IsEmpty == false; } }
        public void Match(Action<T, ConsList<T>> Cons, Action Nil)
        {
            Match((x, xs) => { Cons(x, xs); return Unit._; }, () => { Nil(); return Unit._; });
        }

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

        public override bool Equals(object obj)
        {
            var other = obj as ConsList<T>;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            var hash = 31;
            foreach (var x in this)
                hash ^= x == null ? 0 : x.GetHashCode();
            return hash;
        }

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
    }
}
