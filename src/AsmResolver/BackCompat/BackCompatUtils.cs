using System;
using System.Runtime.CompilerServices;

namespace AsmResolver.BackCompat;

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
}
