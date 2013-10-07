using System;
using LangExt;

namespace SeqExt.Mutable
{
    public sealed class VarArray<T> : SeqBase<T>, RandomAccessSeq<T>
    {
        readonly System.Collections.Generic.List<T> value;

        public VarArray()
        {
            this.value = new System.Collections.Generic.List<T>();
        }

        public VarArray(Seq<T> seq)
        {
            this.value = new System.Collections.Generic.List<T>(seq);
        }

        public int Size { get { return this.value.Count; } }

        public void Add(T item) { this.value.Add(item); }

        public void AddAll(Seq<T> seq) { this.value.AddRange(seq); }

        public void Insert(int index, T item) { this.value.Insert(index, item); }

        public void InsertAll(int index, Seq<T> seq) { this.value.InsertRange(index, seq); }

        public bool RemoveFirst(T item) { return this.value.Remove(item); }

        public int RemoveAll(Func<T, bool> pred) { return this.value.RemoveAll(x => pred(x)); }

        public void RemoveAt(int index) { this.value.RemoveAt(index); }

        public void RemoveRange(Range range)
        {
            if (range.Length != 0)
                this.value.RemoveRange(range.Increasing ? range.Begin : range.Last, range.Length);
        }

        public void Clear() { this.value.Clear(); }

        protected override IEnumerator<T> GetEnumeratorImpl()
        {
            return new Enumerator<T>(this.value.GetEnumerator());
        }

        public T this[int index]
        {
            get { return this.value[index]; }
            set { this.value[index] = value; }
        }

        public Option<T> TryGet(int index)
        {
            if (index < this.value.Count && index >= 0)
                return Option.Some(this.value[index]);
            return Option.None;
        }

        public void SortThis() { this.value.Sort(); }

        public void SortThisWith(System.Collections.Generic.IComparer<T> comparer)
        {
            this.value.Sort(comparer);
        }

        public void SortThisWith(Func<T, T, int> comparison)
        {
            this.value.Sort((a, b) => comparison(a, b));
        }

        public void ReverseThis()
        {
            this.value.Reverse();
        }
    }
}
