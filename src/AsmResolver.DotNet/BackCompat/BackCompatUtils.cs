using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AsmResolver.DotNet.BackCompat;

internal static class BackCompatUtils
{
    internal const MethodImplOptions AggressiveInlining = (MethodImplOptions)256;

#if NET35
    private static class TypeHolder<T>
    {
        public static readonly T[] Empty = new T[0];
    }
#endif

    [MethodImpl(AggressiveInlining)]
    internal static T[] EmptyArray<T>()
#if !NET35
            => Array.Empty<T>();
#else
        => TypeHolder<T>.Empty;
#endif

    internal static string JoinString<T>(string joiner, IEnumerable<T> enumerable)
    {
#if NET35
        return string.Join(joiner, enumerable.Select(x => x?.ToString()).ToArray());
#else
        return string.Join(joiner, enumerable);
#endif
    }
}
