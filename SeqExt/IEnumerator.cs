using System;

namespace SeqExt
{
    public interface IEnumerator<out T> : System.Collections.Generic.IEnumerator<T> { }
}
