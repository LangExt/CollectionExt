using System;

namespace SeqExt
{
    /// <summary>
    /// シーケンスに対する反復処理をサポートします。
    /// </summary>
    public interface IEnumerator<out T> : System.Collections.Generic.IEnumerator<T> { }

    /// <summary>
    /// 既存のSystem.Collections.Generic.IEnumeratorをラップしたSeqExt.IEnumerator実装です。
    /// </summary>
    public sealed class Enumerator<T> : IEnumerator<T>
    {
        readonly System.Collections.Generic.IEnumerator<T> value;
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        public Enumerator(System.Collections.Generic.IEnumerator<T> value)
        {
            this.value = value;
        }

        /// <summary>
        /// ラップされたIEnumeratorのCurrentを呼び出します。
        /// </summary>
        public T Current { get { return this.value.Current; } }

        object System.Collections.IEnumerator.Current { get { return this.value.Current; } }

        /// <summary>
        /// ラップされたIEnumeratorのMoveNextを呼び出します。
        /// </summary>
        public bool MoveNext()
        {
            return this.value.MoveNext();
        }

        /// <summary>
        /// ラップされたIEnumeratorのResetを呼び出します。
        /// </summary>
        public void Reset()
        {
            this.value.Reset();
        }

        /// <summary>
        /// ラップされたIEnumeratorのDisposeを呼び出します。
        /// </summary>
        public void Dispose()
        {
            this.value.Dispose();
        }
    }

}
