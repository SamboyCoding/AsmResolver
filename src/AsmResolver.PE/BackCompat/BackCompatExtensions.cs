using System.IO;

namespace AsmResolver.PE.BackCompat;

internal static class BackCompatExtensions
{
#if NET35
    internal static void CopyTo(this Stream s, Stream other)
    {
        byte[] buffer = new byte[4096];
        int read;
        while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
            other.Write(buffer, 0, read);
    }
#endif
}
