using System.Text;

namespace AsmResolver.BackCompat;

internal static class BackCompatExtensions
{
#if NET35

    internal static StringBuilder Clear(this StringBuilder b)
    {
        b.Length = 0;
        return b;
    }

#endif
}
