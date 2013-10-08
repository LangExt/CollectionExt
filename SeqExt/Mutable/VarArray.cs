using System;
using LangExt;

namespace SeqExt.Mutable
{
    /// <summary>
    /// 可変長配列です。
    /// </summary>
    public sealed class VarArray<T> : SeqBase<T>, RandomAccessSeq<T>
    {
        readonly System.Collections.Generic.List<T> value;

        /// <summary>
        /// 空の可変長配列を生成します。
        /// </summary>
        public VarArray()
        {
            this.value = new System.Collections.Generic.List<T>();
        }

        /// <summary>
        /// シーケンスの要素を含む可変長配列を生成します。
        /// </summary>
        public VarArray(Seq<T> seq)
        {
            this.value = new System.Collections.Generic.List<T>(seq);
        }

        /// <summary>
        /// 可変長配列のサイズを取得します。
        /// </summary>
        public int Size { get { return this.value.Count; } }

        /// <summary>
        /// 要素を末尾に追加します。この操作は最悪O(N)かかる場合があります。
        /// </summary>
        public void Add(T item) { this.value.Add(item); }

        /// <summary>
        /// シーケンスに含まれるすべての要素を末尾に追加します。この操作は最悪O(N)かかる場合があります。
        /// </summary>
        public void AddAll(Seq<T> seq) { this.value.AddRange(seq); }

        /// <summary>
        /// 要素を指定位置に追加します。この操作にはO(N)かかります。
        /// </summary>
        public void Insert(int index, T item) { this.value.Insert(index, item); }

        /// <summary>
        /// シーケンスに含まれるすべての要素を指定位置に追加します。この操作にはO(N)かかります。
        /// </summary>
        public void InsertAll(int index, Seq<T> seq) { this.value.InsertRange(index, seq); }

        /// <summary>
        /// 指定された項目を先頭から検索し、一つ削除します。指定した項目が見つからなかった場合、このメソッドはfalseを返します。この操作にはO(N)かかります。
        /// </summary>
        public bool RemoveFirst(T item) { return this.value.Remove(item); }

        /// <summary>
        /// 指定された条件に一致する要素をすべて削除します。この操作にはO(N)かかります。
        /// </summary>
        /// <param name="pred">この関数がtrueを返す要素を削除します。</param>
        /// <returns>削除された要素数</returns>
        public int RemoveAll(Func<T, bool> pred) { return this.value.RemoveAll(x => pred(x)); }

        /// <summary>
        /// 指定されたインデックスの要素を削除します。この操作にはO(N)かかります。
        /// </summary>
        public void RemoveAt(int index) { this.value.RemoveAt(index); }

        /// <summary>
        /// 指定された範囲の要素を削除します。この操作にはO(N)かかります。
        /// </summary>
        public void RemoveRange(Range range)
        {
            if (range.Length != 0)
                this.value.RemoveRange(range.Increasing ? range.Begin : range.Last, range.Length);
        }

        /// <summary>
        /// 全ての要素を削除します。
        /// </summary>
        public void Clear() { this.value.Clear(); }

        protected override IEnumerator<T> GetEnumeratorImpl()
        {
            return new Enumerator<T>(this.value.GetEnumerator());
        }

        /// <summary>
        /// 指定されたインデックスの要素を取得・設定します。
        /// </summary>
        public T this[int index]
        {
            get { return this.value[index]; }
            set { this.value[index] = value; }
        }

        /// <summary>
        /// 指定されたインデックスの要素の取得を試みます。
        /// </summary>
        public Option<T> TryGet(int index)
        {
            if (index < this.value.Count && index >= 0)
                return Option.Some(this.value[index]);
            return Option.None;
        }

        /// <summary>
        /// このオブジェクトを既定の並び順でソートします。
        /// 非破壊的にソートしたい場合、LangExt.SeqモジュールのSort拡張メソッドを使います。
        /// </summary>
        public void SortThis() { this.value.Sort(); }

        /// <summary>
        /// このオブジェクトを指定された並び順でソートします。
        /// </summary>
        public void SortThisWith(System.Collections.Generic.IComparer<T> comparer)
        {
            this.value.Sort(comparer);
        }

        /// <summary>
        /// このオブジェクトを指定された並び順でソートします。
        /// </summary>
        public void SortThisWith(Func<T, T, int> comparison)
        {
            this.value.Sort((a, b) => comparison(a, b));
        }

        /// <summary>
        /// このオブジェクトを反転します。
        /// 非破壊的に反転したい場合、LangExt.SeqモジュールのReverse拡張メソッドを使います。
        /// </summary>
        public void ReverseThis()
        {
            this.value.Reverse();
        }
    }
}
