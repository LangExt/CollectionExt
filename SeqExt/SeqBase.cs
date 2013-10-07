using System;
using LangExt;

namespace SeqExt
{
    /// <summary>
    /// シーケンスの実装を簡略化するための抽象クラスです。
    /// </summary>
    public abstract class SeqBase<T> : Seq<T>
    {
        /// <summary>
        /// 現在のオブジェクトを操作するためのEnumeratorを取得します。
        /// </summary>
        protected abstract IEnumerator<T> GetEnumeratorImpl();

        /// <summary>
        /// 現在のオブジェクトを操作するためのEnumeratorを取得します。
        /// </summary>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return GetEnumeratorImpl();
        }

        /// <summary>
        /// 現在のオブジェクトを操作するためのEnumeratorを取得します。
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumeratorImpl();
        }
    }
}
