using System;
using LangExt;

namespace SeqExt
{
    public interface RandomAccessSeq<out T> : Seq<T>
    {
        T this[int index] { get; }
    }
}
