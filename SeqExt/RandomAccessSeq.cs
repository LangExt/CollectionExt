using System;
using LangExt;

namespace SeqExt
{
    /// <summary>
    /// ランダムアクセス可能なシーケンスを表すインターフェイスです。
    /// </summary>
    public interface RandomAccessSeq<out T> : Seq<T>
    {
        /// <summary>
        /// 指定されたインデックスの要素を取得します。
        /// </summary>
        /// <param name="index">取得するインデックス</param>
        /// <returns>指定されたインデックスに対応する要素</returns>
        T this[int index] { get; }
    }
}
