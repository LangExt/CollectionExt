using System;

namespace SeqExt
{
    /// <summary>
    /// シーケンスに対する反復処理をサポートします。
    /// </summary>
    public interface IEnumerator<out T> : System.Collections.Generic.IEnumerator<T> { }
}
