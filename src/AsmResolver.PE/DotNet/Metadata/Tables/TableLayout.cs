using System.Collections.Generic;
using System.Linq;
using AsmResolver.PE.BackCompat;

#if NET35
using IReadOnlyListColumnLayout = System.Collections.Generic.IList<AsmResolver.PE.DotNet.Metadata.Tables.ColumnLayout>;
#else
using IReadOnlyListColumnLayout = System.Collections.Generic.IReadOnlyList<AsmResolver.PE.DotNet.Metadata.Tables.ColumnLayout>;
#endif


namespace AsmResolver.PE.DotNet.Metadata.Tables
{
    /// <summary>
    /// Provides a description of the layout of a table in the tables metadata stream. This includes information about
    /// the name, data type and size in bytes of each column.
    /// </summary>
    public readonly struct TableLayout
    {
        /// <summary>
        /// Defines a new layout for a metadata table.
        /// </summary>
        /// <param name="columns">The column layouts.</param>
        public TableLayout(params ColumnLayout[] columns)
        {
            Columns = columns;
            RowSize = (uint) columns.Sum(c => c.Size);
        }

        /// <summary>
        /// Gets a collection of columns that this table defines.
        /// </summary>
        public IReadOnlyListColumnLayout Columns
        {
            get;
        }

        /// <summary>
        /// Gets the total size in bytes of a single row in the table.
        /// </summary>
        public uint RowSize
        {
            get;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({BackCompatUtils.JoinString(", ", Columns.Select(c => c.Name))})";
        }

    }
}
