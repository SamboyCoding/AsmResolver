using System;
using System.Linq;
using System.Security.Cryptography;
using AsmResolver.PE.BackCompat;
using AsmResolver.PE.DotNet.Metadata.Strings;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace AsmResolver.PE.DotNet.Metadata.Tables
{
    /// <summary>
    /// Provides an implementation for the Type Reference Hash (TRH) as introduced by GData.
    /// This hash is used as an alternative to the ImpHash to identify malware families based on
    /// the type references imported by the .NET image.
    ///
    /// Reference:
    /// https://www.gdatasoftware.com/blog/2020/06/36164-introducing-the-typerefhash-trh
    /// </summary>
    public static class TypeReferenceHash
    {
        /// <summary>
        /// If the provided image is a .NET image, computes the Type Reference Hash (TRH) as introduced by GData to
        /// identify malware based on its imported type references.
        /// </summary>
        /// <param name="image">The image to get the TRH from.</param>
        /// <returns>The hash.</returns>
        /// <exception cref="ArgumentException">Occurs when the provided image does not contain .NET metadata.</exception>
        public static byte[] GetTypeReferenceHash(this IPEImage image)
        {
            var metadata = image.DotNetDirectory?.Metadata;
            if (metadata is null)
                throw new ArgumentException("Portable executable does not contain a .NET image.");

            return metadata.GetTypeReferenceHash();
        }

        /// <summary>
        /// Computes the Type Reference Hash (TRH) as introduced by GData to identify malware based on its
        /// imported type references.
        /// </summary>
        /// <param name="metadata">The metadata directory to get the TRH from.</param>
        /// <returns>The hash.</returns>
        /// <exception cref="ArgumentException">Occurs when the provided image does not contain .NET metadata.</exception>
        public static byte[] GetTypeReferenceHash(this IMetadata metadata)
        {
            var tablesStream = metadata.GetStream<TablesStream>();
            var stringsStream = metadata.GetStream<StringsStream>();

            var table = tablesStream.GetTable<TypeReferenceRow>(TableIndex.TypeRef);

            var elements = table
                .OrderBy(row => stringsStream.GetStringByIndex(row.Namespace))
                .ThenBy(row => stringsStream.GetStringByIndex(row.Name))
                .Select(row =>
                    $"{stringsStream.GetStringByIndex(row.Namespace)}-{stringsStream.GetStringByIndex(row.Name)}");

            string fullString = BackCompatUtils.JoinString(",", elements);

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fullString));
        }
    }
}
