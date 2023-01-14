using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AsmResolver.BackCompat;

internal static class BackCompatExtensions
{
#if NET35

    internal static bool TryDequeue<T>(this Queue<T> queue, [NotNullWhen(true)] out T? result)
    {
        if (queue.Count == 0)
        {
            result = default(T);
            return false;
        }

        result = queue.Dequeue()!;
        return true;
    }

    internal static StringBuilder Clear(this StringBuilder b)
    {
        b.Length = 0;
        return b;
    }

#endif
}
